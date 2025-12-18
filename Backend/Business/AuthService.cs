using AutoMapper;
using Business.Abstract;
using Business.Validations;
using Core.Aspects.Validation;
using Core.Entites;
using Core.Helpers.Methods;
using Entities;
using Entities.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repositories.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Business
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;
		private readonly ILogger<UserService> _logger;
		private readonly IMapper _mapper;


		public AuthService(IUserRepository userRepository, IEmailService emailService, ILogger<UserService> logger, IMapper mapper, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_configuration = configuration;
			_emailService = emailService;
			_logger = logger;
			_mapper = mapper;
		}

		[ValidationAspect(typeof(RegisterDtoValidator))]
		public async Task<ApiResponse<UserRequestDto>> RegisterAsync(RegisterDto registerDto)
		{
			try
			{
				if (!registerDto.IsAgreedToPrivacyPolicy)
					return ApiResponse<UserRequestDto>.ErrorResponse("Kişisel verilerin işlenmesini kabul etmeniz gerekmektedir.");

				// Email benzersizlik kontrolü
				var existingUser = await _userRepository.GetFirstAsync(u => u.Email == registerDto.Email);
				if (existingUser != null)
					return ApiResponse<UserRequestDto>.ErrorResponse("Bu email adresi zaten kullanımda.");

				// Şifre hashleme
				var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

				var user = new User
				{
					Name = registerDto.Name,
					Surname = registerDto.Surname,
					Email = registerDto.Email,
					PasswordHash = passwordHash,
					Role = UserRole.Individual,
					CreatedDate = DateTime.UtcNow
				};

				var createdUser = await _userRepository.AddAsync(user);


				_logger.LogInformation("Yeni kullanıcı kaydedildi. Email: {Email}, Zaman: {Time}", createdUser.Email, DateTime.UtcNow);
				return ApiResponse<UserRequestDto>.SuccessResponse(_mapper.Map<UserRequestDto>(createdUser), "Kullanıcı başarıyla kaydedildi.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Kullanıcı kaydı sırasında bir hata oluştu. Email: {Email}", registerDto.Email);
				return ApiResponse<UserRequestDto>.ErrorResponse($"Kayıt başarısız: {ex.Message}");
			}
		}

		[ValidationAspect(typeof(LoginDtoValidator))]
		public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
		{
			try
			{
				var user = await _userRepository.GetFirstAsync(u => u.Email == loginDto.Email);
				if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
					return ApiResponse<AuthResponseDto>.ErrorResponse("Email veya şifre hatalı.");


				// JWT token oluşturma
				var accessToken = GenerateJwtToken(user);

				//refresh token
				var refreshToken = TokenHelper.GenerateSecureToken();
				user.RefreshToken = refreshToken;
				user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // örnek: 7 gün geçerli

				user.LastLoginDate = DateTime.UtcNow;
				await _userRepository.UpdateAsync(user);
				_logger.LogInformation("Kullanıcı başarıyla giriş yaptı. Email: {Email}, Zaman: {Time}", user.Email, DateTime.UtcNow);
				return ApiResponse<AuthResponseDto>.SuccessResponse(new AuthResponseDto
				{
					AccessToken = accessToken,
					RefreshToken = refreshToken
				}, "Giriş başarılı.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Login sırasında bir hata oluştu. Email: {Email}", loginDto.Email);
				return ApiResponse<AuthResponseDto>.ErrorResponse($"Giriş başarısız: {ex.Message}");
			}
		}

		public async Task<ApiResponse<bool>> LogoutAsync(int userId)
		{
			try
			{
				var user = await _userRepository.GetByIdAsync(userId);
				if (user == null)
					return ApiResponse<bool>.ErrorResponse("Kullanıcı bulunamadı.");

				user.LastLogoutDate = DateTime.UtcNow;
				user.RefreshToken = null;
				user.RefreshTokenExpiry = null;

				await _userRepository.UpdateAsync(user);
				return ApiResponse<bool>.SuccessResponse(true, "Çıkış başarılı.");
			}
			catch (Exception ex)
			{
				return ApiResponse<bool>.ErrorResponse($"Çıkış işlemi başarısız: {ex.Message}");
			}
		}

		[ValidationAspect(typeof(ResetPasswordDtoValidator))]
		public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
		{
			if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
			{
				return ApiResponse<bool>.ErrorResponse("Şifreler birbiriyle uyuşmuyor.");
			}
			var decodedToken = WebUtility.UrlDecode(resetPasswordDto.Token);
			decodedToken = decodedToken.Replace(" ", "+");

			var user = await _userRepository.GetFirstAsync(u => u.Email == resetPasswordDto.Email);
			if (user == null)
				return ApiResponse<bool>.ErrorResponse("Bu email ile kayıtlı kullanıcı yok.");

			if (user.PasswordResetToken != decodedToken)
			{
				return ApiResponse<bool>.ErrorResponse("Token hatalı! Veritabanındaki ile uyuşmuyor.");
			}

			if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
				return ApiResponse<bool>.ErrorResponse("Token süresi dolmuş.");

			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
			user.PasswordResetToken = null;
			user.PasswordResetTokenExpiry = null;

			await _userRepository.UpdateAsync(user);
			return ApiResponse<bool>.SuccessResponse(true, "Şifre başarıyla sıfırlandı.");
		}

		public Task<ApiResponse<bool>> ValidateTokenAsync(string token)
		{
			throw new NotImplementedException();
		}

		[ValidationAspect(typeof(ForgotPasswordDtoValidator))]
		public async Task<ApiResponse<bool>> ForgotPasswordAsync(string email)
		{
			try
			{
				var user = await _userRepository.GetFirstAsync(u => u.Email == email);
				if (user == null)
					return ApiResponse<bool>.ErrorResponse("Bu email adresiyle kayıtlı kullanıcı bulunamadı.");

				// Şifre sıfırlama tokeni oluştur
				var token = TokenHelper.GenerateSecureToken();
				var encodedToken = WebUtility.UrlEncode(token);
				user.PasswordResetToken = token;
				user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
				await _userRepository.UpdateAsync(user);

				var resetLink = $"https://planova-4scg.onrender.com/reset-password?token={encodedToken}&email={email}";
				await _emailService.SendPasswordResetEmailAsync(email, resetLink);
				return ApiResponse<bool>.SuccessResponse(true, "Şifre sıfırlama bağlantısı gönderildi.");
			}
			catch (Exception ex)
			{
				return ApiResponse<bool>.ErrorResponse($"Şifre sıfırlama hatası: {ex.Message}");
			}
		}

		public Task<ApiResponse<AuthResponseDto>> LoginWithGoogleAsync(GoogleLoginDto googleLoginDto)
		{
			throw new NotImplementedException();
		}

		

		public Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string token)
		{
			throw new NotImplementedException();
		}

		private string GenerateJwtToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Role, user.Role.ToString()),

				}),
				Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature),
				Issuer = _configuration["Jwt:Issuer"],
				Audience = _configuration["Jwt:Audience"]
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}

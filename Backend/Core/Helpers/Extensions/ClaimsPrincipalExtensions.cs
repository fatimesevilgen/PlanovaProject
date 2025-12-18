using System.Security.Claims;

namespace Core.Helpers.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		// JWT token içindeki kullanıcı ID'sini alır
		public static int GetUserId(this ClaimsPrincipal user)
		{
			var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userIdClaim))
				throw new UnauthorizedAccessException("User ID claim not found.");
			return int.Parse(userIdClaim);
		}

		// JWT token içindeki kullanıcı email'ini alır
		public static string GetUserEmail(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
		}

		// JWT token içindeki kullanıcı rolünü alır
		public static string GetUserRole(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
		}

		// JWT token içindeki kullanıcı adı
		public static string GetUserName(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
		}
	}
}

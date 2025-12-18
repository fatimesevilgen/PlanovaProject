using Business;
using Business.Abstract;
using Business.Concrete;
using Business.Mappings;
using Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Abstract;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Business.Validations;

namespace WebApi
{
	/// <summary>
	/// uygulamanın ana giriş noktası sınıfı
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Uygulamanın çalıştırıldığı ana metot
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            
			builder.Services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			})
			.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserUpdateDtoValidator>());


			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
				// JWT i�in Swagger yap�land�rmas�
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				   {
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});


			// PostgreSQL baglantısı
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
					npgsqlOptionsAction: npgsqlOptions =>
					{
						npgsqlOptions.EnableRetryOnFailure(
							maxRetryCount: 5,
							maxRetryDelay: TimeSpan.FromSeconds(30),
							errorCodesToAdd: null);
					}));

			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IPrizeRepository, PrizeRepository>();
			builder.Services.AddScoped<IUserPrizeRepository, UserPrizeRepository>();
			builder.Services.AddScoped<IHabitRepository, HabitRepository>();
			builder.Services.AddScoped<IHabitLogRepository, HabitLogRepository>();
			builder.Services.AddScoped<IHabitService, HabitManager>();
			builder.Services.AddScoped<IHabitRepository, HabitRepository>();
			builder.Services.AddScoped<IHabitProgressRepository, HabitProgressRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IUserStatisticsService, UserStatisticsService>();
			builder.Services.AddHttpClient();
			builder.Services.AddScoped<IChatbotService, ChatbotService>();
			builder.Services.AddScoped<IPrizeService, PrizeManager>();



			//AutoMapper
			builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

			// JWT Authentication
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration["Jwt:Issuer"],
						ValidAudience = builder.Configuration["Jwt:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
					};
				});

			var app = builder.Build();

			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			// BAŞLANGIÇ VERİSİ EKLEME

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try

				{
					// DbContext'i çağırıyoruz (Adı AppDbContext ise)
					var context = services.GetRequiredService<Repositories.AppDbContext>();

					// Eğer hiç kategori yoksa bir tane ekle
					if (!context.Set<Entities.Category>().Any())
					{
						context.Set<Entities.Category>().Add(new Entities.Category
						{
							Name = "Genel",
							Icon = "fa-home",

						});
						context.SaveChanges();
						Console.WriteLine("--- OTOMATİK: 'Genel' kategorisi oluşturuldu ---");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Seed Data hatası: " + ex.Message);
				}
			}

			app.Run();
		}
	}
}

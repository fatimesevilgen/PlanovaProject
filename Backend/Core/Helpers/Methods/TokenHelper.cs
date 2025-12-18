using System.Security.Cryptography;

namespace Core.Helpers.Methods
{
	public static class TokenHelper
	{
		public static string GenerateSecureToken(int size = 32)
		{
			var randomNumber = new byte[size];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}
}

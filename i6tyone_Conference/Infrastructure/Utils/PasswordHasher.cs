using Isopoh.Cryptography.Argon2;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public class PasswordHasher
    {
        public static Task<string> PasswordHashAsync(string password)
        {
            return Task.Run(() => Argon2.Hash(password));
        }

        public static Task<bool> VerifyAsync(string dbPasswordHash, string inputPassword)
        {
            return Task.Run(() => Argon2.Verify(dbPasswordHash, inputPassword));
        }
    }
}

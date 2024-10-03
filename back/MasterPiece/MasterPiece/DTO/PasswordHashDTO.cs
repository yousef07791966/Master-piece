using System.Text;

namespace MasterPiece.DTO
{
    public class PasswordHashDTO
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hm = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hm.Key; // The Key property provides a randomly generated salt.
                passwordHash = hm.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {

            using (var hm = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }


    }
}

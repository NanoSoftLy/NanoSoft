using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

namespace NanoSoft.Extensions
{
    [PublicAPI]
    public static class EncryptionExtensions
    {
        public static string ToMd5([NotNull] this string input)
        {
            Check.NotNull(input, nameof(input));

            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);


            // step 2, convert byte array to hex string
            var sb = new StringBuilder();

            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString().ToLower();
        }


        public static string ToBCryptHash(this string input, string salt)
            => BCrypt.Net.BCrypt.HashPassword(input, salt);
    }
}
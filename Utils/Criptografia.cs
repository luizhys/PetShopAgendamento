using System.Security.Cryptography;
using System.Text;

namespace PetShopAgendamento.Utils
{
    public static class Criptografia
    {
        public static string GerarHash(string texto)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(texto);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
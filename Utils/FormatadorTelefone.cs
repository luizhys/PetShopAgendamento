using System.Text.RegularExpressions;

namespace PetShopAgendamento.Utils
{
    public static class FormatadorTelefone
    {
        // Formatar telefone para as views
        public static string Formatar(string telefone)
        {
            if (string.IsNullOrEmpty(telefone)) return "—";
            var digits = Regex.Replace(telefone, @"\D", "");
            if (digits.Length == 11)
                return $"({digits[..2]}) {digits[2..7]}-{digits[7..]}";
            if (digits.Length == 10)
                return $"({digits[..2]}) {digits[2..6]}-{digits[6..]}";
            return telefone;
        }
    }
}
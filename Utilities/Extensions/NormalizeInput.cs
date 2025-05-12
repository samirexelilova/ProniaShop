using static System.Net.Mime.MediaTypeNames;

namespace ProniaShop.Utilities.Extensions
{
    public static class NormalizeInput
    {
        public static string NormalizeName(this string input)
        {
            input = input.Trim().ToLower();
            if (input.Any(char.IsDigit))
               return null;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}

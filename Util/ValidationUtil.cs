using System.Text.RegularExpressions;

namespace GameProducer.Util
{
    public class ValidationUtil
    {
        public static bool IsValidPhoneNumber(string number)
        {
            return Regex.Match(number, @"^[1-9]{2}[1-9]{2}(?:[2-8]|9[1-9])[0-9]{3}[0-9]{4}$").Success;
        }
    }
}

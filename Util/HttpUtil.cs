using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace GameProducer.Util
{
    public class HttpUtil

    {
        public static bool IsSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            return ((int)httpStatusCode >= 200) && ((int)httpStatusCode <= 299);
        }

        public static bool IsValidEmail(string Email)
        {
            return new EmailAddressAttribute().IsValid(Email);
        }
    }
}

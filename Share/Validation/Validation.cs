using System.Text.RegularExpressions;

namespace Share.Validation
{
    public class RegexValidation
    {
        public static string PHONENUMBER_REGEX = "^[0]{1}[0-9]{10}$";
        public static string ALT_PHONENUMBER_REGEX = "^[+234]{4}[0-9]{10}$";

        public static string EMAIL_REGEX = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static string EMAIL_REGEX2 = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public static dynamic reg = new Regex(@"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$", RegexOptions.IgnoreCase);
    }
}

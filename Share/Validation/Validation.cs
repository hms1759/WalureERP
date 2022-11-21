using System.Text.RegularExpressions;

namespace Share.Validation
{
    public class RegexValidation
    {

        //public static string PHONENUMBER_REGEX = "^[0]{1}[0-9]{10}$";
        public static string PHONENUMBER_REGEX = "/^0[0-9]{10}$/";

        public static string EMAIL_REGEX = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";


    }
}

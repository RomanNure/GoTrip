using System.Text.RegularExpressions;

namespace GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns
{
    public class UserFieldsPatterns
    {
        public static readonly Regex LOGIN_PATTERN = new Regex("^[a-zA-Z0-9]{8,20}$");
        public static readonly Regex PASSWORD_PATTERN = new Regex("^[a-zA-Z0-9]{8,30}$");
        public static readonly Regex EMAIL_PATTERN = new Regex("^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" +
            "(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|" +
            "\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\" +
            "\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)" +
            "+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}" +
            "(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-" +
            "\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)])$");

        public static Regex FIRST_NAME_PATTERN = new Regex("^[a-zA-Z]{2,47}$");
        public static Regex LAST_NAME_PATTERN = new Regex("^[a-zA-Z]{2,47}$");
        public static Regex PHONE_PATTERN = new Regex("^\\+?[0-9]{12}$");
    }
}

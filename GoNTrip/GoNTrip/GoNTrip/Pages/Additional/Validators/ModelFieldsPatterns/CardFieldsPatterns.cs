using System.Text.RegularExpressions;

namespace GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns
{
    public static class CardFieldsPatterns
    {
        public static readonly Regex CARD_NUMBER_PATTERN = new Regex("^(\\d{4}\\s*){4}$");
        public static readonly Regex CARD_MONTH_EXP_PATTERN = new Regex("^(0[1-9])|(1[0-2])$");
        public static readonly Regex CARD_YEAR_EXP_PATTERN = new Regex("^(19)|([2-9][0-9])$");
        public static readonly Regex CARD_CVV_PATTERN = new Regex("^[0-9]{3}$");
    }
}

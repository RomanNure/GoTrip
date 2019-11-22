using System.Text.RegularExpressions;

namespace GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns
{
    public static class GuideFieldsPatterns
    {
        public static readonly Regex CARD_NUMBER_PATTERN = new Regex("(\\d{4}\\s*){4}");
    }
}

using System.Text.RegularExpressions;

namespace GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns
{
    public static class TourPatterns
    {
        public static readonly Regex TOUR_NAME_PATTERN = new Regex("^.{1,50}$");
    }
}

using Xamarin.Forms;

using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;

namespace GoNTrip.Pages.Additional.Validators.Templates
{
    public class CustomTourValidator : FormValidator
    {
        public CustomTourValidator(Entry tourNameInput, ValidationHandler<InputView> validHandler, ValidationHandler<InputView> invalidHandler)
        {
            FieldValidationHandler<Entry> tourNameValidation = new FieldValidationHandler<Entry>(
                CN => !string.IsNullOrEmpty(CN.Text) && TourPatterns.TOUR_NAME_PATTERN.IsMatch(CN.Text),
                invalidHandler, validHandler
            );

            Constants.Callback<Entry> SubscribeEventsEntry = T =>
            {
                T.Unfocused += (sender, e) => ValidateId(GetId(sender));
                T.TextChanged += (sender, e) => ValidateId(GetId(sender));
            };

            Add<Entry>(tourNameValidation, tourNameInput, SubscribeEventsEntry);
        }
    }
}

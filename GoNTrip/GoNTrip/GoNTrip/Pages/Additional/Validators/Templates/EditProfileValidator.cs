using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;
using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Validators.Templates
{
    public class EditProfileValidator : FormValidator
    {
        public EditProfileValidator(Entry firstName, Entry lastName, Entry phone, ValidationHandler<Entry> validHandler, ValidationHandler<Entry> invalidHandler)
        {
            FieldValidationHandler<Entry> FirstNameValidation = new FieldValidationHandler<Entry>(
                FirstName => FirstName.Text != null && UserFieldsPatterns.FIRST_NAME_PATTERN.IsMatch(FirstName.Text),
                invalidHandler, validHandler);

            FieldValidationHandler<Entry> LastNameValidation = new FieldValidationHandler<Entry>(
                LastName => LastName.Text != null && UserFieldsPatterns.LAST_NAME_PATTERN.IsMatch(LastName.Text),
                invalidHandler, validHandler);

            FieldValidationHandler<Entry> PhoneValidation = new FieldValidationHandler<Entry>(
                Phone => Phone.Text != null && UserFieldsPatterns.PHONE_PATTERN.IsMatch(Phone.Text),
                invalidHandler, validHandler);

            Constants.Callback<Entry> SubscribeEvents = T =>
            {
                T.Unfocused += (sender, e) => ValidateId(GetId(sender));
                T.TextChanged += (sender, e) => ValidateId(GetId(sender));
            };

            Add<Entry>(FirstNameValidation, firstName, SubscribeEvents);
            Add<Entry>(LastNameValidation, lastName, SubscribeEvents);
            Add<Entry>(PhoneValidation, phone, SubscribeEvents);
        }
    }
}

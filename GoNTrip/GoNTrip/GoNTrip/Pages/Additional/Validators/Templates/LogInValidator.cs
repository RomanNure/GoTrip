using Xamarin.Forms;

using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;

namespace GoNTrip.Pages.Additional.Validators.Templates
{
    public class LogInValidator : FormValidator
    {
        public LogInValidator(Entry login, Entry password, ValidationHandler<Entry> validHandler, ValidationHandler<Entry> invalidHandler)
        {
            FieldValidationHandler<Entry> LoginValidation = new FieldValidationHandler<Entry>(
                Login => Login.Text != null && UserFieldsPatterns.LOGIN_PATTERN.IsMatch(Login.Text),
                invalidHandler, validHandler);

            FieldValidationHandler<Entry> PasswordValidation = new FieldValidationHandler<Entry>(
                Password => Password.Text != null && UserFieldsPatterns.PASSWORD_PATTERN.IsMatch(Password.Text),
                invalidHandler, validHandler);

            Constants.Callback<Entry> SubscribeEvents = T =>
            {
                T.Unfocused += (sender, e) => ValidateId(GetId(sender));
                T.TextChanged += (sender, e) => ValidateId(GetId(sender));
            };

            Add<Entry>(LoginValidation, login, SubscribeEvents);
            Add<Entry>(PasswordValidation, password, SubscribeEvents);
        }
    }
}

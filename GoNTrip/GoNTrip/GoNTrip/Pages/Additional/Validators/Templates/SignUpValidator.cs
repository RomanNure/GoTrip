using Xamarin.Forms;

using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;

namespace GoNTrip.Pages.Additional.Validators.Templates
{
    public class SignUpValidator : FormValidator
    {
        public SignUpValidator(Entry login, Entry password, Entry passwordConfirm, Entry email, ValidationHandler<Entry> validHandler, ValidationHandler<Entry> invalidHandler)
        {
            FieldValidationHandler<Entry> LoginValidation = new FieldValidationHandler<Entry>(
                Login => Login.Text != null && UserFieldsPatterns.LOGIN_PATTERN.IsMatch(Login.Text),
                invalidHandler, validHandler);

            FieldValidationHandler<Entry> PasswordValidation = new FieldValidationHandler<Entry>(
                Password => Password.Text != null && UserFieldsPatterns.PASSWORD_PATTERN.IsMatch(Password.Text),
                invalidHandler, validHandler);

            FieldValidationHandler<Entry> PasswordConfirmValidation = new FieldValidationHandler<Entry>(
                PasswordConfirm => PasswordConfirm.Text != null && UserFieldsPatterns.PASSWORD_PATTERN.IsMatch(PasswordConfirm.Text) && PasswordConfirm.Text == password.Text,
                invalidHandler, validHandler);

            FieldValidationHandler<Entry> EmailValidation = new FieldValidationHandler<Entry>(
                Email => Email.Text != null && UserFieldsPatterns.EMAIL_PATTERN.IsMatch(Email.Text),
                invalidHandler, validHandler);

            Constants.Callback<Entry> SubscribeEvents = T =>
            {
                T.Unfocused += (sender, e) => ValidateId(GetId(sender));
                T.TextChanged += (sender, e) => ValidateId(GetId(sender));
            };

            Add<Entry>(LoginValidation, login, SubscribeEvents);
            Add<Entry>(PasswordValidation, password, SubscribeEvents);
            Add<Entry>(PasswordConfirmValidation, passwordConfirm, SubscribeEvents);
            Add<Entry>(EmailValidation, email, SubscribeEvents);
        }
    }
}

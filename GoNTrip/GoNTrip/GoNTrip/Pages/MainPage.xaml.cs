using System;

using CustomControls;

using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Validators;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private FormValidator SignUpValidator = new FormValidator();
        private FormValidator LogInValidator = new FormValidator();

        private PopupControlSystem PopupControl = null;
        public Constants.Callback<Entry> SubscribeSignUpEvents = null;
        public Constants.Callback<Entry> SubscribeLogInEvents = null;

        public static readonly Clicked OnPopupBodyClickedIgnore = (e, sender) => true;

        public MainPage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            //Clicked OnPopupWrapperClicked = (e, sender) => { popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(); return true; };

            SignUpPopup.OnPopupBodyClicked += OnPopupBodyClickedIgnore;
            LogInPopup.OnPopupBodyClicked += OnPopupBodyClickedIgnore;

            ErrorPopup.OnFirstButtonClicked = AuthErrorClose_Clicked;

            FieldValidationHandler<Entry> LoginValidation = new FieldValidationHandler<Entry>(
                Login => Login.Text != null && UserFieldsPatterns.LOGIN_PATTERN.IsMatch(Login.Text),
                Constants.InvalidHandler, Constants.ValidHandler);

            FieldValidationHandler<Entry> PasswordValidation = new FieldValidationHandler<Entry>(
                Password => Password.Text != null && UserFieldsPatterns.PASSWORD_PATTERN.IsMatch(Password.Text),
                Constants.InvalidHandler, Constants.ValidHandler);

            FieldValidationHandler<Entry> PasswordConfirmValidation = new FieldValidationHandler<Entry>(
                PasswordConfirm => PasswordConfirm.Text != null && UserFieldsPatterns.PASSWORD_PATTERN.IsMatch(PasswordConfirm.Text) && PasswordConfirm.Text == SignUpPasswordEntry.Text,
                Constants.InvalidHandler, Constants.ValidHandler);

            FieldValidationHandler<Entry> EmailValidation = new FieldValidationHandler<Entry>(
                Email => Email.Text != null && UserFieldsPatterns.EMAIL_PATTERN.IsMatch(Email.Text),
                Constants.InvalidHandler, Constants.ValidHandler);

            SubscribeSignUpEvents = T =>
            {
                T.Unfocused += OnValidatedSignUpFieldUnfocused;
                T.TextChanged += OnValidatedSignUpFieldTextChanged;
            };

            SignUpValidator.Add<Entry>(LoginValidation, SignUpLoginEntry, SubscribeSignUpEvents);
            SignUpValidator.Add<Entry>(PasswordValidation, SignUpPasswordEntry, SubscribeSignUpEvents);
            SignUpValidator.Add<Entry>(PasswordConfirmValidation, SignUpPasswordConfirmEntry, SubscribeSignUpEvents);
            SignUpValidator.Add<Entry>(EmailValidation, SignUpEmailEntry, SubscribeSignUpEvents);

            SubscribeLogInEvents = T =>
            {
                T.Unfocused += OnValidatedLogInFieldUnfocused;
                T.TextChanged += OnValidatedLogInFieldTextChanged;
            };

            LogInValidator.Add<Entry>(LoginValidation, LogInLoginEntry, SubscribeLogInEvents);
            LogInValidator.Add<Entry>(PasswordValidation, LogInPasswordEntry, SubscribeLogInEvents);
        }

        private void OnValidatedSignUpFieldUnfocused(object sender, FocusEventArgs e) => SignUpValidator.ValidateId(SignUpValidator.GetId(sender));
        private void OnValidatedSignUpFieldTextChanged(object sender, TextChangedEventArgs e) => SignUpValidator.ValidateId(SignUpValidator.GetId(sender));

        private void OnValidatedLogInFieldUnfocused(object sender, FocusEventArgs e) => LogInValidator.ValidateId(LogInValidator.GetId(sender));
        private void OnValidatedLogInFieldTextChanged(object sender, TextChangedEventArgs e) => LogInValidator.ValidateId(LogInValidator.GetId(sender));

        private void SignUpButton_Clicked(object sender, EventArgs e) => PopupControl.OpenPopup(SignUpPopup);

        private void SignUpPopupConfirm_Clicked(object sender, EventArgs e)
        {
            if (!SignUpValidator.ValidateAll())
                return;

            SignUpAsync(SignUpLoginEntry.Text, SignUpPasswordEntry.Text, SignUpEmailEntry.Text);
        }

        private async void SignUpAsync(string login, string password, string email)
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                User user = await App.DI.Resolve<SignUpController>().SignUp(login, password, email);

                App.Current.MainPage = new CurrentUserProfilePage(user);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
            /*catch(Exception ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                AuthErrorMessage.Text = ex.InnerException.Message;
                PopupControl.OpenPopup(ErrorPopup);
            }*/
        }

        private void LogInButton_Clicked(object sender, EventArgs e) => PopupControl.OpenPopup(LogInPopup);

        private void LogInPopupConfirm_Clicked(object sender, EventArgs e)
        {
            if (!LogInValidator.ValidateAll())
                return;

            LogInAsync(LogInLoginEntry.Text, LogInPasswordEntry.Text);
        }

        private async void LogInAsync(string login, string password)
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                User user = await App.DI.Resolve<LogInController>().LogIn(login, password);
                
                App.Current.MainPage = new CurrentUserProfilePage(user);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private void AuthErrorClose_Clicked(object sender, EventArgs e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0 || PopupControl.IsKeyboardVisible())
                return false;

            PopupControl.CloseTopPopup();
            return true;
        }
    }
}
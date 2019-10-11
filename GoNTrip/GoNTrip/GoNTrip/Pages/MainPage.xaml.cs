using System;
using System.Threading.Tasks;

using CustomControls;

using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Validators;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.ServerInteraction.ResponseParsers.Auth;
using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private FormValidator SignUpValidator = new FormValidator();
        private FormValidator LogInValidator = new FormValidator();

        private PopupControlSystem PopupControl = null;

        Clicked OnPopupBodyClicked = (e, sender) => true;

        public MainPage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            //Clicked OnPopupWrapperClicked = (e, sender) => { popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(); return true; };

            SignUpPopup.OnPopupBodyClicked += OnPopupBodyClicked;
            LogInPopup.OnPopupBodyClicked += OnPopupBodyClicked;
            ActivityPopup.OnPopupBodyClicked += OnPopupBodyClicked;
            ErrorPopup.OnPopupBodyClicked += OnPopupBodyClicked;

            ValidationHandler<InputView> InvalidHandler = Input => Input.BackgroundColor = (Color)Application.Current.Resources["InvalidColor"];
            ValidationHandler<InputView> ValidHandler = Input => Input.BackgroundColor = (Color)Application.Current.Resources["ContentBackColor"];

            FieldValidationHandler<Entry> LoginValidation = new FieldValidationHandler<Entry>(
                Login => Login.Text != null && UserFieldsPatterns.LOGIN_PATTERN.IsMatch(Login.Text),
                InvalidHandler, ValidHandler);

            FieldValidationHandler<Entry> PasswordValidation = new FieldValidationHandler<Entry>(
                Password => Password.Text != null && UserFieldsPatterns.PASSWORD_PATTERN.IsMatch(Password.Text),
                InvalidHandler, ValidHandler);

            FieldValidationHandler<Entry> PasswordConfirmValidation = new FieldValidationHandler<Entry>(
                PasswordConfirm => PasswordConfirm.Text != null && UserFieldsPatterns.PASSWORD_PATTERN.IsMatch(PasswordConfirm.Text) && PasswordConfirm.Text == SignUpPasswordEntry.Text,
                InvalidHandler, ValidHandler);

            FieldValidationHandler<Entry> EmailValidation = new FieldValidationHandler<Entry>(
                Email => Email.Text != null && UserFieldsPatterns.EMAIL_PATTERN.IsMatch(Email.Text),
                InvalidHandler, ValidHandler);

            SignUpValidator.Add<Entry>(LoginValidation, SignUpLoginEntry);
            SignUpValidator.Add<Entry>(PasswordValidation, SignUpPasswordEntry);
            SignUpValidator.Add<Entry>(PasswordConfirmValidation, SignUpPasswordConfirmEntry);
            SignUpValidator.Add<Entry>(EmailValidation, SignUpEmailEntry);

            SignUpLoginEntry.Unfocused += OnValidatedSignUpFieldUnfocused;
            SignUpPasswordEntry.Unfocused += OnValidatedSignUpFieldUnfocused;
            SignUpPasswordConfirmEntry.Unfocused += OnValidatedSignUpFieldUnfocused;
            SignUpEmailEntry.Unfocused += OnValidatedSignUpFieldUnfocused;

            LogInValidator.Add<Entry>(LoginValidation, LogInLoginEntry);
            LogInValidator.Add<Entry>(PasswordValidation, LogInPasswordEntry);

            LogInLoginEntry.Unfocused += OnValidatedLogInFieldUnfocused;
            LogInPasswordEntry.Unfocused += OnValidatedLogInFieldUnfocused;
        }

        private void OnValidatedSignUpFieldUnfocused(object sender, FocusEventArgs e) => SignUpValidator.ValidateId(SignUpValidator.GetId(sender));
        private void OnValidatedLogInFieldUnfocused(object sender, FocusEventArgs e) => LogInValidator.ValidateId(LogInValidator.GetId(sender));

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
                User user = null;
                await Task.Run(() =>
                {
                    IServerCommunicator server = App.DI.ResolveOptional<IServerCommunicator>();
                    AuthQueryFactory authQueryFactory = App.DI.Resolve<AuthQueryFactory>();
                    IQuery signUpQuery = authQueryFactory.SignUp(login, password, email);
                    IResponseParser signUpParser = App.DI.Resolve<SignUpResponseParser>();
                    IServerResponse response = server.SendQuery(signUpQuery);
                    user = signUpParser.Parse(response) as User;
                });

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                App.Current.MainPage = new ProfilePage(user);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                AuthErrorMessage.Text = ex.message;
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
                User user = null;
                await Task.Run(() =>
                {
                    IServerCommunicator server = App.DI.Resolve<IServerCommunicator>();
                    AuthQueryFactory authQueryFactory = App.DI.Resolve<AuthQueryFactory>();
                    IQuery logInQuery = authQueryFactory.LogIn(login, password);
                    IResponseParser logInParser = App.DI.Resolve<LogInResponseParser>();
                    IServerResponse response = server.SendQuery(logInQuery);
                    user = logInParser.Parse(response) as User;
                });

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                App.Current.MainPage = new ProfilePage(user);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                AuthErrorMessage.Text = ex.message;
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
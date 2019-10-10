using System;
using System.Threading.Tasks;

using CustomControls;

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
        private FormValidator Validator = new FormValidator();

        private PopupControlSystem popupControlSystem = null;

        private Popup SignUpPopupModel = null;
        private Popup LogInPopupModel = null;
        private Popup ErrorPopupModel = null;
        private Popup ActivityPopupModel = null;

        public MainPage()
        {
            InitializeComponent();

            popupControlSystem = new PopupControlSystem(OnBackButtonPressed);

            Clicked OnPopupWrapperClicked = null; //(e, sender) => { popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(); return true; };
            Clicked OnPopupBodyClicked = (e, sender) => true;

            SignUpPopupModel = new Popup(SignUpPopup, OnPopupWrapperClicked, SignUpPopupOuterLayout, OnPopupBodyClicked, true, null, null,
                                             SignUpLoginEntry, SignUpPasswordEntry, SignUpPasswordConfirmEntry, SignUpEmailEntry);

            LogInPopupModel = new Popup(LogInPopup, OnPopupWrapperClicked, LogInPopupOuterLayout, OnPopupBodyClicked, true, null, null,
                                        LogInLoginEntry, LogInPasswordEntry);

            ErrorPopupModel = new Popup(ErrorPopup, OnPopupBodyClicked, ErrorPopupOuterLayout, OnPopupBodyClicked, true);
            ActivityPopupModel = new Popup(ActivityPopup, OnPopupBodyClicked, ActivityPopupOuterLayout, OnPopupBodyClicked, false);

            ValidationHandler<InputView> InvalidHandler = Input => Input.BackgroundColor = Color.IndianRed;
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

            Validator.Add<Entry>(LoginValidation, SignUpLoginEntry);
            Validator.Add<Entry>(PasswordValidation, SignUpPasswordEntry);
            Validator.Add<Entry>(PasswordConfirmValidation, SignUpPasswordConfirmEntry);
            Validator.Add<Entry>(EmailValidation, SignUpEmailEntry);

            SignUpLoginEntry.Unfocused += OnValidatedFieldUnfocused;
            SignUpPasswordEntry.Unfocused += OnValidatedFieldUnfocused;
            SignUpPasswordConfirmEntry.Unfocused += OnValidatedFieldUnfocused;
            SignUpEmailEntry.Unfocused += OnValidatedFieldUnfocused;
        }

        private void OnValidatedFieldUnfocused(object sender, FocusEventArgs e) => Validator.ValidateId(Validator.GetId(sender));

        private void SignUpButton_Clicked(object sender, EventArgs e) => popupControlSystem.OpenPopup(SignUpPopupModel);

        private void SignUpPopupConfirm_Clicked(object sender, EventArgs e)
        {
            if (!Validator.ValidateAll())
                return;

            SignUpAsync(SignUpLoginEntry.Text, SignUpPasswordEntry.Text, SignUpEmailEntry.Text);
        }

        private async void SignUpAsync(string login, string password, string email)
        {
            popupControlSystem.OpenPopup(ActivityPopupModel);

            try
            {
                await Task.Run(() =>
                {
                    IServerCommunicator server = new ServerCommunicator(); //Should use DI
                    AuthQueryFactory authQueryFactory = new AuthQueryFactory();
                    IQuery signUpQuery = authQueryFactory.SignUp(login, password, email);
                    IResponseParser signUpParser = new SignUpResponseParser();
                    IServerResponse response = server.SendQuery(signUpQuery);
                    ModelElement user = signUpParser.Parse(response);
                });

                popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(true);

                App.Current.MainPage = new Page(); // Profile Page
            }
            catch (ResponseException ex)
            {
                popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(true);

                AuthErrorMessage.Text = ex.message;
                popupControlSystem.OpenPopup(ErrorPopupModel);
            }
        }

        private void LogInButton_Clicked(object sender, EventArgs e) => popupControlSystem.OpenPopup(LogInPopupModel);

        private void LogInPopupConfirm_Clicked(object sender, EventArgs e)
        {
            //Validate
        }

        private async void LogInAsync()
        {

        }

        private void AuthErrorClose_Clicked(object sender, EventArgs e) => popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded();

        protected override bool OnBackButtonPressed()
        {
            if (popupControlSystem.OpenedPopupsCount == 0 || popupControlSystem.IsKeyboardVisible())
                return false;

            popupControlSystem.CloseTopPopup();
            return true;
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Pages.Help;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.ServerInteraction.ResponseParsers.Auth;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private PopupControlSystem popupControlSystem = null;

        private Popup SignUpPopupModel = null;
        private Popup LogInPopupModel = null;
        private Popup ErrorPopupModel = null;
        private Popup ActivityPopupModel = null;

        private Clicked OnPopupWrapperClicked = null;
        private Clicked OnPopupBodyClicked = (e, sender) => true;

        public MainPage()
        {
            InitializeComponent();

            popupControlSystem = new PopupControlSystem(OnBackButtonPressed);

            //OnPopupWrapperClicked = (e, sender) => { popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(); return true; };

            SignUpPopupModel = new Popup(SignUpPopup, OnPopupWrapperClicked, SignUpPopupOuterLayout, OnPopupBodyClicked, true, null, null,
                                         SignUpLoginEntry, SignUpPasswordEntry, SignUpPasswordConfirmEntry, SignUpEmailEntry);

            LogInPopupModel = new Popup(LogInPopup, OnPopupWrapperClicked, LogInPopupOuterLayout, OnPopupBodyClicked, true, null, null,
                                        LogInLoginEntry, LogInPasswordEntry);

            ErrorPopupModel = new Popup(ErrorPopup, OnPopupBodyClicked, ErrorPopupOuterLayout, OnPopupBodyClicked, true);
            ActivityPopupModel = new Popup(ActivityPopup, OnPopupBodyClicked, ActivityPopupOuterLayout, OnPopupBodyClicked, false);
        }

        private void AuthErrorClose_Clicked(object sender, EventArgs e) => popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded();

        private void SignUpButton_Clicked(object sender, EventArgs e) => popupControlSystem.OpenPopup(SignUpPopupModel);
        private void LogInButton_Clicked(object sender, EventArgs e) => popupControlSystem.OpenPopup(LogInPopupModel);

        private void SignUpPopupConfirm_Clicked(object sender, EventArgs e)
        {
            //Validate
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

                AuthErrorMessage.Text = "Registration Successful";
                popupControlSystem.OpenPopup(ErrorPopupModel);
            }
            catch (ResponseException ex)
            {
                popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(true);

                AuthErrorMessage.Text = ex.message;
                popupControlSystem.OpenPopup(ErrorPopupModel);
            }
        }

        private void LogInPopupConfirm_Clicked(object sender, EventArgs e)
        {
            //Validate
        }

        protected override bool OnBackButtonPressed()
        {
            if (popupControlSystem.OpenedPopupsCount == 0 || popupControlSystem.IsKeyboardVisible())
                return false;

            popupControlSystem.CloseTopPopup();
            return true;
        }
    }
}
using System;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Android.Views;

using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.ServerInteraction.ResponseParsers.Auth;
using GoNTrip.Model;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private ClickableContentView CurrentPopup = null;

        public MainPage()
        {
            InitializeComponent();
        }

        private void SignUpPopupConfirm_Clicked(object sender, EventArgs e)
        {
            IServerCommunicator server = new ServerCommunicator();
            AuthQueryFactory authQueryFactory = new AuthQueryFactory();
            IQuery signUpQuery = authQueryFactory.SignUp(SignUpLoginEntry.Text, SignUpPasswordEntry.Text, SignUpEmailEntry.Text);
            IResponseParser signUpParser = new SignUpResponseParser();
            IServerResponse response = server.SendQuery(signUpQuery);
            ModelElement user = signUpParser.Parse(response);
        }

        private void LogInPopupConfirm_Clicked(object sender, EventArgs e)
        {

        }

        ////SIGN UP UI
        private void SignUpPopupHide()
        {
            if (ClosePopupBackPressNeeded(SignUpPopup, SignUpLoginEntry, SignUpPasswordEntry, SignUpPasswordConfirmEntry, SignUpEmailEntry))
                OnBackButtonPressed();
            CurrentPopup = null;
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            SignUpPopup.IsVisible = true;
            CurrentPopup = SignUpPopup;
        }

        private bool SignUpPopup_OnClick(MotionEvent ME, IClickable sender)
        {
            SignUpPopupHide();
            return false;
        }

        private bool SignUpPopupOuterLayout_OnClick(MotionEvent ME, IClickable sender) => true;
        ////SIGN UP UI

        ////LOG IN UI
        private void LogInPopupHide()
        {
            if (ClosePopupBackPressNeeded(LogInPopup, LogInLoginEntry, LogInPasswordEntry))
                OnBackButtonPressed();
            CurrentPopup = null;
        }

        private void LogInButton_Clicked(object sender, EventArgs e)
        {
            LogInPopup.IsVisible = true;
            CurrentPopup = LogInPopup;
        }

        private bool LogInPopup_OnClick(MotionEvent ME, IClickable sender)
        {
            LogInPopupHide();
            return false;
        }

        private bool LogInPopupOuterLayout_OnClick(MotionEvent ME, IClickable sender) => true;
        ////LOG IN UI

        protected override bool OnBackButtonPressed()
        {
            if (CurrentPopup != null)
            {
                CurrentPopup.IsVisible = false;
                CurrentPopup = null;
            }

            return true;
        }

        private bool ClosePopupBackPressNeeded(ClickableContentView CCV, params Xamarin.Forms.View[] entries)
        {
            bool result = false;

            foreach (Xamarin.Forms.View entry in entries)
                if (entry.IsFocused)
                {
                    entry.Unfocus();
                    result = true;
                    break;
                }

            CCV.IsVisible = false;
            return result;
        }
    }
}
using System;

using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.Validators.Templates;
using System.Collections.Generic;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }
        private SignUpValidator SignUpValidator { get; set; }
        private LogInValidator LogInValidator { get; set; }

        public MainPage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            SignUpPopup.OnPopupBodyClicked += Constants.CLICK_IGNORE;
            LogInPopup.OnPopupBodyClicked += Constants.CLICK_IGNORE;

            ErrorPopup.OnFirstButtonClicked = AuthErrorClose_Clicked;

            SignUpValidator = new SignUpValidator(SignUpLoginEntry, SignUpPasswordEntry, SignUpPasswordConfirmEntry, SignUpEmailEntry, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);
            LogInValidator = new LogInValidator(LogInLoginEntry, LogInPasswordEntry, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);
        }

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
                App.DI.Resolve<Session>().CurrentUser = await App.DI.Resolve<SignUpController>().SignUp(login, password, email);
                App.Current.MainPage = new CurrentUserProfilePage();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
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
                App.DI.Resolve<Session>().CurrentUser = await App.DI.Resolve<LogInController>().LogIn(login, password);
                App.Current.MainPage = new CurrentUserProfilePage();

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
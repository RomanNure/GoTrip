using System;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Android.Views;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private ClickableContentView CurrentPopup = null;

        public SignUpPage()
        {
            InitializeComponent();
        }

        private void SignUpPopupConfirm_Clicked(object sender, EventArgs e)
        {

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

        private bool SignUpPopup_OnClick(MotionEvent ME, ClickableContentView sender)
        {
            SignUpPopupHide();
            return false;
        }

        private bool SignUpPopupOuterLayout_OnClick(MotionEvent ME, ClickableStackLayout sender) => true;
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

        private bool LogInPopup_OnClick(MotionEvent ME, ClickableContentView sender)
        {
            LogInPopupHide();
            return false;
        }

        private bool LogInPopupOuterLayout_OnClick(MotionEvent ME, ClickableStackLayout sender) => true;
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
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Android.Views;

using CustomControls;

using GoNTrip.Pages.Additional.Popups;
using Autofac;
using GoNTrip.Util;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvancedPage : ContentPage
    {
        public AdvancedPage()
        {
            InitializeComponent();
        }

        private PopupControlSystem PopupControl { get; set; }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            Navigator.Current = Additional.Controls.DefaultNavigationPanel.PageEnum.ADVANCED;
            Navigator.LinkClicks(PopupControl, ActivityPopup);
        }

        private bool QrScannerOpenButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action != MotionEventActions.Down)
                return false;

            Scan();
            return false;
        }

        private async void Scan()
        {
            PopupControl.OpenPopup(ActivityPopup);
            string scannedData = await App.DI.Resolve<QrService>().ScanAsync();
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

            ErrorPopup.MessageText = scannedData;
            PopupControl.OpenPopup(ErrorPopup);
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
                PopupControl.OpenPopup(ExitConfirmPopup);
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Android.Views;

using CustomControls;

using Autofac;

using GoNTrip.Util;
using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Controls;
using GoNTrip.Model.FilterSortSearch.Tour;
using GoNTrip.Pages.Additional.PageMementos;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvancedPage : ContentPage
    {
        private PageMemento CurrentPageMemento { get; set; }

        [RestorableConstructor]
        public AdvancedPage()
        {
            InitializeComponent();

            CurrentPageMemento = new PageMemento();
            CurrentPageMemento.Save(this);
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

            CheckTicketsButton.IsEnabled = App.DI.Resolve<Session>().CurrentUser.guide != null;
            BecomeGuideButton.IsEnabled = App.DI.Resolve<Session>().CurrentUser.guide == null;
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

        private void BecomeGuideButton_Clicked(object sender, EventArgs e) =>
            App.Current.MainPage = new GuideRegistrationPage(CurrentPageMemento);

        private void CheckTicketsButton_Clicked(object sender, EventArgs e)
        {
            TourFilterSorterSearcher tourFilterSorterSearcher = new TourFilterSorterSearcher();
            tourFilterSorterSearcher.semiFilters.tourGuideId = App.DI.Resolve<Session>().CurrentUser.guide.id;

            App.Current.MainPage = new TourListPage(Constants.CHECK_TICKETS_TOUR_LIST_PAGE_CAPTION, DefaultNavigationPanel.PageEnum.OTHER, 
                                                    CurrentPageMemento, tourFilterSorterSearcher);
        }
    }
}
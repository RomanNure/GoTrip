using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Android.Views;

using CustomControls;

using Autofac;

using GoNTrip.Util;
using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Controls;
using GoNTrip.Model.FilterSortSearch.Tour;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.Validators.Templates;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvancedPage : ContentPage
    {
        private CustomTourValidator Validator { get; set; }
        private PopupControlSystem PopupControl { get; set; }
        private PageMemento CurrentPageMemento { get; set; }

        [RestorableConstructor]
        public AdvancedPage()
        {
            InitializeComponent();

            CurrentPageMemento = new PageMemento();
            CurrentPageMemento.Save(this);
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            Navigator.Current = DefaultNavigationPanel.PageEnum.ADVANCED;
            Navigator.LinkClicks(PopupControl, ActivityPopup);

            CheckTicketsButton.IsEnabled = App.DI.Resolve<Session>().CurrentUser.guide != null;
            BecomeGuideButton.IsEnabled = App.DI.Resolve<Session>().CurrentUser.guide == null;

            Validator = new CustomTourValidator(CustomTourName, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);
        }

        private void BecomeGuideButton_Clicked(object sender, EventArgs e) =>
            App.Current.MainPage = new GuideRegistrationPage(CurrentPageMemento);

        private void CheckTicketsButton_Clicked(object sender, EventArgs e)
        {
            TourFilterSorterSearcher tourFilterSorterSearcher = new TourFilterSorterSearcher();
            tourFilterSorterSearcher.semiFilters.tourGuideId = App.DI.Resolve<Session>().CurrentUser.guide.Id;
            tourFilterSorterSearcher.semiFilters.noCustomTours = false;

            App.Current.MainPage = new TourListPage(Constants.CHECK_TICKETS_TOUR_LIST_PAGE_CAPTION, DefaultNavigationPanel.PageEnum.OTHER, 
                                                    CurrentPageMemento, tourFilterSorterSearcher);
        }

        private void AddCustomTourButton_Clicked(object sender, EventArgs e) =>
            PopupControl.OpenPopup(CreateCustomTourPopup);

        private async void CreateCustomTourConfirmButton_Clicked(object sender, EventArgs e)
        {
            if (!Validator.ValidateAll())
                return;

            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                await App.DI.Resolve<TourController>().CreateCustomTour(CustomTourName.Text, CustomTourDescription.Text,
                    CustomTourLocation.Text, CustomStartDate.Date, CustomFinishDate.Date);

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
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
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using CustomControls;

using Android.Views;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Controls;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourListPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }

        public TourListPage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            Navigator.Current = DefaultNavigationPanel.PageEnum.TOUR_LIST;
            Navigator.LinkClicks();

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
        }

        private bool UpdateButton_OnClick(MotionEvent ME, IClickable sender) { GetAndLoadTours(); return false; }
        private void ContentPage_Appearing(object sender, EventArgs e) => GetAndLoadTours();

        private async void GetAndLoadTours()
        {
            List<Tour> tours = await GetTours();
            LoadToursAsync(tours);
        }

        private async Task<List<Tour>> GetTours()
        {
            PopupControl.OpenPopup(ActivityPopup);
            List<Tour> rawTourList = new List<Tour>();
            
            try
            {
                rawTourList = await App.DI.Resolve<GetToursController>().GetAllTours();
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }

            Tour TOO_LONG_TOUR = new Tour();
            TOO_LONG_TOUR.name = "TOO LONG TOUR NAME" + "TOO LONG TOUR NAME" + "TOO LONG TOUR NAME" + "TOO LONG TOUR NAME" + "TOO LONG TOUR NAME";
            TOO_LONG_TOUR.description = "TOO LONG TOUR DESCRIPTION" + "TOO LONG TOUR DESCRIPTION" + "TOO LONG TOUR DESCRIPTION" + "TOO LONG TOUR DESCRIPTION" + "TOO LONG TOUR DESCRIPTION";
            rawTourList.Add(TOO_LONG_TOUR);

            return rawTourList;
        }

        private void LoadToursAsync(List<Tour> tours)
        {
            PopupControl.OpenPopup(ActivityPopup);

            TourList.Children.Clear();

            foreach (Tour tour in tours)
                TourList.Children.Add(new TourListItem(tour));

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private bool SearchButton_OnClick(MotionEvent ME, IClickable sender)
        {
            return false;
        }

        private bool FilterButton_OnClick(MotionEvent ME, IClickable sender)
        {
            return false;
        }

        private bool SortButton_OnClick(MotionEvent ME, IClickable sender)
        {
            return false;
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
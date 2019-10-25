using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

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

            Navigator.Current = Additional.Controls.DefaultNavigationPanel.PageEnum.TOUR_LIST;
            Navigator.LinkClicks();

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            List<Tour> allTours = await GetTours();
            LoadToursAsync(allTours);
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

            return rawTourList;
        }

        private void LoadToursAsync(List<Tour> tours)
        {
            PopupControl.OpenPopup(ActivityPopup);

            foreach (Tour tour in tours)
                TourList.Children.Add(new TourListItem(tour));

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private bool SearchButton_OnClick(Android.Views.MotionEvent ME, CustomControls.IClickable sender)
        {
            return false;
        }

        private bool FilterButton_OnClick(Android.Views.MotionEvent ME, CustomControls.IClickable sender)
        {
            return false;
        }

        private bool SortButton_OnClick(Android.Views.MotionEvent ME, CustomControls.IClickable sender)
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
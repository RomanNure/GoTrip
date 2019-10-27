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
using System.Threading;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourListPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }

        private List<TourListItem> TourLayouts = new List<TourListItem>();
        private List<Tour> Tours = new List<Tour>();
        private int FirstTourNum { get; set; }

        private const int PAGE_TOURS_COUNT = 8;

        public TourListPage(int firstTourNum = 0)
        {
            FirstTourNum = firstTourNum;
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            Navigator.Current = DefaultNavigationPanel.PageEnum.TOUR_LIST;
            Navigator.LinkClicks();

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);

            for (int i = 0; i < PAGE_TOURS_COUNT; i++)
            {
                TourListItem tourLayout = await App.DI.Resolve<TourListItemFactory>().CreateTourListItem();
                tourLayout.IsVisible = false;
                TourLayouts.Add(tourLayout);
                TourList.Children.Add(tourLayout);
            }

            Grid buttonsWrapper = new Grid();
            buttonsWrapper.Style = (Style)App.Current.Resources["TourListNavigationButtonsWrapper"];

            Img prevPageButton = new Img() { Source = "prev.png", BorderRadius = 5, ClickedBorderWidth = 0, BorderAlways = false,
                                             ScaleOnClicked = 0.9f, HorizontalOptions = LayoutOptions.End, IsVisible = false };

            Img nextPageButton = new Img() { Source = "next.png", BorderRadius = 5, ClickedBorderWidth = 0, BorderAlways = false,
                                             ScaleOnClicked = 0.9f, HorizontalOptions = LayoutOptions.Start, IsVisible = false };

            prevPageButton.Style = (Style)App.Current.Resources["TourListNavigationButton"];
            nextPageButton.Style = (Style)App.Current.Resources["TourListNavigationButton"];

            prevPageButton.OnClick += (ME, ctx) =>
            {
                if (ME.Action == MotionEventActions.Down)
                    LoadToursActivity(FirstTourNum - PAGE_TOURS_COUNT);
                return false;
            };

            nextPageButton.OnClick += (ME, ctx) =>
            {
                if (ME.Action == MotionEventActions.Down)
                    LoadToursActivity(FirstTourNum + PAGE_TOURS_COUNT);
                return false;
            };

            buttonsWrapper.Children.Add(prevPageButton, 0, 0);
            buttonsWrapper.Children.Add(nextPageButton, 1, 0);
            TourList.Children.Add(buttonsWrapper);

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

            await GetAndLoadTours();

            prevPageButton.IsVisible = true;
            nextPageButton.IsVisible = true;
        }

        private async void LoadToursActivity(int firstTourNum) => await LoadToursActivityAsync(firstTourNum);

        private async Task LoadToursActivityAsync(int firstTourNum)
        {
            int NewTourNum = -1;

            if (firstTourNum >= 0 && firstTourNum < Tours.Count)
                NewTourNum = firstTourNum;

            if (NewTourNum != -1)
            {
                FirstTourNum = NewTourNum;
                await GetAndLoadTours();
                await TourListScroll.ScrollToAsync(0, 0, false);
            }
        }

        private async Task GetAndLoadTours()
        {
            if (Tours.Count == 0)
                Tours = await GetTours();
            else
                await Task.Run(() => Thread.Sleep(100));

            if (Tours.Count != 0)
            {
                PopupControl.OpenPopup(ActivityPopup);
                LoadTours(Tours.Skip(FirstTourNum).Take(PAGE_TOURS_COUNT).ToList());
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
        }

        private async Task<List<Tour>> GetTours()
        {
            PopupControl.OpenPopup(ActivityPopup);
            List<Tour> tours = new List<Tour>();

            try
            {
                tours = await App.DI.Resolve<GetToursController>().GetAllTours();
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }

            return tours;
        }

        private void LoadTours(List<Tour> tours)
        {
            for (int i = 0; i < tours.Count && i < PAGE_TOURS_COUNT; i++)
            {
                TourLayouts[i].Fill(tours[i], i);
                TourLayouts[i].IsVisible = true;
            }

            for (int i = tours.Count; i < PAGE_TOURS_COUNT; i++)
                TourLayouts[i].IsVisible = false;
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
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using CustomControls;

using Android.Views;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Model.FilterSortSearch;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Controls;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.Model.FilterSortSearch.Filters;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourListPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }
        private List<RadioButton> SortedCheckers { get; set; }

        private FilterSorterSearcher CurrentFilterSorterSearcher { get; set; }
        private List<TourListItem> TourLayouts = new List<TourListItem>();
        private List<Tour> Tours = new List<Tour>();
        private int FirstTourNum { get; set; }

        private const int PAGE_TOURS_COUNT = 8;

        public TourListPage() : this(null) { }

        public TourListPage(TourListPageMemento memento = null)
        {
            if (memento != null)
            {
                Tours = memento.TourListPage.Tours;
                FirstTourNum = memento.TourListPage.FirstTourNum;
                CurrentFilterSorterSearcher = memento.TourListPage.CurrentFilterSorterSearcher;
            }

            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            Navigator.Current = DefaultNavigationPanel.PageEnum.TOUR_LIST;
            Navigator.LinkClicks(PopupControl, ActivityPopup);

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            SortedCheckers = new List<RadioButton>() { PriceSortedChecker, FreePlacesSortedChecker };
            CurrentFilterSorterSearcher = new FilterSorterSearcher();

            LoadFilterNumericUpDowns();
        }

        private void LoadFilterNumericUpDowns()
        {
            MinPricePicker.OnValueChanged += (sender) => MaxPricePicker.Val = Math.Max(MinPricePicker.Val, MaxPricePicker.Val);
            MaxPricePicker.OnValueChanged += (sender) => MinPricePicker.Val = Math.Min(MinPricePicker.Val, MaxPricePicker.Val);

            MinStartDate.DateSelected += (sender, e) => MaxStartDate.Date = MinStartDate.Date > MaxStartDate.Date ? MinStartDate.Date : MaxStartDate.Date;
            MaxStartDate.DateSelected += (sender, e) => MinStartDate.Date = MinStartDate.Date > MaxStartDate.Date ? MaxStartDate.Date : MinStartDate.Date;

            MinPlacesPicker.OnValueChanged += (sender) => MaxPlacesPicker.Val = Math.Max(MinPlacesPicker.Val, MaxPlacesPicker.Val);
            MaxPlacesPicker.OnValueChanged += (sender) => MinPlacesPicker.Val = Math.Min(MinPlacesPicker.Val, MaxPlacesPicker.Val);
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

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

            await GetAndLoadToursAsync(CurrentFilterSorterSearcher);
            UpdateFiltersBounds();

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
                await GetAndLoadToursAsync(CurrentFilterSorterSearcher);
                await TourListScroll.ScrollToAsync(0, 0, false);
            }
        }

        private async void GetAndLoadTours(FilterSorterSearcher filterSorterSearcher) => await GetAndLoadToursAsync(filterSorterSearcher);
        private async Task GetAndLoadToursAsync(FilterSorterSearcher filterSorterSearcher)
        {
            if (Tours.Count == 0)
                Tours = await GetTours(filterSorterSearcher);

            if (Tours.Count != 0)
            {
                PopupControl.OpenPopup(ActivityPopup);

                await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));
                LoadTours(Tours.Skip(FirstTourNum).Take(PAGE_TOURS_COUNT).ToList());

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
        }

        private async Task<List<Tour>> GetTours(FilterSorterSearcher filterSorterSearcher = null)
        {
            PopupControl.OpenPopup(ActivityPopup);
            List<Tour> tours = new List<Tour>();

            try
            {
                tours = await App.DI.Resolve<GetToursController>().GetTours(filterSorterSearcher);
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

                Tour tour = tours[i];
                Clicked tourClicked = (ME, ctx) => {
                    if (ME.Action == MotionEventActions.Up)
                    {
                        PopupControl.OpenPopup(ActivityPopup);
                        App.Current.MainPage = new TourPage(tour, new TourListPageMemento(this));
                    }
                    return false;
                };

                TourLayouts[i].OnClick += tourClicked;
                TourLayouts[i].image.OnClick += tourClicked;
            }

            for (int i = tours.Count; i < PAGE_TOURS_COUNT; i++)
                TourLayouts[i].IsVisible = false;
        }

        private void UpdateFiltersBounds()
        {
            MinPricePicker.Val = Tours.Count == 0 ? 0 : Tours.Min(T => T.pricePerPerson);
            MaxPricePicker.Val = Tours.Count == 0 ? 0 : Tours.Max(T => T.pricePerPerson);

            MinStartDate.Date = Tours.Count == 0 ? MinStartDate.Date : Tours.Min(T => T.startDateTime);
            MaxStartDate.Date = Tours.Count == 0 ? MaxStartDate.Date : Tours.Max(T => T.startDateTime);

            MinPlacesPicker.Val = Tours.Count == 0 ? 0 : Tours.Min(T => T.maxParticipants);
            MaxPlacesPicker.Val = Tours.Count == 0 ? 0 : Tours.Max(T => T.maxParticipants);
        }

        private bool SearchButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                PopupControl.OpenPopup(SearchPopup);

            return false;
        }

        private bool FilterButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                PopupControl.OpenPopup(FilterPopup);

            return false;
        }

        private bool SortButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                PopupControl.OpenPopup(SortPopup);

            return false;
        }

        private void SearchPopupConfirm_Clicked(object sender, EventArgs e)
        {
            CurrentFilterSorterSearcher.tourSubstring = TourNameSearcher.Text;
            CurrentFilterSorterSearcher.locationSubstring = TourLocationSearcher.Text;

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            UpdateTours();
        }

        private void FilterPopupConfirm_Clicked(object sender, EventArgs e)
        {
            CurrentFilterSorterSearcher.filters.Clear();

            CurrentFilterSorterSearcher.filters.Add(new PriceFilter(MinPricePicker.Val, MaxPricePicker.Val));
            CurrentFilterSorterSearcher.filters.Add(new DateStartFilter(MinStartDate.Date, MaxStartDate.Date));
            CurrentFilterSorterSearcher.filters.Add(new PlacesFilter((int)MinPlacesPicker.Val, (int)MaxPlacesPicker.Val));

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            UpdateTours();
        }

        private void SortPopupConfirm_Clicked(object sender, EventArgs e)
        {
            if (PriceSortedChecker.Checked)
                CurrentFilterSorterSearcher.SortingCriteria = Sorter.price;
            else if (FreePlacesSortedChecker.Checked)
                CurrentFilterSorterSearcher.SortingCriteria = Sorter.free_places;
            else
                CurrentFilterSorterSearcher.SortingCriteria = Sorter.no;

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            UpdateTours();
        }

        private bool SortedChecker_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action != MotionEventActions.Down)
                return false;

            foreach (RadioButton sortedChecker in SortedCheckers)
                if (!sortedChecker.Equals(sender))
                    sortedChecker.Checked = false;

            return false;
        }

        private bool UpdateButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                UpdateTours();

            return false;
        }

        private void UpdateTours()
        {
            Tours.Clear();
            FirstTourNum = 0;
            GetAndLoadTours(CurrentFilterSorterSearcher);
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
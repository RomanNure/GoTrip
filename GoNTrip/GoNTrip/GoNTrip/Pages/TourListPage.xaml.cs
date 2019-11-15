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
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Controls;
using GoNTrip.Model.FilterSortSearch.Tour;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourListPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }
        private List<RadioButton> SortedCheckers { get; set; }

        private TourFilterSorterSearcher CurrentTourFilterSorterSearcher { get; set; }
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
                CurrentTourFilterSorterSearcher = memento.TourListPage.CurrentTourFilterSorterSearcher;
            }
            else
                CurrentTourFilterSorterSearcher = new TourFilterSorterSearcher();

            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            Navigator.Current = DefaultNavigationPanel.PageEnum.TOUR_LIST;
            Navigator.LinkClicks(PopupControl, ActivityPopup);

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            SortedCheckers = new List<RadioButton>() { PriceSortedChecker, FreePlacesSortedChecker };

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

            Img prevPageButton = new Img() { Source = "prev.png", CornerRad = 5, Border = false, BorderOnClick = false, ScaleOnClicked = 0.9f, HorizontalOptions = LayoutOptions.End, IsVisible = false };

            Img nextPageButton = new Img() { Source = "next.png", CornerRad = 5, Border = false, BorderOnClick = false, ScaleOnClicked = 0.9f, HorizontalOptions = LayoutOptions.Start, IsVisible = false };

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

            await GetAndLoadToursAsync(CurrentTourFilterSorterSearcher);

            if (!CurrentTourFilterSorterSearcher.filters.IsChanged)
                ResetFilterView();
            else
                LoadFilterSorterSearcher(CurrentTourFilterSorterSearcher);

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
                await GetAndLoadToursAsync(CurrentTourFilterSorterSearcher);
                await TourListScroll.ScrollToAsync(0, 0, false);
            }
        }

        private async void GetAndLoadTours(TourFilterSorterSearcher filterSorterSearcher) => await GetAndLoadToursAsync(filterSorterSearcher);
        private async Task GetAndLoadToursAsync(TourFilterSorterSearcher filterSorterSearcher)
        {
            if (Tours.Count == 0)
                Tours = await GetTours(filterSorterSearcher);

            PopupControl.OpenPopup(ActivityPopup);

            await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));

            LoadTours(Tours.Skip(FirstTourNum).Take(PAGE_TOURS_COUNT).ToList());

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private async Task<List<Tour>> GetTours(TourFilterSorterSearcher filterSorterSearcher = null)
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

        private void ResetSearchView()
        {
            TourNameSearcher.Text = "";
            TourLocationSearcher.Text = "";
        }

        private void ResetFilterView()
        {
            MaxPricePicker.Val = Tours.Count == 0 ? 0 : Tours.Max(T => T.pricePerPerson);
            MinPricePicker.Val = Tours.Count == 0 ? 0 : Tours.Min(T => T.pricePerPerson);

            MaxStartDate.Date = Tours.Count == 0 ? MaxStartDate.Date : Tours.Max(T => T.startDateTime);
            MinStartDate.Date = Tours.Count == 0 ? MinStartDate.Date : Tours.Min(T => T.startDateTime);

            MaxPlacesPicker.Val = Tours.Count == 0 ? 0 : Tours.Max(T => T.maxParticipants);
            MinPlacesPicker.Val = Tours.Count == 0 ? 0 : Tours.Min(T => T.maxParticipants);
        }

        private void ResetSortView()
        {
            PriceSortedChecker.Checked = false;
            FreePlacesSortedChecker.Checked = false;
        }

        private void LoadFilterSorterSearcher(TourFilterSorterSearcher filterSorterSearcher)
        {
            LoadSearcher(filterSorterSearcher);
            LoadFilter(filterSorterSearcher);
            LoadSorter(filterSorterSearcher);
        }

        private void LoadSearcher(TourFilterSorterSearcher filterSorterSearcher)
        {
            TourNameSearcher.Text = filterSorterSearcher.search.tourNameSubstr;
            TourLocationSearcher.Text = filterSorterSearcher.search.tourLocationSubstr;
        }

        private void LoadFilter(TourFilterSorterSearcher filterSorterSearcher)
        {
            MinPricePicker.Val = filterSorterSearcher.filters.priceFilter.from;
            MaxPricePicker.Val = filterSorterSearcher.filters.priceFilter.to;

            MinStartDate.Date = filterSorterSearcher.filters.startDateFilter.from;
            MaxStartDate.Date = filterSorterSearcher.filters.startDateFilter.to;

            MinPlacesPicker.Val = filterSorterSearcher.filters.participantsFilter.from;
            MaxPlacesPicker.Val = filterSorterSearcher.filters.participantsFilter.to;
        }

        private void LoadSorter(TourFilterSorterSearcher filterSorterSearcher)
        {
            switch (filterSorterSearcher.sortingCriterion)
            {
                case TourSortCriteria.price: PriceSortedChecker.Checked = true; break;
                case TourSortCriteria.free_places: FreePlacesSortedChecker.Checked = true; break;
                default: break;
            }
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

        private async void FilterPopupConfirm_Clicked(object sender, EventArgs e)
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            CurrentTourFilterSorterSearcher.FillFilters(MinPricePicker.Val, MaxPricePicker.Val, 
                                                        MinStartDate.Date, MaxStartDate.Date, 
                                                        (int)MinPlacesPicker.Val, (int)MaxPlacesPicker.Val);
            await UpdateToursAsync();
        }

        private void SearchPopupConfirm_Clicked(object sender, EventArgs e)
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            CurrentTourFilterSorterSearcher.FillSearch(TourNameSearcher.Text, TourLocationSearcher.Text);

            UpdateTours(!CurrentTourFilterSorterSearcher.filters.IsChanged);
        }

        private async void SortPopupConfirm_Clicked(object sender, EventArgs e)
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            if (PriceSortedChecker.Checked)
                CurrentTourFilterSorterSearcher.sortingCriterion = TourSortCriteria.price;
            else if (FreePlacesSortedChecker.Checked)
                CurrentTourFilterSorterSearcher.sortingCriterion = TourSortCriteria.free_places;
            else
                CurrentTourFilterSorterSearcher.sortingCriterion = TourSortCriteria.no;

            await UpdateToursAsync();
        }

        private void FilterPopupReset_Clicked(object sender, EventArgs e)
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            CurrentTourFilterSorterSearcher.filters.Reset();

            UpdateTours(true);
        }

        private void SearchPopupReset_Clicked(object sender, EventArgs e)
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            CurrentTourFilterSorterSearcher.search.Reset();
            ResetSearchView();

            UpdateTours(!CurrentTourFilterSorterSearcher.filters.IsChanged);
        }

        private async void SortPopupReset_Clicked(object sender, EventArgs e)
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            CurrentTourFilterSorterSearcher.sortingCriterion = TourSortCriteria.no;
            ResetSortView();
            await UpdateToursAsync();
        }

        private bool SortedCheckerLabel_OnClick(MotionEvent ME, IClickable sender) => ((sender as Element).BindingContext as IClickable).Click(ME);

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
            if (ME.Action != MotionEventActions.Down)
                return false;            

            UpdateTours(!CurrentTourFilterSorterSearcher.filters.IsChanged);

            return false;
        }

        private async void UpdateTours(bool resetFiltersView = false)
        {
            await UpdateToursAsync();
            if (resetFiltersView)
                ResetFilterView();
        }

        private async Task UpdateToursAsync()
        {
            Tours.Clear();
            FirstTourNum = 0;
            await GetAndLoadToursAsync(CurrentTourFilterSorterSearcher);
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
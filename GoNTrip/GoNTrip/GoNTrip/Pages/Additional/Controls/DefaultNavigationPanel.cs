using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.Views;

using CustomControls;

using Xamarin.Forms;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Model.FilterSortSearch.Tour;
using GoNTrip.Pages.Additional.Popups.Templates;

namespace GoNTrip.Pages.Additional.Controls
{
    public class DefaultNavigationPanel : NavigationPanel
    {
        public enum PageEnum
        {
            PROFILE,
            MESSAGES,
            TOUR_LIST,
            MY_TOUR_LIST,
            ADVANCED,
            OTHER
        };

        public PageEnum Current { get; set; }

        private const float HEIGHT = 50;
        private const float PADDING = 3;
        private const float SCALE_CLICKED = 0.9f;

        private const string PROFILE_NAVIGATION_BUTTON_SOURCE = "profile.png";
        private const string MESSAGES_NAVIGATION_BUTTON_SOURCE = "messages.png";
        private const string TOUR_LIST_NAVIGATION_BUTTON_SOURCE = "tourList.jpg";
        private const string MY_TOUR_LIST_NAVIGATION_BUTTON_SOURCE = "myTourList.png";
        private const string ADVANCED_NAVIGATION_BUTTON_SOURCE = "advanced.png";

        private const string NAVIGATOR_BACK_COLOR_NAME = "BarBackColor";

        private List<Img> navigationButtons = new List<Img>();

        public event Clicked OnProfileButtonClicked;
        public event Clicked OnMessagesButtonClicked;
        public event Clicked OnTourListButtonClicked;
        public event Clicked OnMyTourListButtonClicked;
        public event Clicked OnAdvancedButtonClicked;
        public event Clicked OnCurrentPageNavigationButtonClicked;

        public DefaultNavigationPanel() : base()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.EndAndExpand;
            Padding = new Thickness(PADDING);

            BackgroundColor = (Color)App.Current.Resources[NAVIGATOR_BACK_COLOR_NAME];
            HeightRequest = HEIGHT;

            Add(PROFILE_NAVIGATION_BUTTON_SOURCE, (ME, sender) => OnProfileButtonClicked(ME, sender), PageEnum.PROFILE);
            Add(MESSAGES_NAVIGATION_BUTTON_SOURCE, (ME, sender) => OnMessagesButtonClicked(ME, sender), PageEnum.MESSAGES);
            Add(TOUR_LIST_NAVIGATION_BUTTON_SOURCE, (ME, sender) => OnTourListButtonClicked(ME, sender), PageEnum.TOUR_LIST);
            Add(MY_TOUR_LIST_NAVIGATION_BUTTON_SOURCE, (ME, sender) => OnMyTourListButtonClicked(ME, sender), PageEnum.MY_TOUR_LIST);
            Add(ADVANCED_NAVIGATION_BUTTON_SOURCE, (ME, sender) => OnAdvancedButtonClicked(ME, sender), PageEnum.ADVANCED);
        }

        public void LinkClicks(PopupControlSystem popupControl, LoadingPopup loadingPopup)
        {
            OnProfileButtonClicked += (ME, sender) => { OpenPage<CurrentUserProfilePage>(popupControl, loadingPopup); return false; };
            OnMessagesButtonClicked += (ME, sender) => { OpenPage<MessagesPage>(popupControl, loadingPopup); return false; };
            OnTourListButtonClicked += (ME, sender) => { OpenPage<TourListPage>(popupControl, loadingPopup); return false; };
            OnMyTourListButtonClicked += (ME, sender) => { OpenMyToursPage(popupControl, loadingPopup); return false; };
            OnAdvancedButtonClicked += (ME, sender) => { OpenPage<AdvancedPage>(popupControl, loadingPopup); return false; };

            OnCurrentPageNavigationButtonClicked += (ME, sender) => false;
        }

        private async void OpenMyToursPage(PopupControlSystem popupControl, LoadingPopup loadingPopup)
        {
            popupControl.OpenPopup(loadingPopup);
            await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));

            TourFilterSorterSearcher tourFilterSorterSearcher = new TourFilterSorterSearcher();
            tourFilterSorterSearcher.semiFilters.tourMemberId = App.DI.Resolve<Session>().CurrentUser.id;

            App.Current.MainPage = new TourListPage(Constants.MY_TOUR_LIST_PAGE_CAPTION, PageEnum.MY_TOUR_LIST, null, tourFilterSorterSearcher);
        }

        private async void OpenPage<T>(PopupControlSystem popupControl, LoadingPopup loadingPopup) where T : ContentPage, new() =>
            await OpenPageAsync<T>(popupControl, loadingPopup);

        private async Task OpenPageAsync<T>(PopupControlSystem popupControl, LoadingPopup loadingPopup) where T : ContentPage, new()
        {
            popupControl.OpenPopup(loadingPopup);

            await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));
            App.Current.MainPage = new T();
        }

        private Img Add(string source, Clicked clicked, PageEnum pageType)
        {
            Img img = new Img();

            img.Source = source;

            img.Border = false;
            img.BorderOnClick = false;
            img.HorizontalOptions = LayoutOptions.Center;

            img.WidthRequest = HEIGHT - 2 * PADDING;
            img.HeightRequest = HEIGHT - 2 * PADDING;

            img.ScaleOnClicked = SCALE_CLICKED;

            img.OnClick += (ME, sender) =>
            {
                if (ME.Action != MotionEventActions.Down)
                    return false;

                if (pageType == Current)
                    OnCurrentPageNavigationButtonClicked?.Invoke(ME, sender);
                else
                    clicked?.Invoke(ME, sender);

                return false;
            };

            navigationButtons.Add(img);

            Add(img);
            return img;
        }
    }
}

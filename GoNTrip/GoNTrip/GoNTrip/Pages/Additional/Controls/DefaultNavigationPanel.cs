using System.Collections.Generic;

using Android.Views;

using CustomControls;

using Xamarin.Forms;

using Autofac;

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

        public const float HEIGHT = 50;
        public const float PADDING = 3;
        public const float SCALE_CLICKED = 0.9f;


        public const string PROFILE_NAVIGATION_BUTTON_SOURCE = "profile.png";
        public const string MESSAGES_NAVIGATION_BUTTON_SOURCE = "messages.png";
        public const string TOUR_LIST_NAVIGATION_BUTTON_SOURCE = "tourList.jpg";
        public const string MY_TOUR_LIST_NAVIGATION_BUTTON_SOURCE = "myTourList.png";
        public const string ADVANCED_NAVIGATION_BUTTON_SOURCE = "advanced.png";

        public const string NAVIGATOR_BACK_COLOR_NAME = "BarBackColor";
        public const string NAVIGATOR_BUTTONS_BORDER_COLOR_NAME = "AdditionalColor";

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

        public void LinkClicks()
        {
            OnProfileButtonClicked += (ME, sender) => { App.Current.MainPage = new CurrentUserProfilePage(); return false; };
            OnMessagesButtonClicked += (ME, sender) => { App.Current.MainPage = new MessagesPage(); return false; };
            OnTourListButtonClicked += (ME, sender) => { App.Current.MainPage = new TourListPage(); return false; };
            OnMyTourListButtonClicked += (ME, sender) => { App.Current.MainPage = new MyTourListPage(); return false; };
            OnAdvancedButtonClicked += (ME, sender) => { App.Current.MainPage = new AdvancedPage(); return false; };

            OnCurrentPageNavigationButtonClicked += (ME, sender) => false;
        }

        private Img Add(string source, Clicked clicked, PageEnum pageType)
        {
            Img img = new Img();

            img.Source = source;

            img.ClickedBorderWidth = 0;
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

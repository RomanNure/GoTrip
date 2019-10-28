using System;
using System.Threading;
using System.Threading.Tasks;

using Android.Views;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.Pages.Additional.Popups.Templates;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourPage : ContentPage
    {
        private const int SECONDARY_IMAGES_COUNT_IN_ROW = 3;
        private const int SECONDARY_IMAGE_BORDER_RADIUS = 18;
        private const float SECONDARY_IMAGE_BORDER_WIDTH = 0;
        private static readonly Color SECONDARY_IMAGE_BORDER_COLOR = (Color)App.Current.Resources["BarBackColor"];

        private PopupControlSystem PopupControl { get; set; }
        private SwipablePhotoCollection PhotoCollection { get; set; }

        private TourListPageMemento TourListPageMemento { get; set; }
        private Tour CurrentTour { get; set; }

        public TourPage(Tour tour, TourListPageMemento memento)
        {
            TourListPageMemento = memento;
            CurrentTour = tour;

            InitializeComponent();

            TourMainImagePreview.WidthRequest = Width;
            TourMainImagePreview.HeightRequest = Height;

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            PhotoCollection = new SwipablePhotoCollection(PopupControl);

            PhotoCollection.Add(TourMainImage);
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);
            await LoadCurrentTour();
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private async Task LoadCurrentTour()
        {
            await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));

            string tourMainImageSource = CurrentTour.mainPictureUrl == null ? Constants.DEFAULT_TOUR_IMAGE_SOURCE : CurrentTour.mainPictureUrl;
            TourMainImagePreview.Source = tourMainImageSource;
            TourMainImage.ImageSource = tourMainImageSource;

            TourSecondaryImages.Children.Clear();

            //CurrentTour.images.Add(Constants.DEFAULT_TOUR_IMAGE_SOURCE);
            //CurrentTour.images.Add(Constants.DEFAULT_TOUR_IMAGE_SOURCE);
            //CurrentTour.images.Add(Constants.DEFAULT_TOUR_IMAGE_SOURCE);
            //CurrentTour.images.Add(Constants.DEFAULT_TOUR_IMAGE_SOURCE);

            double secondaryImageWidth = (Width - TourContentWrapper.Margin.Left 
                                                - TourContentWrapper.Margin.Right 
                                                - TourSecondaryImages.ColumnSpacing * (SECONDARY_IMAGES_COUNT_IN_ROW - 1)) 
                                          / SECONDARY_IMAGES_COUNT_IN_ROW;

            for (int i = 0; i < CurrentTour.images.Count; i++)
            {
                Img img = new Img();

                img.WidthRequest = secondaryImageWidth;
                img.HeightRequest = secondaryImageWidth;

                img.ClickedBorderColor = SECONDARY_IMAGE_BORDER_COLOR;
                img.ClickedBorderWidth = SECONDARY_IMAGE_BORDER_WIDTH;
                img.BorderRadius = SECONDARY_IMAGE_BORDER_RADIUS;

                img.Source = CurrentTour.images[i];

                int picNum = i + 1;
                img.OnClick += (ME, ctx) =>
                {
                    if (ME.Action == MotionEventActions.Up)
                        for (int j = 0; j <= picNum; j++)
                            PhotoCollection.MoveNext();
                    return false;
                };

                TourSecondaryImages.Children.Add(img, i, 0);

                SwipablePhotoPopup photo = new SwipablePhotoPopup();

                photo.YTranslationBorder = Constants.PHOTO_POPUP_Y_TRANSLATION_BORDER;
                photo.XTranslationBorder = Constants.PHOTO_POPUP_X_TRANSLATION_BORDER;

                photo.ImageSource = CurrentTour.images[i];

                Layout.Children.Add(photo);
                PhotoCollection.Add(photo);
            }

            TourName.Text = CurrentTour.name;
            TourAbout.Text = CurrentTour.description;
            TourPriceInfoLabel.Text = CurrentTour.pricePerPerson + Constants.CURRENCY_SYMBOL;
            TourStartInfoLabel.Text = CurrentTour.startDateTime.ToString();
            TourEndInfoLabel.Text = CurrentTour.finishDateTime.ToString();

            TimeSpan duration = CurrentTour.finishDateTime - CurrentTour.startDateTime;
            TourDurationInfoLabel.Text = (duration.Days == 0 ? "" : duration.Days + " days ") +
                                         (duration.Hours == 0 ? "" : duration.Hours + " hours ") +
                                         (duration.Minutes == 0 ? "" : duration.Minutes + " minutes");
            TourPlacesInfoLabel.Text = "0/" + CurrentTour.maxParticipants;
        }

        private bool TourMainImagePreview_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Up)
                PhotoCollection.MoveNext();
            return false;
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
                App.Current.MainPage = new TourListPage(TourListPageMemento);
            else if (PhotoCollection.Opened)
                PhotoCollection.Reset();
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
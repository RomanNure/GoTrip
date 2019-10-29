using System;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using Android.Views;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.Pages.Additional.Popups.Templates;
using GoNTrip.Controllers;
using GoNTrip.ServerInteraction.ResponseParsers;

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
        private Company CurrentTourCompany { get; set; }
        private User CurrentTourAdmin { get; set; }

        public TourPage(Tour tour, TourListPageMemento memento)
        {
            TourListPageMemento = memento;
            CurrentTour = tour;

            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            TourMainImagePreview.WidthRequest = Width;
            TourMainImagePreview.HeightRequest = Width;

            PhotoCollection = new SwipablePhotoCollection(PopupControl);
            PhotoCollection.Add(TourMainImage);

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                CurrentTourAdmin = await GetCurrentTourAdminUserProfile(CurrentTour.administrator);
                CurrentTourCompany = await GeteCurrentTourCompany(CurrentTour.administrator);

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }

            PopupControl.OpenPopup(ActivityPopup);
            await LoadCurrentTour();
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private async Task<User> GetCurrentTourAdminUserProfile(Admin admin) => await App.DI.Resolve<GetUserByAdminController>().GetUserByAdmin(admin);
        private async Task<Company> GeteCurrentTourCompany(Admin admin) => await App.DI.Resolve<GetCompanyByAdminController>().GetCompanyByAdmin(admin);

        private async Task LoadCurrentTour()
        {
            await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));

            string tourMainImageSource = CurrentTour.mainPictureUrl == null ? Constants.DEFAULT_TOUR_IMAGE_SOURCE : CurrentTour.mainPictureUrl;
            TourMainImagePreview.Source = tourMainImageSource;
            TourMainImage.ImageSource = tourMainImageSource;

            TourSecondaryImages.Children.Clear();

            double secondaryImageWidth = (Width - TourContentWrapper.Margin.Left
                                                - TourContentWrapper.Margin.Right
                                                - TourSecondaryImages.ColumnSpacing * (SECONDARY_IMAGES_COUNT_IN_ROW - 1))
                                          / SECONDARY_IMAGES_COUNT_IN_ROW;

            for (int i = 0; i < CurrentTour.photos.Count; i++)
            {
                Img img = new Img();

                img.WidthRequest = secondaryImageWidth;
                img.HeightRequest = secondaryImageWidth;

                img.ClickedBorderColor = SECONDARY_IMAGE_BORDER_COLOR;
                img.ClickedBorderWidth = SECONDARY_IMAGE_BORDER_WIDTH;
                img.BorderRadius = SECONDARY_IMAGE_BORDER_RADIUS;

                img.Source = CurrentTour.photos[i].url;

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

                photo.ImageSource = CurrentTour.photos[i].url;

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

            CompanyImage.Source = CurrentTourCompany == null || CurrentTourCompany.imageLink == null ? Constants.DEFAULT_COMPANY_AVATAR_IMAGE_SOURCE : CurrentTourCompany.imageLink;
            OrganisatorCompanyName.Text = CurrentTourCompany == null ? String.Empty : CurrentTourCompany.name;

            AdminAvatar.Source = CurrentTourAdmin == null || CurrentTourAdmin.avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : CurrentTourAdmin.avatarUrl;
            TourAdminName.Text = CurrentTourAdmin == null ? String.Empty : CurrentTourAdmin.login;
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
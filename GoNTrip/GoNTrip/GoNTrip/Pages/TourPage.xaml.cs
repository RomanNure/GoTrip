using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Android.Views;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.Pages.Additional.Popups.Templates;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourPage : ContentPage
    {
        private const int TOUR_MEMBERS_AVATAR_COUNT_IN_ROW = 4;
        private const int SECONDARY_IMAGES_COUNT_IN_ROW = 3;

        private const int SECONDARY_IMAGE_CORNER_RADIUS = 18;
        private const float SECONDARY_IMAGE_BORDER_WIDTH = 0;

        private static readonly Color SECONDARY_IMAGE_BORDER_COLOR = (Color)App.Current.Resources["BarBackColor"];

        private PopupControlSystem PopupControl { get; set; }
        private SwipablePhotoCollection PhotoCollection { get; set; }

        private TourListPageMemento TourListPageMemento { get; set; }

        private Tour CurrentTour { get; set; }
        private Company CurrentTourCompany { get; set; }
        private User CurrentTourAdmin { get; set; }
        private User CurrentTourGuide { get; set; }

        public TourPage(Tour tour, TourListPageMemento memento)
        {
            TourListPageMemento = memento;
            CurrentTour = tour;

            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            TourMainImagePreview.WidthRequest = Width;
            TourMainImagePreview.HeightRequest = Width;

            PhotoCollection = new SwipablePhotoCollection(PopupControl);

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

            bool mainPhotoShifted = CurrentTour.mainPictureUrl == null && CurrentTour.photos.Count > 0;

            string mainPhotoSource = mainPhotoShifted ? CurrentTour.photos[0].url : (CurrentTour.mainPictureUrl != null ? CurrentTour.mainPictureUrl : Constants.DEFAULT_TOUR_IMAGE_SOURCE);
            List<string> secondaryPhotos = CurrentTour.photos.Skip(mainPhotoShifted ? 1 : 0).Select(P => P.url).ToList();

            if (!mainPhotoShifted && CurrentTour.mainPictureUrl == null)
                TourMainImagePreview.IsVisible = false;
            else
            {
                TourMainImagePreview.Source = mainPhotoSource;
                TourMainImage.ImageSource = mainPhotoSource;

                PhotoCollection.Add(TourMainImage);
            }

            TourSecondaryImages.Children.Clear();

            double secondaryImageWidth = (Width - TourContentWrapper.Margin.Left
                                                - TourContentWrapper.Margin.Right
                                                - TourSecondaryImages.ColumnSpacing * (SECONDARY_IMAGES_COUNT_IN_ROW - 1))
                                          / SECONDARY_IMAGES_COUNT_IN_ROW;

            for (int i = 0; i < secondaryPhotos.Count; i++)
            {
                Img img = new Img();

                img.WidthRequest = secondaryImageWidth;
                img.HeightRequest = secondaryImageWidth;

                img.BorderColor = SECONDARY_IMAGE_BORDER_COLOR;
                img.BorderWidth = SECONDARY_IMAGE_BORDER_WIDTH;
                img.CornerRad = SECONDARY_IMAGE_CORNER_RADIUS;

                img.Source = secondaryPhotos[i];

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
                Layout.Children.Add(photo);

                photo.ImageSource = CurrentTour.photos[i].url;

                photo.YTranslationBorder = Constants.PHOTO_POPUP_Y_TRANSLATION_BORDER;
                photo.XTranslationBorder = Constants.PHOTO_POPUP_X_TRANSLATION_BORDER;

                PhotoCollection.Add(photo);
            }

            TourName.Text = CurrentTour.name;//add unknowns
            TourAbout.Text = CurrentTour.description;
            TourPriceInfoLabel.Text = CurrentTour.pricePerPerson + Constants.CURRENCY_SYMBOL;
            TourStartInfoLabel.Text = CurrentTour.startDateTime.ToString();
            TourEndInfoLabel.Text = CurrentTour.finishDateTime.ToString();

            TimeSpan duration = CurrentTour.finishDateTime - CurrentTour.startDateTime;
            TourDurationInfoLabel.Text = (duration.Days == 0 ? "" : duration.Days + " days ") +
                                         (duration.Hours == 0 ? "" : duration.Hours + " hours ") +
                                         (duration.Minutes == 0 ? "" : duration.Minutes + " minutes");

            TourPlacesInfoLabel.Text = CurrentTour.participatingList.Count + "/" + CurrentTour.maxParticipants;
            TourLocationInfoLabel.Text = CurrentTour.location == null ? Constants.UNKNOWN_FILED_VALUE : CurrentTour.location;

            CompanyImage.Source = CurrentTourCompany == null || CurrentTourCompany.imageLink == null ? Constants.DEFAULT_COMPANY_AVATAR_IMAGE_SOURCE : CurrentTourCompany.imageLink;
            OrganisatorCompanyName.Text = CurrentTourCompany == null || CurrentTourCompany.name == null ? String.Empty : CurrentTourCompany.name;
            OrganisatorCompanyWebSite.Text = CurrentTourCompany == null || CurrentTourCompany.email == null ? String.Empty : CurrentTourCompany.email;

            AdminAvatar.Source = CurrentTourAdmin == null || CurrentTourAdmin.avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : CurrentTourAdmin.avatarUrl;
            TourAdminName.Text = CurrentTourAdmin == null || CurrentTourAdmin.login == null ? String.Empty : CurrentTourAdmin.login;
            TourAdminEmail.Text = CurrentTourAdmin == null || CurrentTourAdmin.email == null ? String.Empty : CurrentTourAdmin.email;

            double tourMemberAvatarWidth = (Width - TourContentWrapper.Margin.Left
                                                  - TourContentWrapper.Margin.Right
                                                  - TourMembers.ColumnSpacing * (TOUR_MEMBERS_AVATAR_COUNT_IN_ROW - 1))
                                          / TOUR_MEMBERS_AVATAR_COUNT_IN_ROW;

            int columnCount = Math.Min(TOUR_MEMBERS_AVATAR_COUNT_IN_ROW, CurrentTour.participatingList.Count);
            for (int i = 0; i < columnCount; i++)
                TourMembers.ColumnDefinitions.Add(new ColumnDefinition() { Width = tourMemberAvatarWidth });

            for (int j = 0; j < CurrentTour.participatingList.Count; j++)
            {
                Img img = new Img();

                img.WidthRequest = tourMemberAvatarWidth;
                img.HeightRequest = tourMemberAvatarWidth;

                img.BorderColor = SECONDARY_IMAGE_BORDER_COLOR;
                img.BorderWidth = SECONDARY_IMAGE_BORDER_WIDTH;
                img.CornerRad = SECONDARY_IMAGE_CORNER_RADIUS;

                User member = await App.DI.Resolve<GetProfileController>().GetUserById(CurrentTour.participatingList[j].id);
                img.Source = member.avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : member.avatarUrl;

                img.OnClick += (ME, ctx) =>
                {
                    if (ME.Action == MotionEventActions.Up)
                        OpenLinkedUser(member);
                    return false;
                };

                Label login = new Label() { Style = (Style)App.Current.Resources["InfoCell"], Text = member.login };

                TourMembers.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                TourMembers.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                int row = j / TOUR_MEMBERS_AVATAR_COUNT_IN_ROW;
                int col = j % TOUR_MEMBERS_AVATAR_COUNT_IN_ROW;

                TourMembers.Children.Add(img, col, 2 * row);
                TourMembers.Children.Add(login, col, 2 * row + 1);
            }

            TourMembersWrapper.IsVisible = CurrentTour.participatingList.Count != 0;
        }

        private bool TourMainImagePreview_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Up)
                PhotoCollection.MoveNext();
            return false;
        }

        private bool AdminProfilePreview_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Up)
                OpenLinkedUser(CurrentTourAdmin);
            return false;
        }

        private bool GuideProfilePreview_OnClick(MotionEvent ME, IClickable sender)
        {
            //if (ME.Action == MotionEventActions.Up)
            //    OpenLinkedUser(CurrentTourGuide);
            return false;
        }

        private void OpenLinkedUser(User user)
        {
            PopupControl.OpenPopup(ActivityPopup);

            ObjectBuilder tourPageBuilder = new ObjectBuilder(typeof(TourPage), 
                new Type[] { typeof(Tour), typeof(TourListPageMemento) }, 
                new object[] { CurrentTour, TourListPageMemento }
            );

            App.Current.MainPage = App.DI.Resolve<Session>().CurrentUser.Equals(user) ? new CurrentUserProfilePage() as Page : 
                                                                                        new OtherUserProfilePage(user, tourPageBuilder);
        }

        private async void JoinTourButton_Clicked(object sender, EventArgs e) => 
            await App.DI.Resolve<JoinTourController>().JoinTour(CurrentTour);

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
            {
                PopupControl.OpenPopup(ActivityPopup);
                App.Current.MainPage = new TourListPage(TourListPageMemento);
            }
            else if (PhotoCollection.Opened)
                PhotoCollection.Reset();
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

using Autofac;

using Android.Views;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Util;
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

        private PageMemento PrevPageMemento { get; set; }
        private PageMemento CurrentPageMemento { get; set; }

        private Tour CurrentTour { get; set; }
        private Company CurrentTourCompany { get; set; }
        private User CurrentTourAdmin { get; set; }
        private List<User> Members { get; set; }

        [RestorableConstructor]
        public TourPage(Tour tour, PageMemento prevPageMemento)
        {
            PrevPageMemento = prevPageMemento;
            CurrentTour = tour;

            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            TicketQR.LinkControlSystem(PopupControl);

            TourMainImagePreview.WidthRequest = Width;
            TourMainImagePreview.HeightRequest = Width;

            PhotoCollection = new SwipablePhotoCollection(PopupControl);

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            CurrentPageMemento = new PageMemento();
            CurrentPageMemento.Save(this, tour, prevPageMemento);
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            await GetCurrentTour();
            await LoadCurrentTour();
        }

        private async Task<User> GetCurrentTourAdminUserProfile(Admin admin) => await App.DI.Resolve<GetUserByAdminController>().GetUserByAdmin(admin);
        private async Task<Company> GetCurrentTourCompany(Admin admin) => await App.DI.Resolve<CompanyController>().GetCompanyByAdmin(admin);
        private async Task<IEnumerable<User>> GetCurrentTourMembers() => await App.DI.Resolve<TourController>().GetTourMembers(CurrentTour);

        private async Task GetCurrentTour()
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                CurrentTourAdmin = await GetCurrentTourAdminUserProfile(CurrentTour.administrator);
                CurrentTourCompany = await GetCurrentTourCompany(CurrentTour.administrator);
                Members = (await GetCurrentTourMembers()).ToList();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private async Task LoadCurrentTour()
        {
            PopupControl.OpenPopup(ActivityPopup);
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

            string rate = "";
            try { rate = new string('★', (int)(await App.DI.Resolve<TourController>().GetAvgTourRate(CurrentTour)).value); }
            catch (ResponseException ex) { }

            TourName.Text = $"{(CurrentTour.name == null ? Constants.UNKNOWN_FILED_VALUE : CurrentTour.name)} {rate}";
            TourAbout.Text = CurrentTour.description == null ? Constants.UNKNOWN_FILED_VALUE : CurrentTour.description;
            TourPriceInfoLabel.Text = CurrentTour.pricePerPerson + Constants.CURRENCY_SYMBOL;
            TourStartInfoLabel.Text = CurrentTour.startDateTime == null ? Constants.UNKNOWN_FILED_VALUE : CurrentTour.startDateTime.ToString();
            TourEndInfoLabel.Text = CurrentTour.finishDateTime == null ? Constants.UNKNOWN_FILED_VALUE : CurrentTour.finishDateTime.ToString();

            TimeSpan duration = CurrentTour.startDateTime == null || CurrentTour.finishDateTime == null ? new TimeSpan(0) : CurrentTour.finishDateTime - CurrentTour.startDateTime;
            TourDurationInfoLabel.Text = duration.Ticks == 0 ? Constants.UNKNOWN_FILED_VALUE : (duration.Days == 0 ? "" : duration.Days + " days ") +
                                                                                               (duration.Hours == 0 ? "" : duration.Hours + " hours ") +
                                                                                               (duration.Minutes == 0 ? "" : duration.Minutes + " minutes");

            TourLocationInfoLabel.Text = CurrentTour.location == null ? Constants.UNKNOWN_FILED_VALUE : CurrentTour.location;

            CompanyImage.Source = CurrentTourCompany == null || CurrentTourCompany.imageLink == null ? Constants.DEFAULT_COMPANY_AVATAR_IMAGE_SOURCE : CurrentTourCompany.imageLink;
            OrganisatorCompanyName.Text = CurrentTourCompany == null || CurrentTourCompany.name == null ? String.Empty : CurrentTourCompany.name;
            OrganisatorCompanyWebSite.Text = CurrentTourCompany == null || CurrentTourCompany.email == null ? String.Empty : CurrentTourCompany.email;

            AdminAvatar.Source = CurrentTourAdmin == null || CurrentTourAdmin.avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : CurrentTourAdmin.avatarUrl;
            TourAdminName.Text = CurrentTourAdmin == null || CurrentTourAdmin.login == null ? String.Empty : CurrentTourAdmin.login;
            TourAdminEmail.Text = CurrentTourAdmin == null || CurrentTourAdmin.email == null ? String.Empty : CurrentTourAdmin.email;

            User currentUser = App.DI.Resolve<Session>().CurrentUser;

            try
            {
                JoinTourButton.IsEnabled = await App.DI.Resolve<TourController>().CheckJoinAbility(currentUser, CurrentTour);

                if (Members.Contains(currentUser))
                {
                    ParticipatingStatus status = await App.DI.Resolve<TourController>().GetParticipatingStatus(currentUser, CurrentTour);
                    TourFinishedButton.IsVisible = !status.finished && DateTime.Now >= CurrentTour.finishDateTime;
                    OpenTicketButton.IsVisible = true; //!status.participated;

                    Ticket ticket = await App.DI.Resolve<TicketsController>().GetTicket(CurrentTour);
                    TicketQR.FirstSource = await App.DI.Resolve<QrService>().Encode(ticket.Data);
                }
                else
                    OpenTicketButton.IsVisible = false;

                if (CurrentTour.guide == null)
                {
                    GuideProfile.IsVisible = false;
                    ScanTicketButton.IsVisible = false;
                    OfferGuidingButton.IsVisible = true;
                    OfferGuidingButton.IsEnabled = await App.DI.Resolve<GuideController>().CheckGuidingAbility(CurrentTour);
                }
                else
                {
                    GuideProfile.IsVisible = true;
                    OfferGuidingButton.IsVisible = false;

                    User guideUser = CurrentTour.guide.UserProfile;

                    TourGuideName.Text = guideUser.login;
                    TourGuideEmail.Text = guideUser.email;
                    GuideAvatar.Source = guideUser.avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : guideUser.avatarUrl;

                    ScanTicketButton.IsVisible = guideUser.Equals(currentUser) && DateTime.Now >= CurrentTour.startDateTime;
                }

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }

            await LoadCurrentTourMembers();
        }

        private async Task LoadCurrentTourMembers()
        {
            PopupControl.OpenPopup(ActivityPopup);
            await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));

            double tourMemberAvatarWidth = (Width - TourContentWrapper.Margin.Left
                                                  - TourContentWrapper.Margin.Right
                                                  - TourMembersWrapper.Padding.Left
                                                  - TourMembersWrapper.Padding.Right
                                                  - TourMembers.Margin.Left
                                                  - TourMembers.Margin.Right
                                                  - TourMembers.ColumnSpacing * (TOUR_MEMBERS_AVATAR_COUNT_IN_ROW - 1))
                                          / TOUR_MEMBERS_AVATAR_COUNT_IN_ROW;

            TourMembers.Children.Clear();
            TourMembers.RowDefinitions.Clear();
            TourMembers.ColumnDefinitions.Clear();

            int columnCount = Math.Min(TOUR_MEMBERS_AVATAR_COUNT_IN_ROW, Members.Count);
            for (int i = 0; i < columnCount; i++)
                TourMembers.ColumnDefinitions.Add(new ColumnDefinition() { Width = tourMemberAvatarWidth });

            for (int i = 0; i < Members.Count; i++)
            {
                Img img = new Img();

                img.WidthRequest = tourMemberAvatarWidth;
                img.HeightRequest = tourMemberAvatarWidth;

                img.BorderColor = SECONDARY_IMAGE_BORDER_COLOR;
                img.BorderWidth = SECONDARY_IMAGE_BORDER_WIDTH;
                img.CornerRad = SECONDARY_IMAGE_CORNER_RADIUS;

                img.Source = Members[i].avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : Members[i].avatarUrl;

                int memberi = i;
                img.OnClick += (ME, ctx) =>
                {
                    if (ME.Action == MotionEventActions.Up)
                        OpenLinkedUser(Members[memberi]);
                    return false;
                };

                Label login = new Label() { Style = (Style)App.Current.Resources["InfoCell"], Text = Members[i].login };

                TourMembers.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                TourMembers.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                int row = i / TOUR_MEMBERS_AVATAR_COUNT_IN_ROW;
                int col = i % TOUR_MEMBERS_AVATAR_COUNT_IN_ROW;

                TourMembers.Children.Add(img, col, 2 * row);
                TourMembers.Children.Add(login, col, 2 * row + 1);
            }            

            TourMembersWrapper.IsVisible = Members.Count != 0;
            TourPlacesInfoLabel.Text = CurrentTour.participatingList.Count + "/" + CurrentTour.maxParticipants;

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
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
            if (ME.Action == MotionEventActions.Up && CurrentTour.guide != null)
                OpenLinkedUser(CurrentTour.guide.UserProfile);
            return false;
        }

        private void OpenLinkedUser(User user)
        {
            PopupControl.OpenPopup(ActivityPopup);
            App.Current.MainPage = App.DI.Resolve<Session>().CurrentUser.Equals(user) ? new CurrentUserProfilePage(CurrentPageMemento) as Page : 
                                                                                        new OtherUserProfilePage(user, CurrentPageMemento);
        }

        private async void JoinTourButton_Clicked(object sender, EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);
            await Task.Run(() => Thread.Sleep(Constants.ACTIVITY_INDICATOR_START_TIMEOUT));

            App.Current.MainPage = new CardEnterPage(CurrentPageMemento, CurrentTour);
        }

        private void OfferGuidingButton_Clicked(object sender, EventArgs e) => PopupControl.OpenPopup(GuidingOfferPopup);

        private async void GuidingOfferConfirm_Clicked(object sender, EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                await App.DI.Resolve<GuideController>().OfferGuiding(CurrentTour, Convert.ToDouble(Salary.Text));
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private bool OpenTicketButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Up)
                PopupControl.OpenPopup(TicketQR);

            return false;
        }

        private bool ScanTicketButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Up)
                ScanTicket();

            return false;
        }

        private async void ScanTicket()
        {
            try
            {
                User user = App.DI.Resolve<Session>().CurrentUser;

                if (user.guide == null)
                    throw new ResponseException("You're not a guide");

                string data = await App.DI.Resolve<QrService>().ScanAsync();

                if (data == null || data == "")
                    throw new ResponseException("Scanning failed");

                TicketChecker ticketChecker = null;

                try { ticketChecker = new TicketChecker(data, user.guide, CurrentTour, MD5.Create()); }
                catch { throw new ResponseException("Scanning failed"); }

                if(!(await App.DI.Resolve<TicketsController>().CheckTicket(ticketChecker)))
                    throw new ResponseException("WARNING: Ticket is not authentic");

                ErrorPopup.MessageText = "Ticket accepted!";
                PopupControl.OpenPopup(ErrorPopup);
            }
            catch(ResponseException ex)
            {
                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private bool TourFinishedButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Up)
            {
                PopupControl.OpenPopup(TourFinishPopup);
                return true;
            }

            return false;
        }

        private async void FinishConfirmButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                await App.DI.Resolve<TourController>().FinishTour(CurrentTour, (int)TourRating.Val, (int)GuideRating.Val);

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
            {
                PopupControl.OpenPopup(ActivityPopup);
                App.Current.MainPage = PrevPageMemento.Restore();
            }
            else if (PhotoCollection.Opened)
                PhotoCollection.Reset();
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
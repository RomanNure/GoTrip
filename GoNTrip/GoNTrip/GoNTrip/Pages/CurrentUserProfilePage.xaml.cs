using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using Android.Views;

using GoNTrip.Util;
using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentUserProfilePage : ContentPage
    {
        private const string CONFIRMED_EMAIL_BACK_COLOR = "ContentBackColor";
        private const string NON_CONFIRMED_EMAIL_BACK_COLOR = "InvalidColor";

        private User CurrentUser { get; set; }

        private delegate Task<Stream> Load();
        private PopupControlSystem PopupControl = null;

        public CurrentUserProfilePage(User user)
        {
            CurrentUser = user;

            InitializeComponent();
            PopupControl = new PopupControlSystem(OnBackButtonPressed);
        }

        private void ProfilePage_Appearing(object sender, System.EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);
            LoadUserProfile();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            SelectAvatarSourcePopup.OnFirstButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Camera>().TakePhoto(Camera.CameraLocation.FRONT, Constants.MaxPhotoWidthHeight)); };
            SelectAvatarSourcePopup.OnSecondButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Gallery>().PickPhoto(Constants.MaxPhotoWidthHeight)); };
        }

        private async void LoadUserProfile()
        {
            try
            {
                User user = await App.DI.Resolve<GetProfileController>().GetUserById(CurrentUser.id);

                if (user != null)
                {
                    if (user.avatarUrl != null)
                        UserAvatar.Source = user.avatarUrl;

                    string login = user.login == null ? Constants.UNKNOWN_FILED_VALUE : user.login;

                    UserNameLabel.Text = login[0].ToString().ToUpper() + login.Substring(1) + "'s Profile";
                    LoginInfoLabel.Text = login;

                    NameInfoLabel.Text = user.fullName == null ? Constants.UNKNOWN_FILED_VALUE : user.fullName;

                    EmailInfoLabel.Text = user.email == null ? Constants.UNKNOWN_FILED_VALUE : user.email;
                    EmailInfoLabel.BackgroundColor = (Color)App.Current.Resources[user.email == null || !user.emailConfirmed ? NON_CONFIRMED_EMAIL_BACK_COLOR : CONFIRMED_EMAIL_BACK_COLOR];

                    PhoneInfoLabel.Text = user.phone == null ? Constants.UNKNOWN_FILED_VALUE : user.phone;
                }

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0 || PopupControl.IsKeyboardVisible())
                App.Current.MainPage = new MainPage();
            else
                PopupControl.CloseTopPopup();

            return true;
        }

        private bool UserAvatarChange_OnClick(MotionEvent ME, CustomControls.IClickable sender)
        {
            if(ME.Action == MotionEventActions.Down)
                PopupControl.OpenPopup(SelectAvatarSourcePopup);

            return false;
        }

        private async void UploadAvatar(Load avatarLoader)
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                Stream stream = await avatarLoader();

                if(stream == null)
                {
                    PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                    ErrorPopup.MessageText = Constants.FILE_SELECTION_ERROR;
                    PopupControl.OpenPopup(ErrorPopup);
                    return;
                }

                CurrentUser = await App.DI.Resolve<ChangeAvatarController>().ChangeAvatar(CurrentUser, stream);
                CurrentUser = await App.DI.Resolve<UpdateProfileController>().Update(CurrentUser);
                LoadUserProfile();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }
    }
}
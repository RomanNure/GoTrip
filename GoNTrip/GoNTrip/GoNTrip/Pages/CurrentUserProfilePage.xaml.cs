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
        private delegate Task<Stream> Load();

        private long UserId { get; set; }
        private PopupControlSystem PopupControl = null;

        public CurrentUserProfilePage(long userId)
        {
            UserId = userId;

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
                User user = await App.DI.Resolve<GetProfileController>().GetUserById(UserId);

                if (user != null)
                {
                    if (user.avatarUrl != null)
                        UserAvatar.Source = user.avatarUrl;

                    if (user.login != null)
                        UserNameLabel.Text = user.login[0].ToString().ToUpper() + user.login.Substring(1) + "'s Profile";
                }

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                
                //FILL PAGE
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

                FilePath url = await App.DI.Resolve<ChangeAvatarController>().ChangeAvatar(UserId, stream);
                UserAvatar.Source = ServerAccess.ServerCommunicator.MULTIPART_SERVER_URL + "/" + url.path;
                //send to db server

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
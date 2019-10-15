using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Android.Views;
using Android.Graphics;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.ServerInteraction.ResponseParsers;

using Plugin.Media;
using Plugin.Media.Abstractions;


namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private long UserId { get; set; }
        private PopupControlSystem PopupControl = null;

        public ProfilePage(long userId)
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

            SelectAvatarSourcePopup.OnFirstButtonClicked = (ctx, arg) => LoadAvatarFromCamera();
            SelectAvatarSourcePopup.OnSecondButtonClicked = (ctx, arg) => LoadAvatarFromGallery();
            SelectAvatarSourcePopup.OnThirdButtonClicked = (ctx, arg) => LoadAvatarFromURL();
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

        private async void LoadAvatarFromGallery()
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            PopupControl.OpenPopup(ActivityPopup);

            PickMediaOptions pickOptions = new PickMediaOptions();
            MediaFile file = await CrossMedia.Current.PickPhotoAsync(pickOptions);

            if(file != null)
            {
                Bitmap newAvatar = BitmapFactory.DecodeStream(file.GetStream());
                //load to server
            }

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private async void LoadAvatarFromCamera()
        {
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            PopupControl.OpenPopup(ActivityPopup);

            StoreCameraMediaOptions cameraOptions = new StoreCameraMediaOptions();
            MediaFile file = await CrossMedia.Current.TakePhotoAsync(cameraOptions);

            if (file != null)
            {
                await App.DI.Resolve<ChangeAvatarController>().ChangeAvatar(UserId, file.GetStream());
                //load to server
            }

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private void LoadAvatarFromURL()
        {
            /*TODO*/
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
        }
    }
}
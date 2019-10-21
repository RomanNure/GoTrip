using System;
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
using GoNTrip.Pages.Additional.Validators;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;
using GoNTrip.Pages.Additional.Validators.Templates;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentUserProfilePage : ContentPage
    {
        private const string CONFIRMED_EMAIL_BACK_COLOR = "ContentBackColor";
        private const string NON_CONFIRMED_EMAIL_BACK_COLOR = "InvalidColor";

        private User CurrentUser { get; set; }

        private delegate Task<Stream> Load();

        private PopupControlSystem PopupControl { get; set; }
        private EditProfileValidator EditProfileValidator { get; set; }

        public CurrentUserProfilePage(User user)
        {
            CurrentUser = user;

            InitializeComponent();
            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            EditProfileValidator = new EditProfileValidator(FirstNameEntry, LastNameEntry, PhoneEntry, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);
        }

        private void ProfilePage_Appearing(object sender, System.EventArgs e)
        {
            LoadUserProfile();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            SelectAvatarSourcePopup.OnFirstButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Camera>().TakePhoto(Camera.CameraLocation.FRONT, Constants.MAX_PHOTO_WIDTH_HEIGHT)); };
            SelectAvatarSourcePopup.OnSecondButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Gallery>().PickPhoto(Constants.MAX_PHOTO_WIDTH_HEIGHT)); };
        }

        private async void LoadUserProfile()
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                CurrentUser = await App.DI.Resolve<GetProfileController>().GetUserById(CurrentUser.id);
                LoadCurrentUserProfile();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private void LoadCurrentUserProfile()
        {
            if (CurrentUser != null)
            {
                string avatarSource = CurrentUser.avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : CurrentUser.avatarUrl;
                UserAvatar.Source = avatarSource;
                AvatarView.ImageSource = avatarSource;

                string login = CurrentUser.login == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.login;
                UserNameLabel.Text = login[0].ToString().ToUpper() + login.Substring(1) + "'s Profile";
                LoginInfoLabel.Text = login;

                if(CurrentUser.fullName == null)
                {
                    NameInfoLabel.Text = Constants.UNKNOWN_FILED_VALUE;
                    FirstNameEntry.Text = String.Empty;
                    LastNameEntry.Text = String.Empty;
                }
                else
                {
                    NameInfoLabel.Text = CurrentUser.fullName;

                    string[] names = CurrentUser.fullName.Split(Constants.FIRST_LAST_NAME_SPLITTER);
                    FirstNameEntry.Text = names[0];
                    LastNameEntry.Text = names[1];
                }

                EmailInfoLabel.Text = CurrentUser.email == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.email;
                EmailInfoLabel.BackgroundColor = (Color)App.Current.Resources[CurrentUser.email == null || !CurrentUser.emailConfirmed ? NON_CONFIRMED_EMAIL_BACK_COLOR : CONFIRMED_EMAIL_BACK_COLOR];

                PhoneEntry.Text = CurrentUser.phone == null ? String.Empty : CurrentUser.phone;
                PhoneInfoLabel.Text = CurrentUser.phone == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.phone;

                AdditionalUserInfo.IsVisible = CurrentUser.description != null && CurrentUser.description != "";
                AdditionalUserInfo.Text = CurrentUser.description == null || CurrentUser.description == "" ? String.Empty : CurrentUser.description;
            }
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

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                LoadCurrentUserProfile();
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private bool EditProfile_OnClick(MotionEvent ME, CustomControls.IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                PopupControl.OpenPopup(UpdateProfilePopup);
            return false;
        }

        private void EditProfilePopupConfirm_Clicked(object sender, EventArgs e)
        {
            if (!EditProfileValidator.ValidateAll())
                return;

            EditProfileAsync();
        }

        private async void EditProfileAsync()
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                CurrentUser.fullName = FirstNameEntry.Text + Constants.FIRST_LAST_NAME_SPLITTER.ToString() + LastNameEntry.Text;
                CurrentUser.phone = PhoneEntry.Text;

                CurrentUser = await App.DI.Resolve<UpdateProfileController>().Update(CurrentUser);
                LoadCurrentUserProfile();

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

        private bool UpdateProfile_OnClick(MotionEvent ME, CustomControls.IClickable sender)
        {
            LoadUserProfile();
            return true;
        }

        private bool UserAvatar_OnClick(MotionEvent ME, CustomControls.IClickable sender)
        {
            PopupControl.OpenPopup(AvatarView);
            return true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0 || PopupControl.IsKeyboardVisible())
                App.Current.MainPage = new MainPage();
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using Android.Views;

using CustomControls;

using GoNTrip.Util;
using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.Validators.Templates;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentUserProfilePage : ContentPage
    {
        private const char FIRST_LAST_NAME_SPLITTER = ' ';

        private const string CONFIRMED_EMAIL_BACK_COLOR = "ContentBackColor";
        private const string NON_CONFIRMED_EMAIL_BACK_COLOR = "InvalidColor";

        private delegate Task<Stream> Load();

        private PopupControlSystem PopupControl { get; set; }
        private EditProfileValidator EditProfileValidator { get; set; }

        public CurrentUserProfilePage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            AvatarView.LinkControlSystem(PopupControl);

            EditProfileValidator = new EditProfileValidator(FirstNameEntry, LastNameEntry, PhoneEntry, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);

            SelectAvatarSourcePopup.OnFirstButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Camera>().TakePhoto(Camera.CameraLocation.FRONT, Constants.MAX_PHOTO_WIDTH_HEIGHT)); };
            SelectAvatarSourcePopup.OnSecondButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Gallery>().PickPhoto(Constants.MAX_PHOTO_WIDTH_HEIGHT)); };

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            Navigator.Current = Additional.Controls.DefaultNavigationPanel.PageEnum.PROFILE;
            Navigator.LinkClicks(PopupControl, ActivityPopup);
        }

        private void ProfilePage_Appearing(object sender, EventArgs e) => LoadUserProfile();

        private async void LoadUserProfile()
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                Session session = App.DI.Resolve<Session>();
                session.CurrentUser = await App.DI.Resolve<GetProfileController>().GetUserById(session.CurrentUser.id);
                session.CurrentUser.AdministratedCompanies = await App.DI.Resolve<GetAdministratedCompaniesController>().GetAdministratedCompanies(session.CurrentUser);
                session.CurrentUser.OwnedCompanies = await App.DI.Resolve<GetOwnedCompaniesController>().GetOwnedCompanies(session.CurrentUser);

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
            User CurrentUser = App.DI.Resolve<Session>().CurrentUser;
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

                    string[] names = CurrentUser.fullName.Split(FIRST_LAST_NAME_SPLITTER);
                    FirstNameEntry.Text = names[0];
                    LastNameEntry.Text = names[1];
                }

                EmailInfoLabel.Text = CurrentUser.email == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.email;
                EmailInfoLabel.BackgroundColor = (Color)App.Current.Resources[CurrentUser.email == null || !CurrentUser.emailConfirmed ? NON_CONFIRMED_EMAIL_BACK_COLOR : CONFIRMED_EMAIL_BACK_COLOR];

                PhoneEntry.Text = CurrentUser.phone == null ? String.Empty : CurrentUser.phone;
                PhoneInfoLabel.Text = CurrentUser.phone == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.phone;

                UserAbout.Text = CurrentUser.description == null ? String.Empty : CurrentUser.description;

                string owned = CurrentUser.OwnedCompanies.Count == 0 ? "" : $"Owner of {String.Join(", ", CurrentUser.OwnedCompanies)}";
                string admined = CurrentUser.AdministratedCompanies.Count == 0 ? "" : $"Admin of {String.Join(", ", CurrentUser.AdministratedCompanies)}";

                AdditionalUserInfo.Text = String.Join("\n", new string[] { owned, admined }).Trim();

                AvatarView.Sign.Text = login + (AdditionalUserInfo.Text == "" ? "" : " - " + AdditionalUserInfo.Text);
                AvatarView.Sign.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;
            }
        }

        private bool UserAvatarChange_OnClick(MotionEvent ME, IClickable sender)
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

                Session session = App.DI.Resolve<Session>();
                session.CurrentUser = await App.DI.Resolve<ChangeAvatarController>().ChangeAvatar(session.CurrentUser, stream);
                session.CurrentUser = await App.DI.Resolve<UpdateProfileController>().Update(session.CurrentUser);

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

        private bool EditProfile_OnClick(MotionEvent ME, IClickable sender)
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
                Session session = App.DI.Resolve<Session>();
                session.CurrentUser.fullName = FirstNameEntry.Text + FIRST_LAST_NAME_SPLITTER.ToString() + LastNameEntry.Text;
                session.CurrentUser.phone = PhoneEntry.Text;

                session.CurrentUser = await App.DI.Resolve<UpdateProfileController>().Update(session.CurrentUser);
                session.CurrentUser.AdministratedCompanies = await App.DI.Resolve<GetAdministratedCompaniesController>().GetAdministratedCompanies(session.CurrentUser);
                session.CurrentUser.OwnedCompanies = await App.DI.Resolve<GetOwnedCompaniesController>().GetOwnedCompanies(session.CurrentUser);

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

        private bool UpdateProfile_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                LoadUserProfile();

            return false;
        }

        private bool UserAvatar_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                PopupControl.OpenPopup(AvatarView);
            
            return false;
        }

        private bool UserAboutSave_OnClick(MotionEvent ME, IClickable sender)
        {
            if(ME.Action == MotionEventActions.Down)
                UserAboutSaveAsync();

            return false;
        }

        private async void UserAboutSaveAsync()
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                Session session = App.DI.Resolve<Session>();
                session.CurrentUser.description = UserAbout.Text;

                session.CurrentUser = await App.DI.Resolve<UpdateProfileController>().Update(session.CurrentUser);
                session.CurrentUser.AdministratedCompanies = await App.DI.Resolve<GetAdministratedCompaniesController>().GetAdministratedCompanies(session.CurrentUser);
                session.CurrentUser.OwnedCompanies = await App.DI.Resolve<GetOwnedCompaniesController>().GetOwnedCompanies(session.CurrentUser);

                LoadCurrentUserProfile();

                UserAbout.IsReadOnly = true;
                UserAboutSave.IsVisible = false;
                UserAboutEdit.IsVisible = true;

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private bool UserAboutEdit_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
            {
                UserAboutSave.IsVisible = true;
                UserAboutEdit.IsVisible = false;

                UserAbout.IsReadOnly = false;
                UserAbout.Focus();
            }

            return false;
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
                PopupControl.OpenPopup(ExitConfirmPopup);
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
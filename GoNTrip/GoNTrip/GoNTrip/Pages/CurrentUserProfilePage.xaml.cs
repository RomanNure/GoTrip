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
        private Constants.Callback<Entry> SubscribeUpdateProfileEvents = null;
        private FormValidator UpdateProfileValidator = new FormValidator();

        public CurrentUserProfilePage(User user)
        {
            CurrentUser = user;

            InitializeComponent();
            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            FieldValidationHandler<Entry> FirstNameValidation = new FieldValidationHandler<Entry>(
                FirstName => FirstName.Text != null && UserFieldsPatterns.FIRST_NAME_PATTERN.IsMatch(FirstName.Text),
                Constants.InvalidHandler, Constants.ValidHandler
            );

            FieldValidationHandler<Entry> LastNameValidation = new FieldValidationHandler<Entry>(
                LastName => LastName.Text != null && UserFieldsPatterns.LAST_NAME_PATTERN.IsMatch(LastName.Text),
                Constants.InvalidHandler, Constants.ValidHandler
            );

            FieldValidationHandler<Entry> PhoneValidation = new FieldValidationHandler<Entry>(
                Phone => Phone.Text != null && UserFieldsPatterns.PHONE_PATTERN.IsMatch(Phone.Text),
                Constants.InvalidHandler, Constants.ValidHandler
            );

            SubscribeUpdateProfileEvents = T =>
            {
                T.Unfocused += ProfileUpdateValidatedFieldUnfocused;
                T.TextChanged += ProfileUpdateValidatedFieldTextChanged;
            };

            UpdateProfileValidator.Add<Entry>(FirstNameValidation, FirstNameEntry, SubscribeUpdateProfileEvents);
            UpdateProfileValidator.Add<Entry>(LastNameValidation, LastNameEntry, SubscribeUpdateProfileEvents);
            UpdateProfileValidator.Add<Entry>(PhoneValidation, PhoneEntry, SubscribeUpdateProfileEvents);
        }

        private void ProfileUpdateValidatedFieldTextChanged(object sender, TextChangedEventArgs e) => UpdateProfileValidator.ValidateId(UpdateProfileValidator.GetId(sender));
        private void ProfileUpdateValidatedFieldUnfocused(object sender, FocusEventArgs e) => UpdateProfileValidator.ValidateId(UpdateProfileValidator.GetId(sender));

        private void ProfilePage_Appearing(object sender, System.EventArgs e)
        {
            LoadUserProfile();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            SelectAvatarSourcePopup.OnFirstButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Camera>().TakePhoto(Camera.CameraLocation.FRONT, Constants.MaxPhotoWidthHeight)); };
            SelectAvatarSourcePopup.OnSecondButtonClicked = (ctx, arg) => { UploadAvatar(async () => await App.DI.Resolve<Gallery>().PickPhoto(Constants.MaxPhotoWidthHeight)); };
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
                if (CurrentUser.avatarUrl != null)
                {
                    UserAvatar.Source = CurrentUser.avatarUrl;
                    AvatarView.ImageSource = this.CurrentUser.avatarUrl;
                }

                string login = CurrentUser.login == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.login;

                UserNameLabel.Text = login[0].ToString().ToUpper() + login.Substring(1) + "'s Profile";
                LoginInfoLabel.Text = login;

                NameInfoLabel.Text = CurrentUser.fullName == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.fullName;
                if (CurrentUser.fullName != null)
                {
                    string[] names = CurrentUser.fullName.Split(Constants.FIRST_LAST_NAME_SPLITTER);
                    FirstNameEntry.Text = names[0];
                    LastNameEntry.Text = names[1];
                }

                EmailInfoLabel.Text = CurrentUser.email == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.email;
                EmailInfoLabel.BackgroundColor = (Color)App.Current.Resources[CurrentUser.email == null || !CurrentUser.emailConfirmed ? NON_CONFIRMED_EMAIL_BACK_COLOR : CONFIRMED_EMAIL_BACK_COLOR];

                if (CurrentUser.phone != null)
                    PhoneEntry.Text = CurrentUser.phone;

                PhoneInfoLabel.Text = CurrentUser.phone == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.phone;
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

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                LoadCurrentUserProfile();
                //LoadUserProfile();
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private bool UserAvatar_OnClick(MotionEvent ME, CustomControls.IClickable sender)
        {
            PopupControl.OpenPopup(AvatarView);
            return true;
        }

        private bool UpdateProfile_OnClick(MotionEvent ME, CustomControls.IClickable sender)
        {
            LoadUserProfile();
            return true;
        }

        private async void EditProfilePopupConfirm_Clicked(object sender, EventArgs e)
        {
            PopupControl.CloseTopPopup();
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                if (!UpdateProfileValidator.ValidateAll())
                    return;

                CurrentUser.fullName = FirstNameEntry.Text + Constants.FIRST_LAST_NAME_SPLITTER.ToString() + LastNameEntry.Text;
                CurrentUser.phone = PhoneEntry.Text;

                CurrentUser = await App.DI.Resolve<UpdateProfileController>().Update(CurrentUser);
                LoadCurrentUserProfile();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
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
            PopupControl.OpenPopup(UpdateProfilePopup);
            return true;
        }
    }
}
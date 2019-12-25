﻿using System;
using System.Threading.Tasks;

using Android.Views;

using Autofac;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtherUserProfilePage : ContentPage
    {
        private const string DEFAULT_USER_DESCRIPTION = "Hi, I'm using Go&Trip";

        private const string CONFIRMED_EMAIL_BACK_COLOR = "ContentBackColor";
        private const string NON_CONFIRMED_EMAIL_BACK_COLOR = "InvalidColor";

        private User CurrentUser { get; set; }
        private PageMemento PrevPageMemento { get; set; }

        private PopupControlSystem PopupControl { get; set; }

        public OtherUserProfilePage(User user, PageMemento prevPageMemento)//when implementing messaging add CurrentPageMemento prop
        {
            CurrentUser = user;
            PrevPageMemento = prevPageMemento;

            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            AvatarView.LinkControlSystem(PopupControl);

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
        }

        private void ContentPage_Appearing(object sender, EventArgs e) => GetAndLoadCurrentUserProfile();

        private async void GetAndLoadCurrentUserProfile()
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                CurrentUser.AdministratedCompanies = await App.DI.Resolve<GetAdministratedCompaniesController>().GetAdministratedCompanies(CurrentUser);
                CurrentUser.OwnedCompanies = await App.DI.Resolve<GetOwnedCompaniesController>().GetOwnedCompanies(CurrentUser);

                await LoadCurrentUserProfile();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private async Task LoadCurrentUserProfile()
        {
            if (CurrentUser != null)
            {
                string avatarSource = CurrentUser.avatarUrl == null ? Constants.DEFAULT_AVATAR_SOURCE : CurrentUser.avatarUrl;
                UserAvatar.Source = avatarSource;
                AvatarView.ImageSource = avatarSource;

                string login = CurrentUser.login == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.login;

                string rate = "";
                if (CurrentUser.guide != null)
                {
                    try { rate = new string('★', (int)(await App.DI.Resolve<GuideController>().GetAvgGuideRate(CurrentUser.guide)).value); }
                    catch (ResponseException ex) { }
                }

                UserNameLabel.Text = login[0].ToString().ToUpper() + login.Substring(1) + "'s Profile " + rate;
                LoginInfoLabel.Text = login;

                NameInfoLabel.Text = CurrentUser.fullName == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.fullName;
                EmailInfoLabel.Text = CurrentUser.email == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.email;
                EmailInfoLabel.BackgroundColor = (Color)App.Current.Resources[CurrentUser.email == null || !CurrentUser.emailConfirmed ? NON_CONFIRMED_EMAIL_BACK_COLOR : CONFIRMED_EMAIL_BACK_COLOR];

                PhoneInfoLabel.Text = CurrentUser.phone == null ? Constants.UNKNOWN_FILED_VALUE : CurrentUser.phone;

                UserAbout.Text = CurrentUser.description == null ? DEFAULT_USER_DESCRIPTION : CurrentUser.description;

                string owned = CurrentUser.OwnedCompanies.Count == 0 ? "" : $"Owner of {String.Join(", ", CurrentUser.OwnedCompanies)}";
                string admined = CurrentUser.AdministratedCompanies.Count == 0 ? "" : $"Admin of {String.Join(", ", CurrentUser.AdministratedCompanies)}";
                string guide = CurrentUser.guide == null ? "" : "Guide";

                AdditionalUserInfo.Text = String.Join("\n", new string[] { owned, admined, guide }).Trim();

                AvatarView.Sign.Text = login + (AdditionalUserInfo.Text == "" ? "" : " - " + AdditionalUserInfo.Text);
                AvatarView.Sign.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;
            }
        }

        private bool UserAvatar_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
                PopupControl.OpenPopup(AvatarView);

            return false;
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
            {
                PopupControl.OpenPopup(ActivityPopup);
                App.Current.MainPage = PrevPageMemento.Restore();
            }
            else
                PopupControl.CloseTopPopup();

            return true;
        }

        private bool SendMessage_OnClick(MotionEvent ME, IClickable sender)
        {
            return false;
        }
    }
}
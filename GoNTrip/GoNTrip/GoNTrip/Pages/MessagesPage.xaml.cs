using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Controls;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesPage : ContentPage
    {
        private const string NOTIFICATION_CONTENT_CLASS_NAME = "NotificationContent";
        private const string NOTIFICATION_WRAPPER_CLASS_NAME = "NotificationWrapper";
        private const string NOTIFICATION_UNREAD_FLAG_CLASS_NAME = "UnreadFlag";

        private PopupControlSystem PopupControl { get; set; }

        public MessagesPage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ExitConfirmPopup.OnFirstButtonClicked = (ctx, arg) => App.Current.MainPage = new MainPage();
            ExitConfirmPopup.OnSecondButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            Navigator.Current = Additional.Controls.DefaultNavigationPanel.PageEnum.MESSAGES;
            Navigator.LinkClicks(PopupControl, ActivityPopup);
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);

            List<Notification> notifications = new List<Notification>();

            try
            {
                notifications = await App.DI.Resolve<NotificationsController>().GetNotifications();
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }

            foreach(Notification notification in notifications)
            {
                NotificationPreview preview = new NotificationPreview();

                preview.Style = Resources[NOTIFICATION_WRAPPER_CLASS_NAME] as Style;
                preview.ContentStyle = Resources[NOTIFICATION_CONTENT_CLASS_NAME] as Style;
                preview.UnreadFlagStyle = Resources[NOTIFICATION_UNREAD_FLAG_CLASS_NAME] as Style;

                preview.FillWith(notification);

                NotificationsWrapper.Children.Add(preview);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount != 0)
                PopupControl.CloseTopPopup();
            else
                PopupControl.OpenPopup(ExitConfirmPopup);

            return true;
        }
    }
}
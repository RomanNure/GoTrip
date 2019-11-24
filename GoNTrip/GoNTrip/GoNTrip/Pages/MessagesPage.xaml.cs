using System;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using Android.Views;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Model.Notifications;
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

            ConfirmableNotificationClose.Clicked += (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            NotConfirmableNotificationClose.Clicked += (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            Navigator.Current = DefaultNavigationPanel.PageEnum.MESSAGES;
            Navigator.LinkClicks(PopupControl, ActivityPopup);
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);

            NotificationController notificationController = App.DI.Resolve<NotificationController>();
            List<INotification> notifications = new List<INotification>();

            try
            { 
                List<RawNotification> rawNotifications = await notificationController.GetNotifications();

                foreach (RawNotification rawNotification in rawNotifications)
                    notifications.Add(await notificationController.ParseRawNotification(rawNotification));

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }

            foreach(INotification notification in notifications)
            {
                NotificationPreview preview = new NotificationPreview();

                preview.Style = Resources[NOTIFICATION_WRAPPER_CLASS_NAME] as Style;
                preview.ContentStyle = Resources[NOTIFICATION_CONTENT_CLASS_NAME] as Style;
                preview.UnreadFlagStyle = Resources[NOTIFICATION_UNREAD_FLAG_CLASS_NAME] as Style;

                preview.FillWith(notification);

                INotification ntf = notification;
                preview.OnClick += (ME, arg) =>
                {
                    if (ME.Action != MotionEventActions.Down)
                        return false;

                    if (!ntf.IsChecked)
                        SeeNotification(ntf, preview);

                    if (ntf.IsConfirmable)
                    {
                        ConfirmableNotificationTopic.Text = ntf.Topic;
                        ConfirmableNotificationText.Text = ntf.GetDetailMessage();
                        ConfirmableNotificationConfirm.Clicked += (s, a) =>
                        {
                            ConfirmNotification(ntf);
                            DisplayAlert("t", "confirmed " + ntf.Topic, "OK");
                        };
                        ConfirmableNotificationRefuse.Clicked += (s, a) =>
                        {
                            RefuseNotification(ntf);
                            DisplayAlert("t", "refused " + ntf.Topic, "OK");
                        };
                        PopupControl.OpenPopup(ConfirmableNotificationPopup);
                    }
                    else
                    {
                        NotConfirmableNotificationTopic.Text = ntf.Topic;
                        NotConfirmableNotificationText.Text = ntf.GetDetailMessage();
                        PopupControl.OpenPopup(NotConfirmableNotificationPopup);
                    }

                    return false;
                };

                NotificationsWrapper.Children.Add(preview);
            }
        }

        private async void SeeNotification(INotification ntf, NotificationPreview preview)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                await App.DI.Resolve<NotificationController>().SeeNotification(ntf);
                preview.Read();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private async void ConfirmNotification(INotification ntf)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);
                await App.DI.Resolve<NotificationController>().AcceptOffer(ntf);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private async void RefuseNotification(INotification ntf)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);
                await App.DI.Resolve<NotificationController>().RefuseOffer(ntf);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
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
            if (PopupControl.OpenedPopupsCount != 0)
                PopupControl.CloseTopPopup();
            else
                PopupControl.OpenPopup(ExitConfirmPopup);

            return true;
        }
    }
}
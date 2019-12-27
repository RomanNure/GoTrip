using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using Android.Views;

using GoNTrip.Util;
using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Model.Notifications;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.Controls;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.PageMementos;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesPage : ContentPage
    {
        private const string NOTIFICATION_CONTENT_CLASS_NAME = "NotificationContent";
        private const string NOTIFICATION_WRAPPER_CLASS_NAME = "NotificationWrapper";
        private const string NOTIFICATION_UNREAD_FLAG_CLASS_NAME = "UnreadFlag";

        private PopupControlSystem PopupControl { get; set; }
        private PageMemento CurrentPageMemento { get; set; }

        [RestorableConstructor]
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

            CurrentPageMemento = new PageMemento();
            CurrentPageMemento.Save(this);
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

            int row = -1;
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

                        cEventHelper.RemoveEventHandler(ConfirmableNotificationConfirm, "Clicked");
                        cEventHelper.RemoveEventHandler(ConfirmableNotificationRefuse, "Clicked");
                        cEventHelper.RemoveEventHandler(ConfirmableNotificationViewTour, "Clicked");

                        if(!(ntf is CustomGuidePropositionNotification))
                            ConfirmableNotificationConfirm.Clicked += (s, a) => ConfirmNotification(ntf, preview);
                        else
                        {
                            CustomGuidePropositionNotification cgontf = ntf as CustomGuidePropositionNotification;
                            ConfirmableNotificationConfirm.Clicked += (s, a) =>
                            {
                                ConfirmNotification(ntf, preview);
                                App.Current.MainPage = new CardEnterPage(CurrentPageMemento, cgontf.tour, cgontf.sum);
                            };
                        }

                        ConfirmableNotificationRefuse.Clicked += (s, a) => RefuseNotification(ntf, preview);

                        if (ntf is GuidingOfferNotification || ntf is GuideToAdminPropositionNotification || ntf is CustomGuidePropositionNotification)
                        {
                            GuidingOfferNotification gontf = ntf as GuidingOfferNotification;
                            GuideToAdminPropositionNotification gtapn = ntf as GuideToAdminPropositionNotification;
                            CustomGuidePropositionNotification cgpn = ntf as CustomGuidePropositionNotification;

                            Tour selectedTour = gontf?.tour ?? gtapn?.tour ?? cgpn?.tour;

                            ConfirmableNotificationViewTour.Clicked += (s, a) =>
                                App.Current.MainPage = new TourPage(selectedTour, CurrentPageMemento);
                        }
                        else
                            ConfirmableNotificationViewTour.IsVisible = false;

                        PopupControl.OpenPopup(ConfirmableNotificationPopup);
                    }
                    else
                    {
                        NotConfirmableNotificationTopic.Text = ntf.Topic;
                        NotConfirmableNotificationText.Text = ntf.GetDetailMessage();

                        cEventHelper.RemoveEventHandler(NotConfirmableNotificationDelete, "Clicked");
                        NotConfirmableNotificationDelete.Clicked += (s, a) => DeleteNotification(ntf, preview);
                        
                        PopupControl.OpenPopup(NotConfirmableNotificationPopup);
                    }

                    return false;
                };

                NotificationsWrapper.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                NotificationsWrapper.Children.Add(preview, 0, ++row);
            }
        }

        private async void SeeNotification(INotification ntf, NotificationPreview preview)
        {
            try
            {
                await App.DI.Resolve<NotificationController>().SeeNotification(ntf);
                preview.Read();
            }
            catch(ResponseException ex)
            {
                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private async void DeleteNotification(INotification ntf, NotificationPreview preview)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                NotificationController notificationController = App.DI.Resolve<NotificationController>();
                await notificationController.DeleteNotification(ntf);
                NotificationsWrapper.Children.Remove(preview);

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

        private async void ConfirmNotification(INotification ntf, NotificationPreview preview)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                NotificationController notificationController = App.DI.Resolve<NotificationController>();
                await notificationController.AcceptOffer(ntf);
                await notificationController.DeleteNotification(ntf);
                NotificationsWrapper.Children.Remove(preview);

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

        private async void RefuseNotification(INotification ntf, NotificationPreview preview)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                NotificationController notificationController = App.DI.Resolve<NotificationController>();
                await notificationController.RefuseOffer(ntf);
                await notificationController.DeleteNotification(ntf);
                NotificationsWrapper.Children.Remove(preview);

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
            if (PopupControl.OpenedPopupsCount != 0)
                PopupControl.CloseTopPopup();
            else
                PopupControl.OpenPopup(ExitConfirmPopup);

            return true;
        }
    }
}
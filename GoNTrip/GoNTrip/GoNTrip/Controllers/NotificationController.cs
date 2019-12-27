using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.Model.Notifications;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class NotificationController
    {

        private const string CUSTOM_GUIDE_PROPOSITION_NOTICATION_TYPE = "CustomGuideProposition";
        private const string GUIDE_PROPOSITION_NOTICATION_TYPE = "GuideProposition";
        private const string GUIDING_OFFER_NOTIFICATION_TYPE = "OfferGuiding";
        private const string PLAIN_NOTIFICATION_TYPE = "Plain";

        public async Task<bool> HasNewNotifications()
        {
            IQuery notificationsPulseQuery = await App.DI.Resolve<NotificationPulseQueryFactory>().HasNewNotifications(App.DI.Resolve<Session>().CurrentUser);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(notificationsPulseQuery);
            return App.DI.Resolve<IResponseParser>().Parse<BoolResult, ResponseException>(response).result;
        }

        public async Task<List<RawNotification>> GetNotifications()
        {
            IQuery getNotificationsQuery = await App.DI.Resolve<GetUserNotificationsQueryFactory>().GetUserNotifications(App.DI.Resolve<Session>().CurrentUser);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getNotificationsQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<RawNotification, ResponseException>(response).ToList();
        }

        public async Task<RawNotification> SeeNotification(INotification notification)
        {
            IQuery seeNotificationQuery = await App.DI.Resolve<SeeNotificationQueryFactory>().SeeNotification(notification);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(seeNotificationQuery);
            return App.DI.Resolve<IResponseParser>().Parse<RawNotification, ResponseException>(response);
        }

        public async Task<Tour> AcceptOffer(INotification notification)
        {
            IQuery acceptOfferQuery = await App.DI.Resolve<AcceptNotificationQueryFactory>().AcceptNotification(notification);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(acceptOfferQuery);
            return App.DI.Resolve<IResponseParser>().Parse<Tour, ResponseException>(response);
        }

        public async Task<RawNotification> RefuseOffer(INotification notification)
        {
            IQuery refuseOfferQuery = await App.DI.Resolve<RefuseNotificationQueryFactory>().RefuseNotification(notification);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(refuseOfferQuery);
            return App.DI.Resolve<IResponseParser>().Parse<RawNotification, ResponseException>(response);
        }

        public async Task<RawNotification> DeleteNotification(INotification notification)
        {
            IQuery deleteNotificationQuery = await App.DI.Resolve<DeleteNotificationQuryFactory>().DeleteNotification(notification);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(deleteNotificationQuery);
            return App.DI.Resolve<IResponseParser>().Parse<RawNotification, ResponseException>(response);
        }

        public async Task<INotification> ParseRawNotification(RawNotification rawNotification)
        {
            switch(rawNotification.type)
            {
                case GUIDING_OFFER_NOTIFICATION_TYPE:
                    return await ParseGuidingOfferNotification(rawNotification);
                case GUIDE_PROPOSITION_NOTICATION_TYPE:
                    return await ParseGuideToAdminPropositionNotification(rawNotification);
                case CUSTOM_GUIDE_PROPOSITION_NOTICATION_TYPE:
                    return await ParseCustomGuidePropositionNotification(rawNotification);
                case PLAIN_NOTIFICATION_TYPE:
                default:
                    return new NewsNotification(rawNotification.id, rawNotification.isChecked, rawNotification.topic);
            }
        }

        private async Task<GuidingOfferNotification> ParseGuidingOfferNotification(RawNotification rawNotification)
        {
            Tour tour = await App.DI.Resolve<TourController>().GetTourById(Convert.ToInt32(rawNotification.data.tourId));
            Company company = await App.DI.Resolve<CompanyController>().GetCompanyById(Convert.ToInt32(rawNotification.data.companyId));

            return new GuidingOfferNotification(rawNotification.id, rawNotification.isChecked, 
                                                rawNotification.topic, company, tour,
                                                Convert.ToInt32(rawNotification.data.guideId), 
                                                Convert.ToDouble(rawNotification.data.sum));
        }

        private async Task<GuideToAdminPropositionNotification> ParseGuideToAdminPropositionNotification(RawNotification rawNotification)
        {
            Tour tour = await App.DI.Resolve<TourController>().GetTourById(Convert.ToInt32(rawNotification.data.tourId));
            Guide guide = await App.DI.Resolve<GuideController>().GetById(Convert.ToInt32(rawNotification.data.guideId));

            return new GuideToAdminPropositionNotification(rawNotification.id, rawNotification.isChecked,
                                                           rawNotification.topic, tour, guide,
                                                           Convert.ToInt32(rawNotification.data.sum));
        }

        private async Task<CustomGuidePropositionNotification> ParseCustomGuidePropositionNotification(RawNotification rawNotification)
        {
            Tour tour = await App.DI.Resolve<TourController>().GetTourById(Convert.ToInt32(rawNotification.data.tourId));
            Guide guide = await App.DI.Resolve<GuideController>().GetById(Convert.ToInt32(rawNotification.data.guideId));

            return new CustomGuidePropositionNotification(rawNotification.id, rawNotification.isChecked,
                                                          rawNotification.topic, tour, guide,
                                                          Convert.ToInt32(rawNotification.data.sum));
        }
    }
}

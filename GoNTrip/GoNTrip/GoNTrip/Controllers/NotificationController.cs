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
        private const string GUIDING_OFFER_NOTIFICATION_TYPE = "OfferGuiding";

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

        public async Task AcceptOffer(INotification notification)
        {
            IQuery acceptOfferQuery = await App.DI.Resolve<AcceptNotificationQueryFactory>().AcceptNotification(notification);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(acceptOfferQuery);
            //someth to parse
        }

        public async Task RefuseOffer(INotification notification)
        {

        }

        public async Task<INotification> ParseRawNotification(RawNotification rawNotification)
        {
            switch(rawNotification.type)
            {
                case GUIDING_OFFER_NOTIFICATION_TYPE:
                    return await ParseGuidingOfferNotification(rawNotification);
                default:
                    return new NewsNotification(rawNotification.id, rawNotification.isChecked, rawNotification.topic);
            }
        }

        private async Task<GuidingOfferNotification> ParseGuidingOfferNotification(RawNotification rawNotification)
        {
            Tour tour = await App.DI.Resolve<GetToursController>().GetTourById(Convert.ToInt32(rawNotification.data.tourId));
            long guideId = rawNotification.data.guideId;
            //consider company and sum

            return new GuidingOfferNotification(rawNotification.id, rawNotification.isChecked, 
                                                rawNotification.topic, new Company(), tour, 
                                                guideId, 0);
        }
    }
}

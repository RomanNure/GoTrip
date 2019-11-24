using System.Threading.Tasks;

using Autofac;

using Newtonsoft.Json;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;

namespace GoNTrip.Controllers
{
    public class PayController
    {
        private const string ERROR_STATUS = "error";
        private const string FAILURE_STATUS = "failure";
        private const string REVERSED_STATUS = "reversed";
        private const string SECCESS_STATUS = "success";

        private const string _3DS_VERIFY_STATUS = "3ds_verify";
        private const string CVV_VERIFY_STATUS = "cvv_verify";
        private const string OTP_VERIFY_STATUS = "otp_verify";
        private const string RECIEVER_VERIFY_STATUS = "receiver_verify";
        private const string SENDER_VERIFY_STATUS = "sender_verify";

        private const string WAIT_ACCEPT_STATUS = "wait_accept";
        private const string WAIT_LC_STATUS = "wait_lc";
        private const string WAIT_SECIRE_STATUS = "wait_secure";

        private const string LIQPAY_SERVER = "https://www.liqpay.ua/";
        private static readonly string SERVER_CALLBACK = App.DI.Resolve<IServerCommunicator>().ServerURL + "/participating/add/liqpay";

        public LiqpayPayment CreatePayment(Tour tour, Card card) =>
            new LiqpayPayment(App.DI.Resolve<Session>(), tour, card, SERVER_CALLBACK);

        public async Task<LiqpayResponse> PayForTour(LiqpayPayment payment)
        {   
            IQuery payQuery = await App.DI.Resolve<PayQueryFactory>().Pay(payment);

            IServerCommunicator server = App.DI.Resolve<IServerCommunicator>();

            string temp = server.ServerURL;
            server.ServerURL = LIQPAY_SERVER;

            IServerResponse response = await server.SendQuery(payQuery);
            server.ServerURL = temp;

            LiqpayResponse paymentResponse = JsonConvert.DeserializeObject<LiqpayResponse>(response.Data);

            switch(paymentResponse.Status)
            {
                //case ERROR_STATUS 
                //throw ResponseExceptions 
            }

            return null;
        }
    }
}

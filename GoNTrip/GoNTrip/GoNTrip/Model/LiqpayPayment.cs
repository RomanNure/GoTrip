using System;
using System.Text;
using System.Security.Cryptography;

using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiqpayPayment : ModelElement
    {
        private const string PUBLIC_KEY = "sandbox_i74310151520";
        private const string PRIVATE_KEY = "sandbox_kgwzzF9TsmUOIJmQQeQyM4G4yrxfGJxVq64k8hLn";

        private const string ACTION = "paylc";
        private const int VERSION = 3;
        private const string CURRENCY = "USD";

        private static readonly SHA1 Sha1 = SHA1.Create();
        private static readonly MD5 mD5 = MD5.Create();

        [JsonProperty("action")]
        private string Action { get; set; }

        [JsonProperty("version")]
        private int Version { get; set; }

        [JsonProperty("public_key")]
        private string PublicKey { get; set; }

        [JsonProperty("phone")]
        private string Phone { get; set; }

        [JsonProperty("amount")]
        private double Amount { get; set; }

        [JsonProperty("currency")]
        private string Currency { get; set; }

        [JsonProperty("description")]
        private string Description { get; set; }

        [JoinPrepareField("orderId")]
        [JsonProperty("order_id")]
        private string OrderId { get; set; }

        [JsonProperty("card")]
        private string Card { get; set; }

        [JsonProperty("card_exp_month")]
        private string CardExpMonth { get; set; }

        [JsonProperty("card_exp_year")]
        private string CardExpYear { get; set; }

        [JsonProperty("card_cvv")]
        private string CardCvv { get; set; }

        [JsonProperty("server_url")]
        private string ServerUrl { get; set; }

        [PayField("data")]
        [JsonIgnore]
        public string Base64 { get; private set; }

        [PayField("signature")]
        [JsonIgnore]
        public string Signature { get; private set; }

        public LiqpayPayment(Session session, Tour tour, Card card, string serverCallback)
        {
            Action = ACTION;
            Version = VERSION;
            PublicKey = PUBLIC_KEY;
            Phone = session.CurrentUser.phone;
            Amount = tour.pricePerPerson;
            Currency = CURRENCY;
            Description = "Participating in " + tour.name;
            OrderId = BitConverter.ToString(mD5.ComputeHash(Encoding.UTF8.GetBytes($"{session.CurrentUser.id}_{session.SessionId}_{tour.id}"))).Replace("-", "").ToUpper();
            Card = card.CardNum;
            CardExpMonth = card.MonthExp;
            CardExpYear = card.YearExp;
            CardCvv = card.Cvv;
            ServerUrl = serverCallback;

            string data = JsonConvert.SerializeObject(this);
            Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
            Signature = Convert.ToBase64String(Sha1.ComputeHash(Encoding.UTF8.GetBytes(PRIVATE_KEY + Base64 + PRIVATE_KEY)));
        }

    }
}

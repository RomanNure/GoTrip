using System;
using System.Text;
using System.Security.Cryptography;

using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiqpayPayment : IPayment, ModelElement
    {
        private const string PUBLIC_KEY = "sandbox_i74310151520";
        private const string PRIVATE_KEY = "sandbox_kgwzzF9TsmUOIJmQQeQyM4G4yrxfGJxVq64k8hLn";

        private const string ACTION = "paylc";
        private const int VERSION = 3;
        private const string CURRENCY = "USD";
        private const string SERVER_URL = "http://37.229.135.155:5000/api/server/test";

        private static readonly SHA1 Sha1 = SHA1.Create();

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

        public LiqpayPayment(User user, Tour tour, Card card)
        {
            Action = ACTION;
            Version = VERSION;
            PublicKey = PUBLIC_KEY;
            Phone = user.phone;
            Amount = tour.pricePerPerson;
            Currency = CURRENCY;
            Description = "Participating in " + tour.name;
            OrderId = tour.id + "_" + user.id;
            Card = card.CardNum;
            CardExpMonth = card.MonthExp;
            CardExpYear = card.YearExp;
            CardCvv = card.Cvv;
            ServerUrl = SERVER_URL;

            string data = JsonConvert.SerializeObject(this);
            Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
            Signature = Convert.ToBase64String(Sha1.ComputeHash(Encoding.UTF8.GetBytes(PRIVATE_KEY + Base64 + PRIVATE_KEY)));
        }

    }
}

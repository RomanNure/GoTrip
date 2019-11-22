using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Guide : ModelElement
    {
        public long id { get; private set; }

        [JsonProperty("registeredUser")]
        public User UserProfile { get; private set; }

        [AddGuideField("wantedToursKeyWords")]
        [JsonProperty("wantedToursKeyWords")]
        public string Keywords { get; private set; }

        [AddGuideField("cardNumber")]
        [JsonProperty("cardNumber")]
        public string Card { get; private set; }

        [JsonConstructor]
        public Guide(long id, User registeredUser, string wantedToursKeyWords, string cardNumber)
        {
            this.id = id;
            UserProfile = registeredUser;
            Keywords = wantedToursKeyWords;
            Card = cardNumber;
        }

        public Guide(string wantedToursKeyWords, string cardNumber)
        {
            Keywords = wantedToursKeyWords;
            Card = cardNumber;
        }
    }
}

using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class LiqpayResponse : ModelElement
    {
        public string Status { get; private set; }

        public LiqpayResponse(string status) => Status = status;
    }
}

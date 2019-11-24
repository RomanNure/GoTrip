using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Notification : ModelElement
    {
        public long id { get; set; }
        public string type { get; set; }
        public string topic { get; set; }

        [JsonProperty("checked")]
        public bool isChecked { get; set; }

        public User user { get; set; }
        public dynamic data { get; set; }

        public Notification() { }
    }
}

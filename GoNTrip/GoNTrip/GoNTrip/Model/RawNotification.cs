using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class RawNotification : ModelElement
    {
        public long id { get; set; }
        public string type { get; set; }
        public string topic { get; set; }

        [JsonProperty("checked")]
        public bool isChecked { get; set; }

        public User user { get; set; }

        public dynamic data { get; private set; }

        [JsonConstructor]
        public RawNotification(string data) => this.data = JsonConvert.DeserializeObject(data);
    }
}

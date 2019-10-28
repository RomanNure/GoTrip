using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Photo
    {
        public int id { get; set; }
        public string url { get; set; }

        public Photo() { }
    }
}

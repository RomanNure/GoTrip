using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Admin : ModelElement
    {
        public long id { get; set; }

        public Admin() { }
    }
}

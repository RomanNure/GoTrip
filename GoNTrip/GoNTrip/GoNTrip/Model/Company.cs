using System.Collections.Generic;

using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Company : ModelElement
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public string imageLink { get; set; }
        public List<Admin> administrators { get; set; }

        public override string ToString() => name;
    }
}

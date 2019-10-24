using System.Collections.Generic;

using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Company
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public List<Admin> administrators { get; set; }
    }
}

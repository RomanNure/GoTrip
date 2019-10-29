using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Admin : ModelElement
    {
        [GetUserByAdminField]
        [GetCompanyByAdminField]
        public long id { get; set; }

        public Admin() { }
    }
}

using Android.Runtime;

using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [Preserve(AllMembers = true)]
    [JsonObject(MemberSerialization.OptOut)]
    public class User : ModelElement
    {
        //TODO
        public User() { }
    }
}

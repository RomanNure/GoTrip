using Android.Runtime;

using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    [Preserve(AllMembers = true)]
    public class FilePath : ModelElement
    {
        [JsonRequired]
        public string path { get; set; }
    }
}

using Android.Runtime;

using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    [Preserve(AllMembers = true)]
    public class FilePath : ModelElement
    {
        [JsonRequired]
        public string path { get; private set; }

        public FilePath() { }
        public FilePath(string path) => this.path = path;
    }
}

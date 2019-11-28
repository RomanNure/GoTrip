using Newtonsoft.Json;

namespace GoNTrip.Model
{
    public class BoolResult : ModelElement
    {
        [JsonRequired]
        public bool result { get; private set; }
    }
}

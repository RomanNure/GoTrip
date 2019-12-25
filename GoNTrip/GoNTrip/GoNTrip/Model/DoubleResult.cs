using Newtonsoft.Json;

namespace GoNTrip.Model
{
    public class DoubleResult : ModelElement
    {
        [JsonRequired]
        public double value { get; set; }
    }
}

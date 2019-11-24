using Newtonsoft.Json;

namespace GoNTrip.Model
{
    public enum JoinStatus
    {
        PENDING,
        FAILED,
        SUCCESS
    };

    [JsonObject]
    public class JoinTourStatus : ModelElement
    {
        public JoinStatus status { get; set; }

        public JoinTourStatus() { }
    }
}

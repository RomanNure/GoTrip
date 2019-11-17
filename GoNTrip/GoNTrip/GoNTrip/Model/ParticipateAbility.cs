using Newtonsoft.Json;

namespace GoNTrip.Model
{
    public class ParticipateAbility : ModelElement
    {
        [JsonRequired]
        public bool able { get; private set; }
    }
}

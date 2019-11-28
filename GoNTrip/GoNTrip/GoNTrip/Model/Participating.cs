using Newtonsoft.Json;

namespace GoNTrip.Model
{
    public class Participating : ModelElement
    {
        public long id { get; set; }
        public int tourRate { get; set; }
        public int guideRate { get; set; }
        public string orderId { get; set; }

        [JsonConstructor]
        public Participating(long id, int tourRate, int guideRate, string orderId)
        {
            this.id = id;
            this.tourRate = tourRate;
            this.guideRate = guideRate;
            this.orderId = orderId;
        }
    }
}

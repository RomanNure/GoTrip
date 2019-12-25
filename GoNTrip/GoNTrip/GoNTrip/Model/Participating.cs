using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    public class Participating : ModelElement
    {
        public long id { get; set; }

        [FinishTour]
        public int tourRate { get; set; }

        [FinishTour]
        public int guideRate { get; set; }

        public string orderId { get; set; }
        public bool participated { get; set; }
        public bool finished { get; set; }

        [JsonConstructor]
        public Participating(long id, int tourRate, int guideRate, string orderId, bool participated, bool finished)
        {
            this.id = id;
            this.tourRate = tourRate;
            this.guideRate = guideRate;
            this.orderId = orderId;
            this.participated = participated;
            this.finished = finished;
        }

        public Participating(int tourRate, int guideRate)
        {
            this.tourRate = tourRate;
            this.guideRate = guideRate;
        }
    }
}

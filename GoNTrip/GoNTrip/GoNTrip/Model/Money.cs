using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    public class Money : ModelElement
    {
        [OfferGuidingField("sum")]
        public double Sum { get; private set; }

        public Money(double sum) => Sum = sum;
    }
}

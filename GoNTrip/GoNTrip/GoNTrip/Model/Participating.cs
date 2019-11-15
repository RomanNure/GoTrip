namespace GoNTrip.Model
{
    public class Participating : ModelElement
    {
        public long id { get; set; }
        public int tourRate { get; set; }
        public int guideRate { get; set; }
        public string hash { get; set; }

        public Participating() { }

        public Participating(long id, int tourRate, int guideRate, string hash)
        {
            this.id = id;
            this.tourRate = tourRate;
            this.guideRate = guideRate;
            this.hash = hash;
        }
    }
}

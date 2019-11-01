using Newtonsoft.Json;

namespace GoNTrip.Model.FilterSortSearch
{
    [JsonObject]
    public abstract class Filter
    {
        public string name { get; private set; }
        public object from { get; private set; } 
        public object to { get; private set; }

        public Filter(string name, object from, object to)
        {
            this.name = name;
            this.from = from;
            this.to = to;
        }
    }
}

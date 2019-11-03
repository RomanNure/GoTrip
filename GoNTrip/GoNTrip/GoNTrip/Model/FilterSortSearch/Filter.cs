using Newtonsoft.Json;
using System;

namespace GoNTrip.Model.FilterSortSearch
{
    [JsonObject]
    public class Filter<T>
    { 
        [JsonIgnore]
        public bool IsChanged { get => !from.Equals(min) || !to.Equals(max); }

        [JsonIgnore]
        private T min { get; set; }

        [JsonIgnore]
        private T max { get; set; }

        public T from { get; set; }
        public T to { get; set; }

        public Filter(T min, T max)
        {
            this.min = min;
            this.max = max;

            this.from = min;
            this.to = max;
        }
    }
}

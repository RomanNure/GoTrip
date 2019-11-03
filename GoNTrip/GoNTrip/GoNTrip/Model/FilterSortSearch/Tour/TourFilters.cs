using System;

using Newtonsoft.Json;

namespace GoNTrip.Model.FilterSortSearch.Tour
{
    [JsonObject]
    public class TourFilters
    {
        public Filter<Double> priceFilter { get; private set; }
        public Filter<DateTime> startDateFilter { get; private set; }
        public Filter<Int32> participantsFilter { get; private set; }

        public TourFilters()
        {
            priceFilter = new Filter<double>(double.MinValue, double.MaxValue);
            startDateFilter = new Filter<DateTime>(DateTime.MinValue, DateTime.MaxValue);
            participantsFilter = new Filter<int>(int.MinValue, int.MaxValue);
        }

        public void Reset()
        {
            priceFilter.Reset();
            startDateFilter.Reset();
            participantsFilter.Reset();
        }

        [JsonIgnore]
        public bool IsChanged => priceFilter.IsChanged || startDateFilter.IsChanged || participantsFilter.IsChanged;
    }
}

using System;

using Newtonsoft.Json;

namespace GoNTrip.Model.FilterSortSearch.Tour
{
    [JsonObject]
    public class TourFilters
    {
        public DualFilter<Double> priceFilter { get; private set; }
        public DualFilter<DateTime> startDateFilter { get; private set; }
        public DualFilter<Int32> participantsFilter { get; private set; }

        public TourFilters()
        {
            priceFilter = new DualFilter<double>(double.MinValue, double.MaxValue);
            startDateFilter = new DualFilter<DateTime>(DateTime.MinValue, DateTime.MaxValue);
            participantsFilter = new DualFilter<int>(int.MinValue, int.MaxValue);
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

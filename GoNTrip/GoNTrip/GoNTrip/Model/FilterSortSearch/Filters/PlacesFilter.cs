using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Model.FilterSortSearch.Filters
{
    public class PlacesFilter : Filter
    {
        private const string NAME = "participants";
        public PlacesFilter(int from, int to) : base(NAME, from, to) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Model.FilterSortSearch.Filters
{
    public class PriceFilter : Filter
    {
        private const string NAME = "price";
        public PriceFilter(double from, double to) : base(NAME, from, to) { }
    }
}

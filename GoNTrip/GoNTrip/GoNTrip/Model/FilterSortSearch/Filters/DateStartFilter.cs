using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Model.FilterSortSearch.Filters
{
    public class DateStartFilter : Filter
    {
        private const string NAME = "start";
        public DateStartFilter(DateTime from, DateTime to) : base(NAME, from, to) { }
    }
}

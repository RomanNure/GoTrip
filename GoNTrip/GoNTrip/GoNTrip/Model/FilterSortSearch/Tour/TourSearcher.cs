namespace GoNTrip.Model.FilterSortSearch.Tour
{
    public class TourSearcher : ISearcher
    {
        public string tourNameSubstr { get; set; }
        public string tourLocationSubstr { get; set; }

        public TourSearcher(string nameSubstr = "", string locationSubstr = "")
        {
            tourNameSubstr = nameSubstr;
            tourLocationSubstr = locationSubstr;
        }

        public bool IsEmpty() => tourNameSubstr == "" && tourLocationSubstr == "";
    }
}

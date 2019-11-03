using System.Runtime.Serialization;

namespace GoNTrip.Model.FilterSortSearch.Tour
{
    public enum TourSortCriteria
    {
        [EnumMember(Value = "price")]
        price,

        [EnumMember(Value = "free_places")]
        free_places,

        [EnumMember(Value = "no")]
        no
    };
}

using System.Threading.Tasks;

namespace GoNTrip.Pages.Additional.Controls
{
    public class TourListItemFactory
    {
        public async Task<TourListItem> CreateTourListItem() => await Task.Run(() => new TourListItem());
    }
}

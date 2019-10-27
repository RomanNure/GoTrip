using System.Threading.Tasks;

using GoNTrip.Model;

namespace GoNTrip.Pages.Additional.Controls
{
    public class TourListItemFactory
    {
        public async Task<TourListItem> CreateTourListItem() => await Task.Run(() => new TourListItem());
    }
}

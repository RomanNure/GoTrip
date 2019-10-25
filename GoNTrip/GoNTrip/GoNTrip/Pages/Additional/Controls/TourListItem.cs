using Xamarin.Forms;

using GoNTrip.Model;

namespace GoNTrip.Pages.Additional.Controls
{
    public class TourListItem : Grid
    {
        public TourListItem(Tour tour)
        {
            Label tourNameLabel = new Label();
            tourNameLabel.Text = tour.name;

            Children.Add(tourNameLabel, 0, 0);
        }
    }
}

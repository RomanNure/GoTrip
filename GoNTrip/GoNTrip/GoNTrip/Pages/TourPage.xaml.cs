using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourPage : ContentPage
    {
        public TourPage(Tour tour)
        {
            InitializeComponent();
        }
    }
}
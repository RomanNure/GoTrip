using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }

        private int TourListStartNum { get; set; }
        private Tour CurrentTour { get; set; }

        public TourPage(Tour tour, int tourListStartNum)
        {
            TourListStartNum = tourListStartNum;
            CurrentTour = tour;

            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            DisplayAlert("info", CurrentTour.name, "OK");
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
                App.Current.MainPage = new TourListPage(TourListStartNum);
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
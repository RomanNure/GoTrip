using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }

        private TourListPageMemento TourListPageMemento { get; set; }
        private Tour CurrentTour { get; set; }

        public TourPage(Tour tour, TourListPageMemento memento)
        {
            TourListPageMemento = memento;
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
                App.Current.MainPage = new TourListPage(TourListPageMemento);
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
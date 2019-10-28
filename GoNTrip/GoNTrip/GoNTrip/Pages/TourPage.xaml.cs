using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.Pages.Additional.Popups.Templates;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }
        private SwipablePhotoCollection PhotoCollection { get; set; }

        private TourListPageMemento TourListPageMemento { get; set; }
        private Tour CurrentTour { get; set; }

        public TourPage(Tour tour, TourListPageMemento memento)
        {
            TourListPageMemento = memento;
            CurrentTour = tour;

            InitializeComponent();

            TourMainImagePreview.WidthRequest = this.Width;
            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            PhotoCollection = new SwipablePhotoCollection(PopupControl);

            PhotoCollection.Add(TourMainImage);
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);
            await LoadCurrentTour();
            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        private async Task LoadCurrentTour()
        {
            await Task.Run(() => Thread.Sleep(100));
        }

        private bool TourMainImagePreview_OnClick(Android.Views.MotionEvent ME, CustomControls.IClickable sender)
        {
            PhotoCollection.MoveNext();
            return false;
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0)
                App.Current.MainPage = new TourListPage(TourListPageMemento);
            else if (PhotoCollection.Opened)
                PhotoCollection.Reset();
            else
                PopupControl.CloseTopPopup();

            return true;
        }
    }
}
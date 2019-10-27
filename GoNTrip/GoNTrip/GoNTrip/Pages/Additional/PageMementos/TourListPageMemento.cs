namespace GoNTrip.Pages.Additional.PageMementos
{
    public class TourListPageMemento
    {
        public TourListPage TourListPage { get; private set; }
        public TourListPageMemento(TourListPage tourListPage) => TourListPage = tourListPage;
    }
}

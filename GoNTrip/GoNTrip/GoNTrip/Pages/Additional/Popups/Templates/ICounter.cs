namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public interface ICounter
    {
        int Max { get; set; }
        bool MoveNext();
        bool MovePrev();
    }
}

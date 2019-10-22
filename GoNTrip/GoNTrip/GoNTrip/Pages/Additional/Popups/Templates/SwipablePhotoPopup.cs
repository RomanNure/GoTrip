namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class SwipablePhotoPopup : MovablePhotoPopup
    {
        public SwipablePhotoPopup() : base() => base.OnTopBorderExceeded += (sender) => this.ForceHide();
    }
}

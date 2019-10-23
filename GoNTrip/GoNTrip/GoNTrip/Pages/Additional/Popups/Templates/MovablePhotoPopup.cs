using Android.Util;
using Android.Views;
using CustomControls;
using Xamarin.Essentials;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class MovablePhotoPopup : SignedPhotoPopup
    {
        public delegate void OnBorderExceeded(MovablePhotoPopup popup);

        public event OnBorderExceeded OnTopBorderExceeded;
        public event OnBorderExceeded OnBotBorderExceeded;
        public event OnBorderExceeded OnLeftBorderExceeded;
        public event OnBorderExceeded OnRightBorderExceeded;

        public float? XTranslationBorder { get; set; }
        public float? YTranslationBorder { get; set; }

        public MovablePhotoPopup() : base() => base.Image.OnClick += (ME, sender) => Move(ME, sender);

        protected (float x, float y) startMovePoint = (0, 0);
        protected bool Move(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
            {
                startMovePoint = (ME.RawX, ME.RawY);
                return false;
            }

            Img img = sender as Img;

            (float x, float y) translation = GetNewTranslation(ME, startMovePoint, (float)DeviceDisplay.MainDisplayInfo.Density);
            img.TranslationX = translation.x;
            img.TranslationY = translation.y;

            double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

            if (XTranslationBorder != null && width - img.TranslationX < XTranslationBorder)
                OnRightBorderExceeded?.Invoke(this);
            else if (XTranslationBorder != null && width + img.TranslationX < XTranslationBorder)
                OnLeftBorderExceeded?.Invoke(this);
            else if (YTranslationBorder != null && height + img.TranslationY < YTranslationBorder)
                OnTopBorderExceeded?.Invoke(this);
            else if (YTranslationBorder != null && height - img.TranslationY < YTranslationBorder)
                OnBotBorderExceeded?.Invoke(this);

            return false;
        }

        protected virtual (float x, float y) GetNewTranslation(MotionEvent ME, (float x, float y) start, float density)
        {
            if (ME.Action == MotionEventActions.Up)
                return (0, 0);

            return ((ME.RawX - start.x) / density, (ME.RawY - start.y) / density);
        }
    }
}

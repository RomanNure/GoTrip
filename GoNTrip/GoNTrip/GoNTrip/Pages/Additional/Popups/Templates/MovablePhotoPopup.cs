using Android.Util;
using Android.Views;
using CustomControls;
using Xamarin.Essentials;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class MovablePhotoPopup : PhotoPopup
    {
        public delegate void OnBorderExceeded(MovablePhotoPopup popup);

        public event OnBorderExceeded OnTopBorderExceeded;
        public event OnBorderExceeded OnBotBorderExceeded;
        public event OnBorderExceeded OnLeftBorderExceeded;
        public event OnBorderExceeded OnRightBorderExceeded;

        public float? XTranslationBorder { get; set; }
        public float? YTranslationBorder { get; set; }

        public MovablePhotoPopup() : base() => base.Image.OnClick += Popup_OnClick;

        protected (float x, float y) startMovePoint = (0, 0);

        private bool Popup_OnClick(MotionEvent ME, IClickable sender)
        {
            Img img = sender as Img;

            if (ME.Action == MotionEventActions.Down)
            {
                startMovePoint = (ME.RawX, ME.RawY);
                return false;
            }

            if (ME.Action == MotionEventActions.Up)
            {
                img.TranslationX = 0;
                img.TranslationY = 0;
            }
            else
            {
                img.TranslationX = (ME.RawX - startMovePoint.x) / DeviceDisplay.MainDisplayInfo.Density;
                img.TranslationY = (ME.RawY - startMovePoint.y) / DeviceDisplay.MainDisplayInfo.Density;
            }

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
    }
}

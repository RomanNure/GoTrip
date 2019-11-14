using Android.Util;
using Android.Views;
using CustomControls;
using Xamarin.Essentials;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class MovablePhotoPopup : SignedPhotoPopup
    {
        public delegate void Moved(MotionEvent ME, MovablePhotoPopup popup);
        public event Moved OnMove;

        public bool TopBorderExceeded { get; private set; }
        public bool BotBorderExceeded { get; private set; }
        public bool LeftBorderExceeded { get; private set; }
        public bool RightBorderExceeded { get; private set; }

        public float? XTranslationBorder { get; set; }
        public float? YTranslationBorder { get; set; }

        public MovablePhotoPopup() : base() => base.Image.OnClick += (ME, sender) => Move(ME, sender);

        protected (float x, float y) startMovePoint = (0, 0);
        protected bool Move(MotionEvent ME, IClickable sender)
        {
            if (ME.Action == MotionEventActions.Down)
            {
                startMovePoint = (ME.RawX, ME.RawY);

                LeftBorderExceeded = false;
                TopBorderExceeded = false;
                RightBorderExceeded = false;
                BotBorderExceeded = false;
                
                return false;
            }

            Img img = sender as Img;

            (float x, float y) translation = GetNewTranslation(ME, startMovePoint, (float)DeviceDisplay.MainDisplayInfo.Density);

            OnMove?.Invoke(ME, this);

            img.TranslationX = translation.x;
            img.TranslationY = translation.y;

            double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

            RightBorderExceeded = XTranslationBorder != null && img.X + img.Width + img.TranslationX > width - XTranslationBorder;
            LeftBorderExceeded = XTranslationBorder != null && img.X + img.TranslationX < XTranslationBorder;
            TopBorderExceeded = YTranslationBorder != null && img.Y + img.TranslationY < YTranslationBorder;
            BotBorderExceeded = YTranslationBorder != null && img.Y + img.Height + img.TranslationY > height - YTranslationBorder;

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

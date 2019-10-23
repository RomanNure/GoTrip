using Android.Views;
using System;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class SwipablePhotoPopup : MovablePhotoPopup
    {
        public SwipablePhotoPopup() : base()
        {
            base.OnTopBorderExceeded += (sender) => this.ForceHide();
            base.OnBotBorderExceeded += (sender) => this.ForceHide();
        }

        private bool isDX = false;
        private bool isDY = false;

        protected override (float x, float y) GetNewTranslation(MotionEvent ME, (float x, float y) start, float density)
        {
            if (ME.Action == MotionEventActions.Up)
            {
                isDX = false;
                isDY = false;
                return (0, 0);
            }

            if (isDY || isDX)
                return CalcTransition(ME, start, density);

            isDX = Math.Abs(ME.RawX - start.x) > Math.Abs(ME.RawY - start.y);
            isDY = !isDX;

            return CalcTransition(ME, start, density);
        }

        private (float x, float y) CalcTransition(MotionEvent ME, (float x, float y) start, float density)
        {
            if (isDY)
                return (0, (ME.RawY - start.y) / density);

            if(isDX)
                return ((ME.RawX - start.x) / density, 0);

            return (0, 0);
        }
    }
}

using System;

using Android.Views;

using Xamarin.Essentials;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class SwipablePhotoPopup : MovablePhotoPopup
    {
        public void LinkControlSystem(PopupControlSystem popupControl) => base.OnMove += (ME, sender) => ProcessMoving(popupControl, ME, sender);

        public void ProcessMoving(PopupControlSystem popupControl, MotionEvent ME, MovablePhotoPopup sender)
        {
            if (TopBorderExceeded || BotBorderExceeded)
            {
                if (ME.Action == MotionEventActions.Up)
                {
                    Image.Scale = 1;

                    if(sender.Opened)
                        popupControl.CloseTopPopupAndHideKeyboardIfNeeded();
                }
                else
                {
                    double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
                    double outerTail = Math.Abs(TopBorderExceeded ? YTranslationBorder.Value - Image.Y - Image.TranslationY : Image.Y + Image.Height + Image.TranslationY - height + YTranslationBorder.Value);
                    Image.Scale = (1 - outerTail / Image.Height);
                }
            }
            else
                Image.Scale = 1;
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

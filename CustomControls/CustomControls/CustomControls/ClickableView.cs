using System;
using Android.Views;
using Xamarin.Forms;

namespace CustomControls
{
    public class ClickableView : BoxView, IClickable
    {
        public delegate void Changed(ClickableView sender);
        public event Clicked OnClick;

        private bool clickAnimation = true;
        public bool ClickAnimation { get { return clickAnimation; } set { clickAnimation = value; } }

        private float scaleOnClicked = 1;
        public float ScaleOnClicked { get { return scaleOnClicked; } set { scaleOnClicked = value; } }

        private uint clickAnimationDuration = 50;
        public uint ClickAnimationDuration { get { return clickAnimationDuration; } set { clickAnimationDuration = Math.Max(0, value); } }

        public bool Click(MotionEvent ME)
        {
            if (OnClick == null)
                return false;

            return OnClick(ME, this);
        }

        protected void IfChangedCall(Object oldV, Object newV, Changed eventToCall)
        {
            bool call = !oldV.Equals(newV);
            if (call) eventToCall?.Invoke(this);
        }
    }
}

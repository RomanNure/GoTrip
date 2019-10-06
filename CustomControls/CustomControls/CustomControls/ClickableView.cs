using System;
using Android.Views;
using Xamarin.Forms;

namespace CustomControls
{
    public class ClickableView : BoxView
    {
        public delegate void Changed(ClickableView sender);
        public delegate bool MEClicked(MotionEvent ME, ClickableView sender);

        public event MEClicked MEClick;

        public bool Click(MotionEvent ME)
        {
            if (MEClick == null)
                return false;

            return MEClick(ME, this);
        }

        protected void IfChangedCall(Object oldV, Object newV, Changed eventToCall)
        {
            bool call = !oldV.Equals(newV);
            if (call) eventToCall?.Invoke(this);
        }
    }
}

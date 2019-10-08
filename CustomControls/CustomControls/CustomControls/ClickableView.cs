using System;
using Android.Views;
using Xamarin.Forms;

namespace CustomControls
{
    public class ClickableView : BoxView, IClickable
    {
        public delegate void Changed(ClickableView sender);
        public event Clicked OnClick;

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

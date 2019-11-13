using Android.Views;

using Xamarin.Forms;

namespace CustomControls
{
    public class ClickableLabel : Label, IClickable
    {
        public event Clicked OnClick;

        public bool Click(MotionEvent ME)
        {
            if (OnClick == null)
                return false;

            return OnClick(ME, this);
        }
    }
}

using Android.Views;
using Xamarin.Forms;

namespace CustomControls
{
    public abstract class ClickableView : BoxView
    {
        public delegate void Changed();
        public delegate bool MEClicked(MotionEvent ME);

        public event MEClicked MEClick;

        public bool Click(MotionEvent ME)
        {
            if (MEClick == null)
                return false;

            return MEClick(ME);
        }
    }
}

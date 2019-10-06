using Android.Views;
using Xamarin.Forms;

namespace CustomControls
{
    public class ClickableStackLayout : StackLayout
    {
        public delegate bool Clicked(MotionEvent ME, ClickableStackLayout sender);
        public event Clicked OnClick;

        public bool Click(MotionEvent ME)
        {
            if (OnClick == null)
                return false;

            return OnClick.Invoke(ME, this);
        }
    }
}

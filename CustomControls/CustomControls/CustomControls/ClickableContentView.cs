using Android.Views;
using Xamarin.Forms;

namespace CustomControls
{
    public class ClickableContentView : ContentView, IClickable
    {
        public event Clicked OnClick;

        public bool Click(MotionEvent ME)
        {
            if (OnClick == null)
                return false;

            return OnClick.Invoke(ME, this);
        }
    }
}

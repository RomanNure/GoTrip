using Android.Views;
using Xamarin.Forms;

namespace CustomControls
{
    public class ClickableContentView : ContentView
    {
        public delegate bool Clicked(MotionEvent ME, ClickableContentView sender);
        public event Clicked OnClick;

        public bool Click(MotionEvent ME)
        {
            if (OnClick == null)
                return false;

            return OnClick.Invoke(ME, this);
        }
    }
}

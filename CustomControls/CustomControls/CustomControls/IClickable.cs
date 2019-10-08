using Android.Views;

namespace CustomControls
{
    public delegate bool Clicked(MotionEvent ME, IClickable sender);

    public interface IClickable
    {
        bool Click(MotionEvent ME);
        event Clicked OnClick;
    }
}

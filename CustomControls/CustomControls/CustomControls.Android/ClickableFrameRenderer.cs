using Android.Content;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClickableFrame), typeof(ClickableFrameRenderer))]
namespace CustomControls.Droid
{
    public class ClickableFrameRenderer : FrameRenderer
    {
        private ClickableFrame Target;

        public ClickableFrameRenderer(Context ctx) : base(ctx) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            Target = e.NewElement as ClickableFrame;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            Target.Click(e);
            return true;
        }
    }
}
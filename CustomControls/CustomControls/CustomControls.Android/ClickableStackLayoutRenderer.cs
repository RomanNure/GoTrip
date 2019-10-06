using Android.Content;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClickableStackLayout), typeof(ClickableStackLayoutRenderer))]
namespace CustomControls.Droid
{
    public class ClickableStackLayoutRenderer : VisualElementRenderer<StackLayout>
    {
        private ClickableStackLayout Target;

        public ClickableStackLayoutRenderer(Context ctx) : base(ctx) { }
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);
            Target = e.NewElement as ClickableStackLayout;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            Target.Click(e);
            return true;
        }
    }
}
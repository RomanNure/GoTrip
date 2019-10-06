using Android.Content;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClickableContentView), typeof(ClickableContentViewRenderer))]
namespace CustomControls.Droid
{
    public class ClickableContentViewRenderer : VisualElementRenderer<ContentView>
    {
        private ClickableContentView Target;

        public ClickableContentViewRenderer(Context ctx) : base(ctx) { }
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);
            Target = e.NewElement as ClickableContentView;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            Target.Click(e);
            return true;
        }
    }
}
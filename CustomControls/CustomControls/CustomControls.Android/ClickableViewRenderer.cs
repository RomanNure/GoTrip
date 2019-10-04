using Android.Content;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClickableView), typeof(ClickableViewRenderer))]
namespace CustomControls.Droid
{
    public class ClickableViewRenderer : BoxRenderer
    {
        protected Android.Graphics.Color ColorConvert(Xamarin.Forms.Color C) => new Android.Graphics.Color((byte)(C.R * 255), (byte)(C.G * 255), (byte)(C.B * 255), (byte)C.A * 255);
        protected float Density => Resources.DisplayMetrics.Density;

        protected ClickableView Target { get; private set; }
        protected bool Clicked = false;

        public ClickableViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            Target = e.NewElement as ClickableView;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (Target.Click(e))
                return true;

            Clicked = e.Action != MotionEventActions.Up;
            return true;
        }
    }
}
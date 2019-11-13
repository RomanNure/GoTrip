using Android.Views;
using Android.Content;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using CustomControls;
using CustomControls.Droid;

[assembly : ExportRenderer(typeof(ClickableLabel), typeof(ClickableLabelRenderer))]
namespace CustomControls.Droid
{
    public class ClickableLabelRenderer : LabelRenderer
    {
        private ClickableLabel TargetLabel { get; set; }

        public ClickableLabelRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            TargetLabel = e.NewElement as ClickableLabel;
        }

        public override bool OnTouchEvent(MotionEvent e) => TargetLabel.Click(e);
    }
}
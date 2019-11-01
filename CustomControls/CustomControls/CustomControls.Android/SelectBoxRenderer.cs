using Android.Content;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SelectBox), typeof(SelectBoxRenderer))]
namespace CustomControls.Droid
{
    public class SelectBoxRenderer : ClickableViewRenderer
    {
        private SelectBox TargetSelect;

        public SelectBoxRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);
            TargetSelect = e.NewElement as SelectBox;
            TargetSelect.CheckedChanged += (sender) => PostInvalidate();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (base.OnTouchEvent(e))
                return true;

            if (e.Action == MotionEventActions.Down)
                TargetSelect.Checked = !TargetSelect.Checked;

            //PostInvalidate();

            return false;
        }
    }
}
using System;
using System.Threading;

using Android.Content;
using Android.Graphics;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RadioButton), typeof(RadioButtonRenderer))]
namespace CustomControls.Droid
{
    public class RadioButtonRenderer : SelectBoxRenderer
    {
        private RadioButton TargetRadioButton;
        private Paint OuterP = new Paint();
        private Paint InnerP = new Paint();

        public RadioButtonRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);

            OuterP.SetStyle(Paint.Style.Stroke);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            TargetRadioButton = Target as RadioButton;

            TargetRadioButton.OnColorChanged += UpdatePaints;
            UpdatePaints();
        }

        private void UpdatePaints(ClickableView sender = null)
        {
            OuterP.StrokeWidth = TargetRadioButton.OuterWidth * Density;
            OuterP.Color = ColorConvert(TargetRadioButton.OuterColor);

            InnerP.Color = ColorConvert(TargetRadioButton.InnerColor);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int size = Math.Min(canvas.Width, canvas.Height);

            float cx = canvas.Width / 2.0f;
            float cy = canvas.Height / 2.0f;

            canvas.DrawCircle(cx, cy, size / 2, OuterP);

            if (TargetRadioButton.Checked)
                canvas.DrawCircle(cx, cy, TargetRadioButton.InnerRadius * Density, InnerP);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            PostInvalidate();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            base.OnTouchEvent(e);
            return true;
        }
    }
}
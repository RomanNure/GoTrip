using System;

using Android.Views;
using Android.Content;
using Android.Graphics;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]
namespace CustomControls.Droid
{
    public class CheckBoxRenderer : SelectBoxRenderer
    {
        private CheckBox TargetCheckBox;
        private Paint OuterP = new Paint();
        private Paint InnerP = new Paint();

        public CheckBoxRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);

            OuterP.SetStyle(Paint.Style.Stroke);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            TargetCheckBox = Target as CheckBox;

            TargetCheckBox.OnColorChanged += UpdatePaints;
            UpdatePaints();
        }

        private void UpdatePaints(ClickableView sender = null)
        {
            OuterP.StrokeWidth = TargetCheckBox.OuterWidth * Density;
            OuterP.Color = ColorConvert(TargetCheckBox.OuterColor);

            InnerP.Color = ColorConvert(TargetCheckBox.InnerColor);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int size = Math.Min(canvas.Width, canvas.Height);

            float dx = (canvas.Width - size) / 2.0f;
            float dy = (canvas.Height - size) / 2.0f;

            canvas.DrawRoundRect(dx, dy, dx + size, dy + size, TargetCheckBox.BorderRadiusX, TargetCheckBox.BorderRadiusY, OuterP);

            float differ = (TargetCheckBox.OuterWidth + TargetCheckBox.RadiusDiffer) * Density / 2;

            if (TargetCheckBox.Checked)
                canvas.DrawRoundRect(dx + differ, dy + differ, dx + size - differ, dy + size - differ, TargetCheckBox.BorderRadiusX, TargetCheckBox.BorderRadiusY, InnerP);
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
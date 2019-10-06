using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TapTrackView), typeof(TapTrackRenderer))]
namespace CustomControls.Droid
{ 
    public class TapTrackRenderer : ViewRenderer
    {
        private Paint P = new Paint();
        private const float PointRadius = 3;

        public TapTrackView TTV { get; private set; }
        public TapTrackRenderer(Context ctx) : base(ctx)
        {
            P.StrokeWidth = 3;
            P.Color = Android.Graphics.Color.Black;
            Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            e.NewElement.BackgroundColor = Xamarin.Forms.Color.Red;
            TTV = e.NewElement as TapTrackView;

            TTV.OnPop += () => PostInvalidate();
            TTV.OnClear += () => PostInvalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(Android.Graphics.Color.Red);
            foreach(List<Xamarin.Forms.Point> PG in TTV.PointGroups)
            {
                for (int i = 0; i < PG.Count - 1; i++)
                {
                    canvas.DrawCircle((float)PG[i].X, (float)PG[i].Y, PointRadius, P);
                    canvas.DrawLine((float)PG[i].X, (float)PG[i].Y, (float)PG[i + 1].X, (float)PG[i + 1].Y, P);
                }

                if (PG.Count > 0)
                    canvas.DrawCircle((float)PG.Last().X, (float)PG.Last().Y, PointRadius, P);
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
                TTV.PointGroups.Add(new List<Xamarin.Forms.Point>());

            TTV.PointGroups.Last().Add(new Xamarin.Forms.Point(e.RawX - Element.X, e.RawY - Element.Y));

            PostInvalidate();
            return true;
        }
    }
}

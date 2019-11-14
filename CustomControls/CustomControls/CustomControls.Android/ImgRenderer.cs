using System;
using System.Net;
using System.Linq;
using IO = System.IO;
using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using XF = Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: XF.ExportRenderer(typeof(Img), typeof(ImgRenderer))]
namespace CustomControls.Droid
{
    public class ImgRenderer : ClickableViewRenderer
    {
        private Img TargetImage { get; set; }
        private Bitmap BitmapData { get; set; }

        private Path[] Borders = new Path[4];
        private const float cornerAngle = -(float)Math.PI / 2.0f;
        private static readonly float[] cornerAngleStarts = new float[4] { (float)Math.PI, (float)Math.PI / 2.0f, 0, -(float)Math.PI / 2.0f };
        private static readonly float[] cornerCircleCenterCoeffs = new float[16]
        {
            0, 1, 0, 1,
            1, -1, 0, 1,
            1, -1, 1, -1,
            0,  1, 1, -1
        };

        private static readonly Paint CornerCropPaint = new Paint();

        public ImgRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);

            CornerCropPaint.Color = Color.Transparent;
            CornerCropPaint.SetStyle(Paint.Style.Fill);
            CornerCropPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XF.BoxView> e)
        {
            base.OnElementChanged(e);

            TargetImage = Target as Img;

            TargetImage.OnSourceChanged += sender => { UpdateBitmapData(); PostInvalidate(); };
            TargetImage.OnCornerRadiusChanged += sender => { UpdateBitmapData(); PostInvalidate(); };
            TargetImage.OnCornerSmoothChanged += sender => { UpdateBitmapData(); PostInvalidate(); };

            TargetImage.OnBorderColorChanged += sender => PostInvalidate();
            TargetImage.OnBorderWidthChanged += sender => PostInvalidate();
            TargetImage.OnBorderStatusChanged += sender => PostInvalidate();

            UpdateBitmapData();
            PostInvalidate();
        }

        private void UpdateBitmapData()
        {
            if (TargetImage.Source == "")
            {
                BitmapData = null;
                PostInvalidate();
                return;
            }

            try
            {
                Bitmap temp = LoadBitmap(TargetImage.Source);

                TargetImage.WidthRequest = TargetImage.WidthRequest == -1 ? temp.Width / Density : TargetImage.WidthRequest;
                TargetImage.HeightRequest = TargetImage.HeightRequest == -1 ? temp.Height / Density : TargetImage.HeightRequest;

                Bitmap tempScaled = Bitmap.CreateScaledBitmap(temp, (int)(TargetImage.WidthRequest * Density), (int)(TargetImage.HeightRequest * Density), false);
                temp.Dispose();

                BitmapData = tempScaled.Copy(Bitmap.Config.Argb8888, true);
                tempScaled.Dispose();

                UpdateCorners();
            }
            catch { BitmapData = null; }
        }

        private Bitmap LoadBitmap(string path)
        {
            try
            {
                IO.Stream str = Android.App.Application.Context.Assets.Open(path);
                return BitmapFactory.DecodeStream(str);
            }
            catch
            {
                Uri url = new Uri(TargetImage.Source);
                HttpWebRequest request = new HttpWebRequest(url);

                IO.Stream response = request.GetResponse().GetResponseStream();
                return BitmapFactory.DecodeStream(response);
            }
        }

        private PointF[] GetBorderPoints(float centerX, float centerY, float angleStart, float angle, float radius, int pointsCount)
        {
            PointF[] pts = new PointF[pointsCount];

            double angleDelta = angle / (pointsCount - 1);
            double currentAngle = angleStart;

            for (int i = 0; i < pointsCount; i++, currentAngle += angleDelta)
                pts[i] = GetCirclePoint(centerX, centerY, radius, currentAngle);

            return pts;
        }

        private Path BuildBorderPath(IEnumerable<PointF> borderPoints, Path.FillType fillType, bool close)
        {
            List<PointF> p = borderPoints.ToList();
            Path path = new Path();

            path.SetFillType(fillType);
            path.MoveTo(borderPoints.ElementAt(0).X, borderPoints.ElementAt(0).Y);

            foreach (PointF point in borderPoints)
                path.LineTo(point.X, point.Y);

            if (close) path.Close();

            return path;
        }

        private Path BuildCornerPath(IEnumerable<PointF> borderPoints, float cornerX, float cornerY)
        {
            Path path = BuildBorderPath(borderPoints, Path.FillType.EvenOdd, false);

            path.LineTo(cornerX, cornerY);
            path.LineTo(borderPoints.ElementAt(0).X, borderPoints.ElementAt(0).Y);

            path.Close();

            return path;
        }

        private PointF GetCirclePoint(float cx, float cy, float radius, double angle) =>
            new PointF(cx + radius * (float)Math.Cos(angle), cy - radius * (float)Math.Sin(angle));

        private void UpdateCorners()
        {
            if (BitmapData == null)
                return;

            Canvas canvas = new Canvas(BitmapData);
            PointF[][] borderPoints = new PointF[4][];

            for(Img.Corners i = Img.Corners.TOP_LEFT; i <= Img.Corners.BOT_LEFT; i++)
            {
                int ii = (int)i;
                int radius = (int)(TargetImage.GetCornerRadius(i) * Density);

                float px = cornerCircleCenterCoeffs[ii * Img.CORNERS_COUNT] * BitmapData.Width;
                float py = cornerCircleCenterCoeffs[ii * Img.CORNERS_COUNT + 2] * BitmapData.Height;

                float cx = px + cornerCircleCenterCoeffs[ii * Img.CORNERS_COUNT + 1] * radius;
                float cy = py + cornerCircleCenterCoeffs[ii * Img.CORNERS_COUNT + 3] * radius;

                borderPoints[ii] = GetBorderPoints(cx, cy, cornerAngleStarts[(int)i], cornerAngle, radius, TargetImage.CornersSmooth);
                canvas.DrawPath(BuildCornerPath(borderPoints[ii], px, py), CornerCropPaint);
            }

            for (int i = 0; i < Borders.Length - 1; i++)
                Borders[i] = BuildBorderPath(borderPoints[i].Skip(borderPoints[i].Length / 2 - 1).Concat(borderPoints[i + 1].Take(borderPoints[i + 1].Length / 2 + 1)), Path.FillType.EvenOdd, false);
            Borders[Borders.Length - 1] = BuildBorderPath(borderPoints.Last().Skip(borderPoints.Last().Length / 2 - 1).Concat(borderPoints.First().Take(borderPoints.First().Length / 2 + 1)), Path.FillType.EvenOdd, false);
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (BitmapData == null)
                return;

            try
            {
                Bitmap tempBitmap = Bitmap.CreateScaledBitmap(BitmapData, canvas.Width, canvas.Height, false);

                float dx = (canvas.Width - tempBitmap.Width) / 2.0f;
                float dy = (canvas.Height - tempBitmap.Height) / 2.0f;

                canvas.DrawBitmap(tempBitmap, dx, dy, null);

                tempBitmap.Dispose();

                if ((TargetImage.Border || (TargetImage.BorderOnClick && Clicked)) && TargetImage.BorderWidth != 0)
                    for (Img.Sides i = Img.Sides.TOP; i <= Img.Sides.LEFT; i++)
                    {
                        Paint borderPaint = new Paint();
                        borderPaint.StrokeCap = Paint.Cap.Round;
                        borderPaint.SetStyle(Paint.Style.Stroke);
                        borderPaint.StrokeWidth = TargetImage.BorderWidth;
                        borderPaint.Color = ColorConvert(TargetImage.GetBorderColor(i));
                        canvas.DrawPath(Borders[(int)i], borderPaint);
                    }
            }
            catch { }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            PostInvalidate();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (base.OnTouchEvent(e))
                return true;

            PostInvalidate();

            return true;
        }
    }
}
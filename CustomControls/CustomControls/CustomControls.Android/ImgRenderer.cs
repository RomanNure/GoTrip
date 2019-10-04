using System;
using System.IO;
using System.Net;

using Android.Content.Res;
using Android.Content;
using Android.Graphics;
using Android.Views;

using CustomControls;
using CustomControls.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Collections.Generic;
using System.Linq;

[assembly: ExportRenderer(typeof(Img), typeof(ImgRenderer))]
namespace CustomControls.Droid
{
    public class ImgRenderer : BoxRenderer
    {
        public Img TargetImage { get; private set; }
        public Bitmap BitmapData { get; private set; }

        private bool Clicked = false;
        private List<List<float>> Borders = new List<List<float>>();

        public float Density => Resources.DisplayMetrics.Density;

        private static readonly Paint TransperentPaint = new Paint();

        public ImgRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);
            TransperentPaint.Color = Android.Graphics.Color.Transparent;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.BoxView> e)
        {
            base.OnElementChanged(e);

            TargetImage = e.NewElement as Img;

            TargetImage.OnSourceChanged += () => UpdateBitmapData();
            TargetImage.OnBorderRadiusChanged += () => UpdateBitmapData();
            TargetImage.OnBorderColorChanged += () => PostInvalidate();

            UpdateBitmapData();
        }

        private Android.Graphics.Color ColorConvert(Xamarin.Forms.Color C)
        {
            return new Android.Graphics.Color((byte)(C.R * 255), (byte)(C.G * 255), (byte)(C.B * 255), (byte)C.A * 255);
        }

        private void UpdateBitmapData()
        {
            if (TargetImage.Source == "")
            {
                BitmapData = null;
                PostInvalidate();
                return;
            }

            Uri url = new Uri(TargetImage.Source);
            HttpWebRequest request = new HttpWebRequest(url);

            try
            {
                Stream response = request.GetResponse().GetResponseStream();
                Bitmap temp = BitmapFactory.DecodeStream(response);

                TargetImage.WidthRequest = TargetImage.WidthRequest == -1 ? temp.Width / Density : TargetImage.WidthRequest;
                TargetImage.HeightRequest = TargetImage.HeightRequest == -1 ? temp.Height / Density : TargetImage.HeightRequest;

                Bitmap tempScaled = Bitmap.CreateScaledBitmap(temp, (int)(TargetImage.WidthRequest * Density), (int)(TargetImage.HeightRequest * Density), false);
                temp.Dispose();

                BitmapData = tempScaled.Copy(Bitmap.Config.Argb8888, true);
                tempScaled.Dispose();

                AddBorderRadiusMask();
            }
            catch { BitmapData = null; }

            PostInvalidate();
        }

        private static readonly int[,] CORNER_PNT_TRANS_COEFS = new int[4, 4]
        {
            { 0,  1, 0,  1 },
            { 1, -1, 0,  1 },
            { 1, -1, 1, -1 },
            { 0,  1, 1, -1 }
        };

        private void AddBorderRadiusMask()
        {
            if (BitmapData == null)
                return;

            Borders.Clear();

            for (int i = 0; i <= Img.SIDES_COUNT; i++)
                Borders.Add(new List<float>());
            int side = 0;

            for (Img.Corners corner = 0; corner < (Img.Corners)Img.CORNERS_COUNT; corner++)
            {
                int borderRadius = TargetImage.GetBorderRadius(corner);

                int start = (borderRadius - 1) * ((int)corner % 2);
                int delta = start == 0 ? 1 : -1;

                int dColor = (int)Math.Sqrt(borderRadius * borderRadius / 2);
                bool isColorChanged = start > dColor;

                for (int i = start; i >= 0 && i < borderRadius; i += delta) //x 
                {
                    int x = (int)(TargetImage.WidthRequest * Density * CORNER_PNT_TRANS_COEFS[(int)corner, 0] + i * CORNER_PNT_TRANS_COEFS[(int)corner, 1]);
                    int maxY = borderRadius - (int)Math.Sqrt(2 * borderRadius * i - i * i);

                    if (x >= 0 && x < BitmapData.Width)
                        for (int j = 0; j < maxY; j++) //y
                        {
                            int y = (int)(TargetImage.HeightRequest * Density * CORNER_PNT_TRANS_COEFS[(int)corner, 2] + j * CORNER_PNT_TRANS_COEFS[(int)corner, 3]);
                            if (y >= 0 && y < BitmapData.Height)
                                BitmapData.SetPixel(x, y, Android.Graphics.Color.Transparent);
                        }

                    int by = (int)(TargetImage.HeightRequest * Density * CORNER_PNT_TRANS_COEFS[(int)corner, 2] + maxY * CORNER_PNT_TRANS_COEFS[(int)corner, 3]);

                    Borders[side].Add(x);
                    Borders[side].Add(by);

                    if (i > dColor != isColorChanged)
                    {
                        Borders[++side].Add(x);
                        Borders[side].Add(by);

                        isColorChanged = !isColorChanged;
                    }
                }

                if (borderRadius == 0)
                {
                    int x = (int)(TargetImage.WidthRequest * Density * CORNER_PNT_TRANS_COEFS[(int)corner, 0]);
                    int y = (int)(TargetImage.HeightRequest * Density * CORNER_PNT_TRANS_COEFS[(int)corner, 2]);

                    Borders[side].Add(x);
                    Borders[side].Add(y);

                    Borders[++side].Add(x);
                    Borders[side].Add(y);

                    isColorChanged = !isColorChanged;
                }
            }

            foreach (float i in Borders.First())
                Borders.Last().Add(i);
            Borders.RemoveAt(0);
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (BitmapData == null)
                return;

            Bitmap tempBitmap = Bitmap.CreateScaledBitmap(BitmapData, canvas.Width, canvas.Height, false);

            float dx = (canvas.Width - tempBitmap.Width) / 2.0f;
            float dy = (canvas.Height - tempBitmap.Height) / 2.0f;

            float ratioX = (float)tempBitmap.Width / BitmapData.Width;
            float ratioY = (float)tempBitmap.Height / BitmapData.Height;

            canvas.DrawBitmap(tempBitmap, dx, dy, null);

            tempBitmap.Dispose();

            if (TargetImage.BorderAlways || TargetImage.ClickAnimation && Clicked)
            {
                for (int i = 0; i < Borders.Count; i++)
                {
                    Paint P = new Paint();
                    P.StrokeCap = Paint.Cap.Round;
                    P.StrokeWidth = TargetImage.ClickedBorderWidth;
                    P.Color = ColorConvert(TargetImage.GetBorderColor((Img.Sides)i));

                    List<float> tempBorders = ShiftAndScalePoints(dx, dy, ratioX, ratioY, Borders[i].ToArray());

                    for (int l = 0; l < tempBorders.Count - 2; l += 2)
                        canvas.DrawLine(tempBorders[l], tempBorders[l + 1], tempBorders[l + 2], tempBorders[l + 3], P);

                    List<float> nextLineFirstPoint = ShiftAndScalePoints(dx, dy, ratioX, ratioY, Borders[(i + 1) % Borders.Count][0], Borders[(i + 1) % Borders.Count][1]);
                    canvas.DrawLine(tempBorders[tempBorders.Count - 2], tempBorders[tempBorders.Count - 1], nextLineFirstPoint[0], nextLineFirstPoint[1], P);
                }
            }
        }

        private List<float> ShiftAndScalePoints(float dx, float dy, float ratioX, float ratioY, params float[] P)
        {
            List<float> NP = new List<float>(P);
            for (int i = 0; i < NP.Count; i += 2)
            {
                NP[i] += dx;
                NP[i + 1] += dy;

                NP[i] *= ratioX;
                NP[i + 1] *= ratioY;
            }
            return NP;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            PostInvalidate();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (TargetImage.Click(e))
                return true;

            Clicked = e.Action != MotionEventActions.Up;
            PostInvalidate();

            return true;
        }
    }
}
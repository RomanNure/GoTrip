using System;
using System.Linq;

using Android.Views;

using Xamarin.Forms;

namespace CustomControls
{
    public class Img : BoxView
    {
        public delegate void Changed();

        public event Changed OnSourceChanged;
        public event Changed OnBorderRadiusChanged;
        public event Changed OnBorderColorChanged;

        public delegate bool MEClicked(MotionEvent ME);

        public event MEClicked MEClick;

        public const int CORNERS_COUNT = 4;
        private int[] BorderRadiuses = new int[CORNERS_COUNT];

        public enum Corners : int
        {
            TOP_LEFT = 0,
            TOP_RIGHT = 1,
            BOT_RIGHT = 2,
            BOT_LEFT = 3
        };

        public const int SIDES_COUNT = 4;
        private Color[] BorderColors = new Color[SIDES_COUNT] { Color.Gray, Color.Gray, Color.Gray, Color.Gray };

        public enum Sides : int
        {
            TOP = 0,
            RIGHT = 1,
            BOT = 2,
            LEFT = 3
        };

        public int GetBorderRadius(Corners corner) => BorderRadiuses[(int)corner];
        public Color GetBorderColor(Sides side) => BorderColors[(int)side];

        public Color TopColor
        {
            get { return BorderColors[(int)Sides.TOP]; }
            set { BorderColors[(int)Sides.TOP] = value; OnBorderColorChanged?.Invoke(); }
        }

        public Color RightColor
        {
            get { return BorderColors[(int)Sides.RIGHT]; }
            set { BorderColors[(int)Sides.RIGHT] = value; OnBorderColorChanged?.Invoke(); }
        }

        public Color BotColor
        {
            get { return BorderColors[(int)Sides.BOT]; }
            set { BorderColors[(int)Sides.BOT] = value; OnBorderColorChanged?.Invoke(); }
        }

        public Color LeftColor
        {
            get { return BorderColors[(int)Sides.LEFT]; }
            set { BorderColors[(int)Sides.LEFT] = value; OnBorderColorChanged?.Invoke(); }
        }

        public Color ClickedBorderColor
        {
            set
            {
                for (int i = 0; i < SIDES_COUNT; i++)
                    BorderColors[i] = value;
                OnBorderColorChanged?.Invoke();
            }
        }

        private bool borderAlways = true;
        public bool BorderAlways { get { return borderAlways; } set { borderAlways = value; } }

        private bool clickAnimation = true;
        public bool ClickAnimation { get { return clickAnimation; } set { clickAnimation = value; } }

        private float clickedBorderWidth = 10;
        public float ClickedBorderWidth { get { return clickedBorderWidth; } set { clickedBorderWidth = Math.Max(value, 0); } }

        public int TopLeftBorderRadius
        {
            get { return BorderRadiuses[(int)Corners.TOP_LEFT]; }
            set { BorderRadiuses[(int)Corners.TOP_LEFT] = value; OnBorderRadiusChanged?.Invoke(); }
        }

        public int TopRightBorderRadius
        {
            get { return BorderRadiuses[(int)Corners.TOP_RIGHT]; }
            set { BorderRadiuses[(int)Corners.TOP_RIGHT] = value; OnBorderRadiusChanged?.Invoke(); }
        }

        public int BotRightBorderRadius
        {
            get { return BorderRadiuses[(int)Corners.BOT_RIGHT]; }
            set { BorderRadiuses[(int)Corners.BOT_RIGHT] = value; OnBorderRadiusChanged?.Invoke(); }
        }

        public int BotLeftBorderRadius
        {
            get { return BorderRadiuses[(int)Corners.BOT_LEFT]; }
            set { BorderRadiuses[(int)Corners.BOT_LEFT] = value; OnBorderRadiusChanged?.Invoke(); }
        }

        public int BorderRadius
        {
            set
            {
                for (int i = 0; i < BorderRadiuses.Length; i++)
                    BorderRadiuses[i] = value;
                OnBorderRadiusChanged?.Invoke();
            }
        }

        private string source = "";
        public string Source
        {
            get { return source; }
            set { source = value; OnSourceChanged?.Invoke(); }
        }

        public bool Click(MotionEvent ME)
        {
            if (MEClick == null)
                return false;

            return MEClick(ME);
        }
    }
}

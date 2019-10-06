using System;
using System.Linq;

using Android.Views;

using Xamarin.Forms;

namespace CustomControls
{
    public class Img : ClickableView
    {
        public event Changed OnSourceChanged;
        public event Changed OnBorderRadiusChanged;
        public event Changed OnBorderColorChanged;

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
            set { Color old = BorderColors[(int)Sides.TOP]; BorderColors[(int)Sides.TOP] = value; IfChangedCall(old, BorderColors[(int)Sides.TOP], OnBorderColorChanged); }
        }

        public Color RightColor
        {
            get { return BorderColors[(int)Sides.RIGHT]; }
            set { Color old = BorderColors[(int)Sides.RIGHT]; BorderColors[(int)Sides.RIGHT] = value; IfChangedCall(old, BorderColors[(int)Sides.RIGHT], OnBorderColorChanged); }
        }

        public Color BotColor
        {
            get { return BorderColors[(int)Sides.BOT]; }
            set { Color old = BorderColors[(int)Sides.BOT]; BorderColors[(int)Sides.BOT] = value; IfChangedCall(old, BorderColors[(int)Sides.BOT], OnBorderColorChanged); }
        }

        public Color LeftColor
        {
            get { return BorderColors[(int)Sides.LEFT]; }
            set { Color old = BorderColors[(int)Sides.LEFT]; BorderColors[(int)Sides.LEFT] = value; IfChangedCall(old, BorderColors[(int)Sides.LEFT], OnBorderColorChanged); }
        }

        public Color ClickedBorderColor
        {
            set
            {
                bool call = false;
                for (int i = 0; i < SIDES_COUNT; i++)
                {
                    call |= BorderColors[i] != value;
                    BorderColors[i] = value;
                }

                if(call) OnBorderColorChanged?.Invoke(this);
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
            set { int old = BorderRadiuses[(int)Corners.TOP_LEFT]; BorderRadiuses[(int)Corners.TOP_LEFT] = value; IfChangedCall(old, BorderRadiuses[(int)Corners.TOP_LEFT], OnBorderRadiusChanged); }
        }

        public int TopRightBorderRadius
        {
            get { return BorderRadiuses[(int)Corners.TOP_RIGHT]; }
            set { int old = BorderRadiuses[(int)Corners.TOP_RIGHT]; BorderRadiuses[(int)Corners.TOP_RIGHT] = value; IfChangedCall(old, BorderRadiuses[(int)Corners.TOP_RIGHT], OnBorderRadiusChanged); }
        }

        public int BotRightBorderRadius
        {
            get { return BorderRadiuses[(int)Corners.BOT_RIGHT]; }
            set { int old = BorderRadiuses[(int)Corners.BOT_RIGHT]; BorderRadiuses[(int)Corners.BOT_RIGHT] = value; IfChangedCall(old, BorderRadiuses[(int)Corners.BOT_RIGHT], OnBorderRadiusChanged); }
        }

        public int BotLeftBorderRadius
        {
            get { return BorderRadiuses[(int)Corners.BOT_LEFT]; }
            set { int old = BorderRadiuses[(int)Corners.BOT_LEFT]; BorderRadiuses[(int)Corners.BOT_LEFT] = value; IfChangedCall(old, BorderRadiuses[(int)Corners.BOT_LEFT], OnBorderRadiusChanged); }
        }

        public int BorderRadius
        {
            set
            {
                bool call = false;
                for (int i = 0; i < BorderRadiuses.Length; i++)
                {
                    call |= BorderRadiuses[i] != value;
                    BorderRadiuses[i] = value;
                }
                if(call) OnBorderRadiusChanged?.Invoke(this);
            }
        }

        private string source = "";
        public string Source
        {
            get { return source; }
            set { source = value; OnSourceChanged?.Invoke(this); }
        }
    }
}

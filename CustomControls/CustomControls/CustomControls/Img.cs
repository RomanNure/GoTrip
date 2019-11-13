using System;

using Xamarin.Forms;

namespace CustomControls
{
    public class Img : ClickableView
    {
        public event Changed OnSourceChanged;

        public event Changed OnCornerRadiusChanged;
        public event Changed OnCornerSmoothChanged;

        public event Changed OnBorderColorChanged;
        public event Changed OnBorderWidthChanged;
        public event Changed OnBorderStatusChanged;

        public const int CORNERS_COUNT = 4;
        private int[] CornerRadiuses = new int[CORNERS_COUNT];

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

        public int GetCornerRadius(Corners corner) => CornerRadiuses[(int)corner];
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

        public Color BorderColor
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

        private const int MIN_SMOOTH = 1;
        private int cornersSmooth = 20;
        public int CornersSmooth { get => cornersSmooth; set { int oldSmooth = cornersSmooth; cornersSmooth = Math.Max(value, MIN_SMOOTH); IfChangedCall(oldSmooth, cornersSmooth, OnCornerSmoothChanged); } }

        private bool border = true;
        public bool Border { get { return border; } set { bool oldBorder = border; border = value; IfChangedCall(oldBorder, border, OnBorderStatusChanged); } }

        private bool borderOnClick = true;
        public bool BorderOnClick { get { return borderOnClick; } set { bool oldBorderOnClick = borderOnClick; borderOnClick = value; IfChangedCall(oldBorderOnClick, borderOnClick, OnBorderStatusChanged); } }

        private float borderWidth = 10;
        public float BorderWidth { get { return borderWidth; } set { float oldBorderWidth = borderWidth; borderWidth = Math.Max(value, 0); IfChangedCall(oldBorderWidth, borderWidth, OnBorderWidthChanged); } }

        public int TopLeftCornerRadius
        {
            get { return CornerRadiuses[(int)Corners.TOP_LEFT]; }
            set
            {
                int old = CornerRadiuses[(int)Corners.TOP_LEFT];
                CornerRadiuses[(int)Corners.TOP_LEFT] = GetPossibleCornerRad(value, Corners.TOP_RIGHT, Corners.BOT_LEFT);
                IfChangedCall(old, CornerRadiuses[(int)Corners.TOP_LEFT], OnCornerRadiusChanged);
            }
        }

        public int TopRightCornerRadius
        {
            get { return CornerRadiuses[(int)Corners.TOP_RIGHT]; }
            set
            {
                int old = CornerRadiuses[(int)Corners.TOP_RIGHT];
                CornerRadiuses[(int)Corners.TOP_RIGHT] = GetPossibleCornerRad(value, Corners.TOP_LEFT, Corners.BOT_RIGHT);
                IfChangedCall(old, CornerRadiuses[(int)Corners.TOP_RIGHT], OnCornerRadiusChanged);
            }
        }

        public int BotRightCornerRadius
        {
            get { return CornerRadiuses[(int)Corners.BOT_RIGHT]; }
            set
            {
                int old = CornerRadiuses[(int)Corners.BOT_RIGHT];
                CornerRadiuses[(int)Corners.BOT_RIGHT] = GetPossibleCornerRad(value, Corners.BOT_LEFT, Corners.TOP_RIGHT);
                IfChangedCall(old, CornerRadiuses[(int)Corners.BOT_RIGHT], OnCornerRadiusChanged);
            }
        }

        public int BotLeftCornerRadius
        {
            get { return CornerRadiuses[(int)Corners.BOT_LEFT]; }
            set
            {
                int old = CornerRadiuses[(int)Corners.BOT_LEFT];
                CornerRadiuses[(int)Corners.BOT_LEFT] = GetPossibleCornerRad(value, Corners.BOT_RIGHT, Corners.TOP_LEFT);
                IfChangedCall(old, CornerRadiuses[(int)Corners.BOT_LEFT], OnCornerRadiusChanged);
            }
        }

        private int GetPossibleCornerRad(int radius, Corners horisontalOppositeCorner, Corners verticalOppositeCorner)
        {
            if (radius <= 0) return 0;

            int maxOppositeRadius = Math.Max(CornerRadiuses[(int)horisontalOppositeCorner], CornerRadiuses[(int)verticalOppositeCorner]);
            double minSize = Math.Min(WidthRequest, HeightRequest);

            return radius + maxOppositeRadius <= minSize ? radius : (int)minSize - maxOppositeRadius;
        }

        public int CornerRad
        {
            set
            {
                bool call = false;
                for (int i = 0; i < CornerRadiuses.Length; i++)
                {
                    call |= CornerRadiuses[i] != value;
                    CornerRadiuses[i] = value;
                }
                if(call) OnCornerRadiusChanged?.Invoke(this);
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

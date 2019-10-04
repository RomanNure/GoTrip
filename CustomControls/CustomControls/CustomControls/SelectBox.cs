using System;
using Xamarin.Forms;

namespace CustomControls
{
    public class SelectBox : ClickableView
    {
        public event Changed OnColorChanged;

        public bool Checked { get; set; }

        private Color outerColor = Color.Gray;
        public Color OuterColor { get { return outerColor; } set { if (value != null) outerColor = value; OnColorChanged?.Invoke(); } }

        private Color innerColor = Color.Gray;
        public Color InnerColor { get { return innerColor; } set { if (value != null) innerColor = value; OnColorChanged?.Invoke(); } }

        private int outerWidth = 3;
        public int OuterWidth { get { return outerWidth; } set { outerWidth = Math.Max(0, value); } }

        private int radiusDiffer = 3;
        public int RadiusDiffer { get { return radiusDiffer; } set { radiusDiffer = Math.Max(0, value); } }

        public float InnerRadius { get { return (float)Math.Max(0, (Math.Min(Height, Width) - OuterWidth) / 2 - RadiusDiffer); } }
    }
}

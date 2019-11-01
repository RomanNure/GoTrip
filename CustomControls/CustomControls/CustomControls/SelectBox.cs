using System;
using Xamarin.Forms;

namespace CustomControls
{
    public abstract class SelectBox : ClickableView
    {
        public event Changed OnColorChanged;
        public event Changed CheckedChanged;

        private bool checked_ = false;
        public bool Checked
        {
            get { return checked_; }
            set { bool old = checked_; checked_ = value; IfChangedCall(old, checked_, CheckedChanged); }
        }

        private Color outerColor = Color.Gray;
        public Color OuterColor { get { return outerColor; } set { Color old = outerColor; if (value != null) outerColor = value; IfChangedCall(old, outerColor, OnColorChanged); } }

        private Color innerColor = Color.Gray;
        public Color InnerColor { get { return innerColor; } set { Color old = innerColor; if (value != null) innerColor = value; IfChangedCall(old, innerColor, OnColorChanged); } }

        private int outerWidth = 3;
        public int OuterWidth { get { return outerWidth; } set { outerWidth = Math.Max(0, value); } }

        private int radiusDiffer = 3;
        public int RadiusDiffer { get { return radiusDiffer; } set { radiusDiffer = Math.Max(0, value); } }

        public float InnerRadius { get { return OuterWidth / 2 - RadiusDiffer; } }
    }
}

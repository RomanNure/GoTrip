using System;

namespace CustomControls
{
    public class CheckBox : SelectBox
    {
        private int borderRadiusX = 0;
        public int BorderRadiusX { get { return borderRadiusX; } set { borderRadiusX = Math.Max(0, value); } }

        private int borderRadiusY = 0;
        public int BorderRadiusY { get { return borderRadiusY; } set { borderRadiusY = Math.Max(0, value); } }

        public int BorderRadius { set { BorderRadiusX = value; BorderRadiusY = value; } }
    }
}

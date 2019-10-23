using System;
using System.Collections;
using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class TextCounter : Label, ICounter
    {
        private int current = 1;

        private int max = 0;
        public int Max { get { return max; } set { max = Math.Max(1, value); Update(); } }

        public bool MoveNext()
        {
            ++current; Update();
            return current < Max;
        }

        public bool MovePrev()
        {
            --current; Update();
            return current > 1;
        }

        public void Update() => Text = current.ToString() + "/" + Max.ToString();
    }
}

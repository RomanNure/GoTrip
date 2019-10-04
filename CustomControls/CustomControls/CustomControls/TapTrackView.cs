using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CustomControls
{
    public class TapTrackView : View
    {
        public delegate void Update();

        public event Update OnPop;
        public event Update OnClear;

        public List<List<Point>> PointGroups { get; private set; }

        public TapTrackView()
        {
            PointGroups = new List<List<Point>>();
            PointGroups.Add(new List<Point>());
        }

        public void Clear()
        {
            PointGroups.Clear();
            OnClear();
        }

        public void Pop()
        {
            PointGroups.RemoveAt(PointGroups.Count - 1);
            OnPop();
        }
    }
}

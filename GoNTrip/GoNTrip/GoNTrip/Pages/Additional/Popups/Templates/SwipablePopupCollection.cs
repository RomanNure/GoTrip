using System.Collections.Generic;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class SwipablePopupCollection
    {
        private List<SwipablePhotoPopup> popups = new List<SwipablePhotoPopup>();
        private ICounter counter = null;

        public SwipablePopupCollection(ICounter counter) => this.counter = counter;

        public void Add(SwipablePhotoPopup popup)
        {
            popups.Add(popup);
            popup.OnRightBorderExceeded += (sender) => { if (sender.Equals(current)) MovePrev(); };
            popup.OnLeftBorderExceeded += (sender) => { if (sender.Equals(current)) MoveNext(); };

            ++counter.Max;
        }

        private int currentIndex = 0;
        private Popup current => hasCurrent ? popups[currentIndex] : null;

        private bool hasCurrent => currentIndex >= 0 && currentIndex < Count;
        private bool hasNext => currentIndex < Count - 1;
        private bool hasPrev => currentIndex > 0;

        public int Count => popups.Count;

        private bool Move(bool isMoveNext)
        {
            if (!hasCurrent || !((isMoveNext && hasNext) || (!isMoveNext && hasPrev)))
                return false;

            current.ForceHide();
            currentIndex += isMoveNext ? 1 : -1;
            current.Show();

            if (isMoveNext) counter?.MoveNext();
            else counter?.MovePrev();

            return true;
        }

        public bool MoveNext() => Move(true);
        public bool MovePrev() => Move(false);
    }
}

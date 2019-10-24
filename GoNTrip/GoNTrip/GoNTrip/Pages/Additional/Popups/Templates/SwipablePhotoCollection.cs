using System.Collections.Generic;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class SwipablePhotoCollection
    {
        private List<SwipablePhotoPopup> popups = new List<SwipablePhotoPopup>();
        private PopupControlSystem PopupControl { get; set; }

        public SwipablePhotoCollection(PopupControlSystem popupControl) => this.PopupControl = popupControl;

        public void Add(SwipablePhotoPopup popup)
        {
            popups.Add(popup);

            popup.OnRightBorderExceeded += (sender) => { if (sender.Equals(current)) MovePrev(); };
            popup.OnLeftBorderExceeded += (sender) => { if (sender.Equals(current)) MoveNext(); };

            popup.OnBotBorderExceeded += (sender) => Reset();
            popup.OnTopBorderExceeded += (sender) => Reset();
        }

        private int currentIndex = -1;
        private SwipablePhotoPopup current => hasCurrent ? popups[currentIndex] : null;

        private bool hasCurrent => currentIndex >= 0 && currentIndex < Count;
        private bool hasNext => currentIndex < Count - 1;
        private bool hasPrev => currentIndex > 0;

        public int Count => popups.Count;
        public bool Opened { get; private set; }

        private bool Move(bool isMoveNext)
        {
            if (!((isMoveNext && hasNext) || (!isMoveNext && hasPrev)))
                return false;

            Opened = true;

            if (hasCurrent)
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

            currentIndex += isMoveNext ? 1 : -1;
            PopupControl.OpenPopup(current);

            current.Text = (currentIndex + 1) + "/" + popups.Count;

            return true;
        }

        public bool MoveNext() => Move(true);
        public bool MovePrev() => Move(false);

        public void Reset()
        {
            if (hasCurrent)
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

            Opened = false;

            currentIndex = -1;
        }
    }
}

using System.Collections.Generic;

namespace GoNTrip.Pages.Help
{
    public class PopupControlSystem
    {
        public delegate bool Click();
        public Click BackButtonClick { get; private set; }

        public int OpenedPopupsCount { get { return OpenedPopups.Count; } }
        private Stack<Popup> OpenedPopups = new Stack<Popup>();
        
        public PopupControlSystem(Click backButtonClick)
        {
            BackButtonClick = backButtonClick;
        }

        public void OpenPopup(Popup popup)
        {
            popup.Show();
            OpenedPopups.Push(popup);
        }

        public void CloseTopPopupAndHideKeyboardIfNeeded(bool forced = false)
        {
            if (IsKeyboardVisible())
                BackButtonClick();
            CloseTopPopup(forced);
        }

        public bool IsKeyboardVisible() => OpenedPopups.Count == 0 ? false : OpenedPopups.Peek().TotalFocuses != 0;

        public bool CloseTopPopup(bool forced = false)
        {
            if (OpenedPopups.Count == 0)
                return false;

            Popup topPopup = OpenedPopups.Peek();
            bool needHideKeyboard = topPopup.TotalFocuses != 0;

            if ((!forced && topPopup.Hide()) || (forced && topPopup.ForceHide()))
                OpenedPopups.Pop();

            return needHideKeyboard;
        }
    }
}

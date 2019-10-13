using NUnit.Framework;

using GoNTrip.Pages.Additional.Popups;

namespace UnitTests
{
    [TestFixture]
    public class PopupControllerTestClass
    {
        [Test]
        public void OpenClosePopupTest()
        {
            PopupControlSystem popupControlSystem = new PopupControlSystem(() => false);

            Popup P1 = new Popup();
            P1.Closable = true;

            Popup P2 = new Popup();
            P2.Closable = false;

            Popup P3 = new Popup();
            P3.Closable = true;

            popupControlSystem.OpenPopup(P1);
            popupControlSystem.OpenPopup(P2);
            popupControlSystem.OpenPopup(P3);

            Assert.IsTrue(popupControlSystem.OpenedPopupsCount == 3);

            popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded();
            Assert.IsTrue(popupControlSystem.OpenedPopupsCount == 2);

            popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded();
            Assert.IsTrue(popupControlSystem.OpenedPopupsCount == 2);

            popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(true);
            Assert.IsTrue(popupControlSystem.OpenedPopupsCount == 1);

            popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded();
            Assert.IsTrue(popupControlSystem.OpenedPopupsCount == 0);

            popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded();
            Assert.IsTrue(popupControlSystem.OpenedPopupsCount == 0);

            popupControlSystem.CloseTopPopupAndHideKeyboardIfNeeded(true);
            Assert.IsTrue(popupControlSystem.OpenedPopupsCount == 0);
        }

        [Test]
        public void TestTrue()
        {
            Assert.IsTrue(true);
        }
    }
}

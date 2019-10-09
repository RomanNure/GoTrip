using System;
using System.Linq;
using System.Collections.Generic;

using CustomControls;

using Xamarin.Forms;

namespace GoNTrip.Pages.Help
{
    public class Popup
    {
        public ClickableContentView PopupWrapper { get; private set; }
        public ClickableFrame PopupOuterFrame { get; private set; }
        public List<InputView> Inputs { get; private set; }
        public int TotalFocuses { get; private set; }
        public bool Closable { get; private set; }
        public Action OnShow { get; private set; }
        public Action OnHide { get; private set; }

        public Popup(ClickableContentView popupWrapper, Clicked onWrapperClicked, 
                     ClickableFrame popupOuterFrame, Clicked onOuterFrameClicked,
                     bool closable = true, Action OnShow = null, Action OnHide = null,
                     params InputView[] inputs)
        {
            PopupWrapper = popupWrapper;
            PopupOuterFrame = popupOuterFrame;
            Inputs = inputs.ToList();
            Closable = closable;

            foreach(InputView input in Inputs)
            {
                input.Focused += (object sender, FocusEventArgs e) => TotalFocuses++;
                input.Unfocused += (object sender, FocusEventArgs e) => TotalFocuses--;
            }

            PopupWrapper.OnClick += onWrapperClicked;
            PopupOuterFrame.OnClick += onOuterFrameClicked;
        }

        public void Show()
        {
            OnShow?.Invoke();
            PopupWrapper.IsVisible = true;
        }

        public bool ForceHide()
        {
            OnHide?.Invoke();

            foreach (InputView input in Inputs)
                input.Unfocus();

            PopupWrapper.IsVisible = false;
            return true;
        }

        public bool Hide()
        {
            if (Closable)
                ForceHide();
            return Closable;
        }
    }
}

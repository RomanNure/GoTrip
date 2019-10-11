using System;
using System.ComponentModel;
using System.Collections.Generic;

using CustomControls;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Popups
{
    public class IllegalPoupStructureException : Exception { }

    public class Popup : ClickableContentView
    {
        public bool Closable { get; set; }

        public Clicked OnPopupWrapperClicked { get; set; }
        public Clicked OnPopupBodyClicked { get; set; }

        public Action OnShow { get; set; }
        public Action OnHide { get; set; }

        public int TotalFocuses { get; private set; }
        private List<InputView> Inputs = new List<InputView>();

        public Popup()
        {
            this.ChildAdded += (sender, e) => UpdateEvents();
            this.ChildRemoved += (sender, e) => UpdateEvents();
        }

        private void UpdateEvents()
        {
            try
            {
                Inputs.Clear();
                this.OnClick += OnPopupWrapperClicked;

                ClickableFrame OuterFrame = this.Content as ClickableFrame;
                OuterFrame.OnClick += OnPopupBodyClicked;

                ClickableFrame InnerFrame = OuterFrame.Content as ClickableFrame;
                StackLayout ContentWrapper = InnerFrame.Content as StackLayout;

                foreach (View input in ContentWrapper.Children)
                    if (typeof(InputView).IsInstanceOfType(input))
                    {
                        Inputs.Add(input as InputView);
                        input.Focused += (object sender, FocusEventArgs e) => TotalFocuses++;
                        input.Unfocused += (object sender, FocusEventArgs e) => TotalFocuses--;
                    }
            }
            catch
            {
                throw new IllegalPoupStructureException();
            }
        }

        public void Show()
        {
            OnShow?.Invoke();
            this.IsVisible = true;
        }

        public bool ForceHide()
        {
            OnHide?.Invoke();

            foreach (InputView input in Inputs)
                input.Unfocus();

            this.IsVisible = false;
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

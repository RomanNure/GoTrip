using System;
using System.Linq;
using System.Collections.Generic;

using CustomControls;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class SimpleMessagePopup : Popup
    {
        public const string POPUP_LABEL_CLASS = "PopupLabel";
        public const string POPUP_BUTTON_CLASS = "PopupButton";

        public const string INNER_FRAME_CLASS = "PopupInnerFrame";
        public const string OUTER_FRAME_CLASS = "PopupOuterFrame";
        public const string POPUP_BACKGROUND_CLASS = "PopupBackgroud";

        public const string INNER_FRAME_COLOR = "BarBackColor";
        public const string POPUP_BACKGROUND_COLOR = "InactiveTransperentColor";

        private int popupWidth = 150;
        public int PopupWidth { get { return popupWidth; } set { popupWidth = Math.Max(0, value); UpdatePopupShape(); } }

        private int popupHeight = 150;
        public int PopupHeight { get { return popupHeight; } set { popupHeight = Math.Max(0, value); UpdatePopupShape(); } }

        private int cornerRadius = 10;
        public int CornerRadius { get { return cornerRadius; } set { cornerRadius = Math.Max(0, value); UpdatePopupShape(); } }

        private string messageText = "";
        public string MessageText { get { return messageText; } set { messageText = value; UpdateMessageText(); } }

        private string firstButtonText = "";
        public string FirstButtonText { get { return firstButtonText; } set { firstButtonText = value; UpdateButtons(); } }

        private string secondButtonText = "";
        public string SecondButtonText { get { return secondButtonText; } set { secondButtonText = value; UpdateButtons(); } }

        private string thirdButtonText = "";
        public string ThirdButtonText { get { return thirdButtonText; } set { thirdButtonText = value; UpdateButtons(); } }

        private EventHandler onFirstButtonClicked;
        public EventHandler OnFirstButtonClicked { get { return onFirstButtonClicked; } set { onFirstButtonClicked = value; UpdateButtons(); } }

        private EventHandler onSecondButtonClicked;
        public EventHandler OnSecondButtonClicked { get { return onSecondButtonClicked; } set { onSecondButtonClicked = value; UpdateButtons(); } }

        private EventHandler onThirdButtonClicked;
        public EventHandler OnThirdButtonClicked { get { return onThirdButtonClicked; } set { onThirdButtonClicked = value; UpdateButtons(); } }

        private ClickableFrame OuterFrame { get; set; }
        private ClickableFrame InnerFrame { get; set; }

        protected StackLayout ContentsLayout { get; private set; }
        protected Grid ButtonGrid { get; private set; }

        private Label MessageLabel { get; set; }

        private Button FirstButton { get; set; }
        private Button SecondButton { get; set; }
        private Button ThirdButton { get; set; }


        public SimpleMessagePopup()
        {
            StackLayout layout = new StackLayout();
            ContentsLayout = layout;

            Label messageLabel = new Label();
            messageLabel.Style = (Style)Application.Current.Resources[POPUP_LABEL_CLASS];
            messageLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            messageLabel.VerticalOptions = LayoutOptions.FillAndExpand;
            messageLabel.HorizontalTextAlignment = TextAlignment.Center;
            messageLabel.VerticalTextAlignment = TextAlignment.Center;
            MessageLabel = messageLabel;
            layout.Children.Add(MessageLabel);

            UpdateMessageText();
            UpdateButtons();

            ClickableFrame innerFrame = new ClickableFrame();
            innerFrame.Style = (Style)Application.Current.Resources[INNER_FRAME_CLASS];
            innerFrame.BackgroundColor = (Color)Application.Current.Resources[INNER_FRAME_COLOR];
            innerFrame.Content = ContentsLayout;
            InnerFrame = innerFrame;

            ClickableFrame outerFrame = new ClickableFrame();
            outerFrame.Style = (Style)Application.Current.Resources[OUTER_FRAME_CLASS];
            outerFrame.Content = innerFrame;
            OuterFrame = outerFrame;

            UpdatePopupShape();

            base.Style = (Style)Application.Current.Resources[POPUP_BACKGROUND_CLASS];
            BackgroundColor = (Color)Application.Current.Resources[POPUP_BACKGROUND_COLOR];
            IsVisible = false;
            Closable = true;
            Content = outerFrame;
            OnPopupBodyClicked = (e, sender) => true;
        }

        public void UpdateMessageText()
        {
            if (MessageLabel != null)
                MessageLabel.Text = MessageText;
        }

        public void UpdatePopupShape()
        {
            if (InnerFrame != null)
            {
                InnerFrame.WidthRequest = PopupWidth;
                InnerFrame.HeightRequest = PopupHeight;
            }


            if (OuterFrame != null)
            {
                OuterFrame.WidthRequest = PopupWidth;
                OuterFrame.HeightRequest = PopupHeight;

                OuterFrame.CornerRadius = CornerRadius;
            }
        }

        public void UpdateButtons()
        {
            List<(string, EventHandler)> buttons = new List<(string, EventHandler)>() {
                (FirstButtonText, OnFirstButtonClicked),
                (SecondButtonText, OnSecondButtonClicked),
                (ThirdButtonText, OnThirdButtonClicked)
            };

            Grid buttonGrid = new Grid();
            buttonGrid.HorizontalOptions = LayoutOptions.FillAndExpand;
            buttonGrid.VerticalOptions = LayoutOptions.FillAndExpand;

            int i = 0;
            foreach((string, EventHandler) buttonInfo in buttons.Where(B => B.Item1 != "" || B.Item2 != null))
            {
                Button button = new Button();
                button.Text = buttonInfo.Item1;
                button.Style = (Style)Application.Current.Resources[POPUP_BUTTON_CLASS];
                button.Clicked += (sender, e) => buttonInfo.Item2?.Invoke(sender, e);
                buttonGrid.Children.Add(button, i++, 0);
            }

            ButtonGrid = buttonGrid;

            if (ContentsLayout != null)
            {
                if (ContentsLayout.Children.Count == 2)
                    ContentsLayout.Children.RemoveAt(1);

                ContentsLayout.Children.Add(buttonGrid);
            }
        }
    }
}

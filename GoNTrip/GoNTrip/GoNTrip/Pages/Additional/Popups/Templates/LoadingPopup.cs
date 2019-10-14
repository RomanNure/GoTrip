using System;

using CustomControls;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class LoadingPopup : Popup
    {
        public const string POPUP_BACKGROUND_COLOR = "InactiveTransperentColor";
        public const string INDICATOR_COLOR = "AdditionalColor";

        public const string INNER_FRAME_CLASS = "PopupInnerFrame";
        public const string OUTER_FRAME_CLASS = "PopupOuterFrame";
        public const string POPUP_BACKGROUND_CLASS = "PopupBackgroud";

        private int circleRadius = 50;
        public int CircleRadius { get { return circleRadius; } set { circleRadius = Math.Max(0, value); UpdateSize(); } }

        private ClickableFrame OuterFrame { get; set; }
        private ClickableFrame InnerFrame { get; set; }

        public LoadingPopup()
        {
            ActivityIndicator indicator = new ActivityIndicator();
            indicator.HorizontalOptions = LayoutOptions.FillAndExpand;
            indicator.VerticalOptions = LayoutOptions.FillAndExpand;
            indicator.Color = (Color)Application.Current.Resources[INDICATOR_COLOR];
            indicator.IsRunning = true;

            StackLayout layout = new StackLayout();
            layout.Children.Add(indicator);

            ClickableFrame innerFrame = new ClickableFrame();
            innerFrame.Style = (Style)Application.Current.Resources[INNER_FRAME_CLASS];
            innerFrame.Content = layout;
            InnerFrame = innerFrame;            

            ClickableFrame outerFrame = new ClickableFrame();
            outerFrame.Style = (Style)Application.Current.Resources[OUTER_FRAME_CLASS];
            outerFrame.Content = innerFrame;
            OuterFrame = outerFrame;

            UpdateSize();

            base.Style = (Style)Application.Current.Resources[POPUP_BACKGROUND_CLASS];
            BackgroundColor = (Color)Application.Current.Resources[POPUP_BACKGROUND_COLOR];
            IsVisible = false;
            Closable = false;
            Content = outerFrame;
            OnPopupBodyClicked = (e, sender) => true;
        }

        public void UpdateSize()
        {
            if (OuterFrame != null)
            {
                OuterFrame.WidthRequest = CircleRadius * 2;
                OuterFrame.HeightRequest = CircleRadius * 2;
            }

            if (InnerFrame != null)
            {
                InnerFrame.WidthRequest = CircleRadius * 2;
                InnerFrame.HeightRequest = CircleRadius * 2;
            }
        }
    }
}

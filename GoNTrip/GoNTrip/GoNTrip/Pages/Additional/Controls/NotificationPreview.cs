using CustomControls;

using Xamarin.Forms;

using GoNTrip.Model;

namespace GoNTrip.Pages.Additional.Controls
{
    public class NotificationPreview : ClickableFrame
    {
        private BoxView Unread { get; set; }
        private Label InnerContent { get; set; }

        public Style ContentStyle { get => InnerContent.Style; set => InnerContent.Style = value; }
        public Style UnreadFlagStyle { get => Unread.Style; set => Unread.Style = value; }

        public NotificationPreview() : base()
        {
            AbsoluteLayout layout = new AbsoluteLayout();

            Unread = new BoxView();
            InnerContent = new Label();

            layout.Children.Add(InnerContent);
            layout.Children.Add(Unread);

            this.Content = layout;
        }

        public void FillWith(Notification notification)
        {
            InnerContent.Text = $"{notification.type}: {notification.topic}";
            Unread.IsVisible = !notification.isChecked;
        }
    }
}

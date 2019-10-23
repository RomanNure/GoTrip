using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Popups.Templates
{
    public class SignedPhotoPopup : PhotoPopup
    {
        public Label Sign { get; private set; }

        private string text = "";
        public string Text { get { return text; } set { text = value; if(Sign != null) Sign.Text = text; } }

        private const string SIGN_STYLE_CLASS = "PhotoSign";

        public SignedPhotoPopup() : base()
        {
            Sign = new Label();
            Sign.VerticalOptions = LayoutOptions.StartAndExpand;
            Sign.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Sign.Style = (Style)App.Current.Resources[SIGN_STYLE_CLASS];

            ContentWrapper.Children.Add(Sign);
            Image.VerticalOptions = LayoutOptions.EndAndExpand;
        }
    }
}

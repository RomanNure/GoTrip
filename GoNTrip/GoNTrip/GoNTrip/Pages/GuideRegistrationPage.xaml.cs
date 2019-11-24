using System;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CustomControls;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.Validators.Templates;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuideRegistrationPage : ContentPage
    {
        private const char KEYWORD_SEPARATOR = ',';

        private const string KEYWORD_LABEL_CLASS_NAME = "KeyBlock";
        private const string KEYWORD_FRAME_CLASS_NAME = "KeyBlockFrame";

        private IEnumerable<string> Keywords { get; set; }

        private PageMemento PrevPageMemento { get; set; }
        private PopupControlSystem PopupControl { get; set; }
        private GuideRegisterValidator Validator { get; set; }

        public GuideRegistrationPage(PageMemento prevPageMemento)
        {
            InitializeComponent();

            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            GuideHowToOK.Clicked += (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

            PrevPageMemento = prevPageMemento;
            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            Validator = new GuideRegisterValidator(GuidCard, GuideKeyWords, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);
        }

        private void ContentPage_Appearing(object sender, EventArgs e) =>
            PopupControl.OpenPopup(GuideHowToPopup);

        private void GuideKeyWords_TextChanged(object sender, TextChangedEventArgs e)
        {
            KeywordListWrapper.Children.Clear();
            Keywords = (sender as Editor).Text.Split(KEYWORD_SEPARATOR).Select(K => K.Trim()).Where(K => !string.IsNullOrEmpty(K));

            Style labelStyle = (Style)Resources[KEYWORD_LABEL_CLASS_NAME];
            Style keywordFrameStyle = (Style)Resources[KEYWORD_FRAME_CLASS_NAME];

            foreach (string keyword in Keywords)
            {
                ClickableFrame keywordFrame = new ClickableFrame();
                keywordFrame.Style = keywordFrameStyle;

                Label keywordLabel = new Label() { Text = keyword };
                keywordLabel.Style = labelStyle;

                keywordFrame.Content = keywordLabel;
                KeywordListWrapper.Children.Add(keywordFrame);
            }
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (!Validator.ValidateAll())
                return;

            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                await App.DI.Resolve<GuideController>().Add(Keywords, GuidCard.Text);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                App.Current.MainPage = new CurrentUserProfilePage();
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }
            
        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount != 0)
            {
                PopupControl.CloseTopPopup();
                return true;
            }
            else if (PrevPageMemento != null)
            {
                App.Current.MainPage = PrevPageMemento.Restore();
                return true;
            }

            return false;
        }
    }
}
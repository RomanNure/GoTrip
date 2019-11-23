using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Popups;
using GoNTrip.Pages.Additional.PageMementos;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.Pages.Additional.Validators.Templates;
using Autofac;
using GoNTrip.Controllers;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardEnterPage : ContentPage
    {
        private Tour CurrentTour { get; set; }

        private PageMemento PrevPageMemento { get; set; }
        private PopupControlSystem PopupControl { get; set; }
        private CardValidator Validator { get; set; }

        public CardEnterPage(PageMemento prevPageMemento, Tour tour)
        {
            InitializeComponent();

            CurrentTour = tour;
            PrevPageMemento = prevPageMemento;

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            Validator = new CardValidator(UserCard, MonthExp, YearExp, Cvv, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);

            HeaderLabel.Text = $"Pay {tour.pricePerPerson} for participating";
        }

        private async void PayButton_Clicked(object sender, EventArgs e)
        {
            if (!Validator.ValidateAll())
                return;

            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                Card card = new Card(UserCard.Text, MonthExp.Text, YearExp.Text, Cvv.Text);
                await App.DI.Resolve<PayController>().PayForTour(CurrentTour, card);

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
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
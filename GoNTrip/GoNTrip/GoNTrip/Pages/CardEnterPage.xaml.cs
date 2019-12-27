using System;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
    public partial class CardEnterPage : ContentPage
    {
        private Tour CurrentTour { get; set; }
        private double? CustomTourGuideSalary { get; set; }

        private PageMemento PrevPageMemento { get; set; }
        private PopupControlSystem PopupControl { get; set; }
        private CardValidator Validator { get; set; }

        public CardEnterPage(PageMemento prevPageMemento, Tour tour, double? customTourGuideSalary = null)
        {
            InitializeComponent();

            CurrentTour = tour;
            CustomTourGuideSalary = customTourGuideSalary;
            PrevPageMemento = prevPageMemento;

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            ErrorPopup.OnFirstButtonClicked = (ctx, arg) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            Validator = new CardValidator(UserCard, MonthExp, YearExp, Cvv, Constants.VALID_HANDLER, Constants.INVALID_HANDLER);

            HeaderLabel.Text = $"Pay {tour.pricePerPerson}{Constants.CURRENCY_SYMBOL} for participating";
        }

        private async void PayButton_Clicked(object sender, EventArgs e)
        {
            if (!Validator.ValidateAll())
                return;

            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                TourController joinTourController = App.DI.Resolve<TourController>();
                PayController payController = App.DI.Resolve<PayController>();

                Card card = new Card(UserCard.Text.Replace(" ", ""), MonthExp.Text, YearExp.Text, Cvv.Text);
                LiqpayPayment payment = payController.CreatePayment(CurrentTour, card, CustomTourGuideSalary);

                if (!(await joinTourController.JoinPrepare(CurrentTour, payment)))
                    throw new ResponseException("Payment prepearing failed");

                LiqpayResponse response = await App.DI.Resolve<PayController>().PayForTour(payment);

                while(true)
                {
                    Thread.Sleep(1000);
                    JoinTourStatus join = await joinTourController.JoinCheckStatus(payment);

                    switch(join.status)
                    {
                        case JoinStatus.PENDING: continue;
                        case JoinStatus.FAILED: throw new ResponseException("Failed to join tour");
                        case JoinStatus.SUCCESS: break;
                    }

                    break;
                }

                App.Current.MainPage = PrevPageMemento.Restore();
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
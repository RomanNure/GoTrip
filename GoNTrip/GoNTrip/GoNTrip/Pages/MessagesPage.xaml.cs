using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Pages.Additional.Popups;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesPage : ContentPage
    {
        public MessagesPage()
        {
            InitializeComponent();
        }

        private PopupControlSystem PopupControl { get; set; }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            Navigator.Current = Additional.Controls.DefaultNavigationPanel.PageEnum.MESSAGES;
            Navigator.LinkClicks(PopupControl, ActivityPopup);
        }
    }
}
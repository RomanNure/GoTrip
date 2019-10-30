using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Pages.Additional.Popups;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvancedPage : ContentPage
    {
        public AdvancedPage()
        {
            InitializeComponent();
        }

        private PopupControlSystem PopupControl { get; set; }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            PopupControl = new PopupControlSystem(OnBackButtonPressed);

            Navigator.Current = Additional.Controls.DefaultNavigationPanel.PageEnum.ADVANCED;
            Navigator.LinkClicks(PopupControl, ActivityPopup);
        }
    }
}
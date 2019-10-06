using System;
using System.ComponentModel;

using GoNTrip.Pages;

using Xamarin.Forms;

namespace GoNTrip
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Appearing += MainPage_Appearing;
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            App.Current.MainPage = new SignUpPage();
        }
    }
}

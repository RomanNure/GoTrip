using System;

using GoNTrip.Model;
using GoNTrip.Pages.Additional.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtherUserProfilePage : ContentPage
    {
        public OtherUserProfilePage(User user)
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Navigator.Current = DefaultNavigationPanel.PageEnum.OTHER;
            Navigator.LinkClicks();
        }
    }
}
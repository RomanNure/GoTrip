using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvancedPage : ContentPage
    {
        public AdvancedPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Navigator.Current = Additional.Controls.DefaultNavigationPanel.PageEnum.ADVANCED;
            Navigator.LinkClicks();
        }
    }
}
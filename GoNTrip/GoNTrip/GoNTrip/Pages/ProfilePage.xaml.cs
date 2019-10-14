using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GoNTrip.Pages.Additional.Popups;

namespace GoNTrip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private long UserId { get; set; }
        private PopupControlSystem PopupControl = null;

        public ProfilePage(long userId)
        {
            UserId = userId;
            InitializeComponent();
            PopupControl = new PopupControlSystem(OnBackButtonPressed);
        }

        private void ProfilePage_Appearing(object sender, System.EventArgs e)
        {
            LoadUserProfile();
        }

        private async void LoadUserProfile()
        {
            PopupControl.OpenPopup(ActivityPopup);

            await Task.Run(() =>
            {
                UserAvatar.Source = "https://i.stack.imgur.com/3vbu5.jpg?s=32&g=1";
                
            });

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new MainPage();
            return true;
        }
    }
}
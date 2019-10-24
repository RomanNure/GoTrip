using System.Collections.Generic;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Controls
{
    public class NavigationPanel : Grid
    {
        private List<View> navigations = new List<View>();

        public NavigationPanel() { }

        public void Add(View navigation)
        {
            navigations.Add(navigation);
            this.Children.Add(navigation, navigations.Count - 1, 0);
        }
    }
}

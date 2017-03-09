using Xamarin.Forms;

namespace TravelAppMobile.Pages.Base
{
    public class BaseNavigationPage : NavigationPage
    {
        public BaseNavigationPage(Page page) : base(page)
        {
            BarBackgroundColor = Color.FromHex("#16ACE9");
            BarTextColor = Color.White;
        }
    }
}

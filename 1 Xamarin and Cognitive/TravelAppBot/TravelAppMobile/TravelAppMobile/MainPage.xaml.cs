using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravelAppMobile.Animations;
using TravelAppMobile.Pages;
using TravelAppMobile.Utils;
using TravelAppMobile.ViewModels;
using Xamarin.Forms;

namespace TravelAppMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            //Set label app name in middle
            LblAppName.TranslationY = ScreenSize.Height / 2;
            LblAppName.Scale = 2;
            GrdBookFlight.Scale = 0;

            base.OnAppearing();

            var viewModel = (MainPageViewModel)BindingContext;

            viewModel.ToggleMenuAction = () =>
            {
                if (MainViewGrid.TranslationX == 0)
                {
                    ToggleMenu(0, 210, 1, 0.6);
                }
                else
                {
                    ToggleMenu(210, 0, 0.6, 1);
                }
            };

            viewModel.BookFlightAction = () =>
            {
                Navigation.PushAsync(new TravelAgentPage());
            };

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                var animate1 = CommonAnimations.TransLateYAnimation(LblAppName, LblAppName.TranslationY, 0);
                var animate2 = CommonAnimations.ScaleAnimation(LblAppName, 2, 1);
                var animate3 = CommonAnimations.ScaleAnimation(GrdBookFlight, 0, 1);

                this.Animate("trany", animate1, 16, 600, Easing.BounceOut, (d, f) => { });
                this.Animate("scale", animate2, 16, 600, Easing.Linear, (d, f) =>
                {
                    this.Animate("bookanim", animate3, 16, 200, Easing.BounceOut, (x, y) => { });
                });

                return false;
            } );
        }

        private void ToggleMenu(double tranXFrom, double tranXTo, double scaleFrom, double scaleTo)
        {
            var animate1 = CommonAnimations.TransLateXAnimation(MainViewGrid, tranXFrom, tranXTo);
            var animate2 = CommonAnimations.ScaleAnimation(MainViewGrid, scaleFrom, scaleTo);

            this.Animate("tranx", animate1, 16, 200, Easing.Linear, (d, f) => { });
            this.Animate("scale", animate2, 16, 200, Easing.Linear, (d, f) => { });
        }

        private void Menu_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = (ListView)sender;

            ToggleMenu(210, 0, 0.6, 1);

            if (listView.SelectedItem != null)
                listView.SelectedItem = null;
            else
                return;


            var menu = listView.SelectedItem as Models.MenuItem;


            switch (menu.Title)
            {
                case "Language":
                    Navigation.PushAsync(new TravelAgentPage());
                    break;
                case "Vision":
                    Navigation.PushAsync(new OCRPage());
                    break;
            }
        }
    }
}

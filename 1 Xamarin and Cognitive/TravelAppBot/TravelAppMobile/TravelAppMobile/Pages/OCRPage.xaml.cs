using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAppMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelAppMobile.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OCRPage : ContentPage
    {
        public OCRPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as OCRPageViewModel;

            viewModel.OnErrorAction = (msg) =>
            {
                DisplayAlert("Something went Wrong", msg, "Ok");
            };
        }
    }
}

using MobileAppDev1.Services;
using MobileAppDev1.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppDev1
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            if (Settings.FirstRun)
            {
                DatabaseService.LoadSampleData();
                Settings.FirstRun = false;
            }

            var welcome = new TermsPage();
            var navPage = new NavigationPage(welcome);
            MainPage = navPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

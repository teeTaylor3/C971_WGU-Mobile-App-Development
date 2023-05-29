using MobileAppDev1.Models;
using MobileAppDev1.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppDev1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermListPage : ContentPage
    {
        public TermListPage()
        {
            InitializeComponent();

            // Style Changes
            PageTitle.TextColor = Color.FromHex("003057");
            AddTerm.BackgroundColor = Color.FromHex("327DA9");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            TermListView.ItemsSource = await DatabaseService.GetTerms();
        }

        private async void TermListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Term terms = (Term)e.CurrentSelection.FirstOrDefault();
                var termDetailsPage = new TermDetailsPage(terms);
                await Navigation.PushAsync(new TermDetailsPage(terms));
            }
        }

        private async void AddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTermPage());
        }
        private async void Home_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Home", "Return to Home?", "Yes", "No");
            if (result)
            {
                await Navigation.PushAsync(new TermListPage());
            }
        }

        private async void Profile_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Profile", "View Your Profile?", "Yes", "No");
            if (result)
            {
                await Navigation.PushAsync(new Profile());
            }
        }

        private async void Preferences_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Preferences", "View App Preferences?", "Yes", "No");
            if (result)
            {
                await Navigation.PushAsync(new Preferences());
            }
        }
    }
}
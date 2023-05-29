using MobileAppDev1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppDev1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTermPage : ContentPage
    {
        public AddTermPage()
        {
            InitializeComponent();
            // Style Changes
            PageTitle.TextColor = Color.FromHex("003057");
            SaveButton.BackgroundColor = Color.FromHex("42892E");
            CancelButton.BackgroundColor = Color.FromHex("F6BE00");

            // Validating Dates
            dateStartEntry.MinimumDate = new DateTime(2023, 01, 01);
            dateStartEntry.MaximumDate = new DateTime(2030, 06, 30);
            dateEndEntry.MinimumDate = new DateTime(2023, 07, 01);
            dateEndEntry.MaximumDate = new DateTime(2030, 12, 31);
        }

        // The following bool validates the entries
        private bool IsComplete()
        {
            if (string.IsNullOrEmpty(termNameEntry.Text))
            {
                return false;
            }
            else if (dateStartEntry.Date > dateEndEntry.Date)
            {
                DisplayAlert("Warning", "Start Date Must Be Before End Date", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (IsComplete() != true)
            {
                await DisplayAlert("Warning", "Check Fields", "OK");
            }
            else
            {
                await DatabaseService.AddTerm(termNameEntry.Text, dateStartEntry.Date, dateEndEntry.Date);
                await Navigation.PopAsync();
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Cancel", "Cancel Term Creation?", "Yes", "No");
            if (result)
            {
                await Navigation.PopAsync();
            }
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
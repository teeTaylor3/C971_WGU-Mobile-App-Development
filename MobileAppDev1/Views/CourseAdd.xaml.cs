using MobileAppDev1.Models;
using MobileAppDev1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppDev1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCoursePage : ContentPage
	{
        protected Term term;
        public AddCoursePage (Term terms)
		{
			InitializeComponent ();

            // Style Changes
            PageTitle.TextColor = Color.FromHex("003057");
            SaveButton.BackgroundColor = Color.FromHex("42892E");
            CancelButton.BackgroundColor = Color.FromHex("F6BE00");

            // Filling Form
            term = terms;
            termIDEntry.Text = term.TermID.ToString();
            dateStartEntry.MinimumDate = term.TermStart.Date;
            dateStartEntry.MaximumDate = term.TermEnd.Date;
            dateEndEntry.MinimumDate = term.TermEnd.Date;
            dateEndEntry.MaximumDate = term.TermEnd.Date;
        }
        
        // The following bools validates the entries
        private bool IsValidEmail()
        {
            var result = false;
            try
            {
                var r = new Regex("^\\S+@\\S+\\.\\S+$");

                if (r.IsMatch(courseIEEntry.Text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return result;
            }
        }

        private bool IsValidPhoneNumber()
        {
            var result = false;
            try
            {
                var r = new Regex("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}");

                if (r.IsMatch(courseIPNEntry.Text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return result;
            }
        }

        private bool IsComplete()
        {
            if (string.IsNullOrEmpty(courseNameEntry.Text))
            {
                return false;
            }
            else if (dateStartEntry.Date > dateEndEntry.Date)
            {
                DisplayAlert("Warning", "Start Date Must Be Before End Date", "OK");
                return false;
            }
            else if (courseStatus.SelectedIndex == -1)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(courseINEntry.Text) || string.IsNullOrEmpty(courseIPNEntry.Text) || string.IsNullOrEmpty(courseIEEntry.Text))
            {
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
                if (IsValidEmail() != true)
                {
                    await DisplayAlert("Warning", "Invalid Email Format", "OK");
                }
                else if (IsValidPhoneNumber() != true)
                {
                    await DisplayAlert("Warning", "Invalid Phone Number Format", "OK");
                }
                else
                {
                    int newID = Int32.Parse(termIDEntry.Text);
                    await DatabaseService.AddCourse(newID, courseNameEntry.Text, dateStartEntry.Date, dateEndEntry.Date, courseStatus.SelectedItem.ToString(),
                        courseNotesEntry.Text, notifications.IsToggled, courseINEntry.Text, courseIPNEntry.Text, courseIEEntry.Text);
                    await Navigation.PopAsync();
                }
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Cancel", "Cancel Course Creation?", "Yes", "No");
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
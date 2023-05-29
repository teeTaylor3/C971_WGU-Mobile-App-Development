using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileAppDev1.Models;
using MobileAppDev1.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppDev1.Views
{	
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAssessmentPage : ContentPage
	{
        protected Course course;
        public AddAssessmentPage (Course courses)
		{
			InitializeComponent ();

            // Style Changes
            PageTitle.TextColor = Color.FromHex("003057");
            SaveButton.BackgroundColor = Color.FromHex("42892E");
            CancelButton.BackgroundColor = Color.FromHex("F6BE00");

            // Filling Form
            course = courses;
            courseID.Text = course.CourseID.ToString();
            dateStartEntry.MinimumDate = course.CourseStart.Date;
            dateStartEntry.MaximumDate = course.CourseEnd.Date;
            dateEndEntry.MinimumDate = course.CourseEnd.Date;
            dateEndEntry.MaximumDate = course.CourseEnd.Date;
        }

        // The following bool validates the entries
        private bool IsComplete()
        {
            if (string.IsNullOrEmpty(assessmentEntry.Text))
            {
                return false;
            }
            else if (dateStartEntry.Date > dateEndEntry.Date)
            {
                DisplayAlert("Warning", "Start Date Must Be Before End Date", "OK");
                return false;
            }
            else if (assessmentType.SelectedIndex == -1)
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
                int newID = Int32.Parse(courseID.Text);
                await DatabaseService.AddAssessment(newID, assessmentEntry.Text, dateStartEntry.Date, dateEndEntry.Date, assessmentType.SelectedItem.ToString(), notifications.IsToggled);
                await Navigation.PopAsync();
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Cancel", "Cancel Assessment Creation?", "Yes", "No");
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
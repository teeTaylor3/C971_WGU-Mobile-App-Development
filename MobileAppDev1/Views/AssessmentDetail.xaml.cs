using MobileAppDev1.Models;
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
    public partial class AssessmentDetail : ContentPage
    {
        private readonly int currentAssessmentID;
        protected Assessment assessment;
        public AssessmentDetail(Assessment assessments)
        {
            InitializeComponent();

            // Style Changes
            PageTitle.TextColor = Color.FromHex("003057");
            notifications.OnColor = Color.FromHex("003057");
            notifications.ThumbColor = Color.FromHex("327DA9");
            EditButton.BackgroundColor = Color.FromHex("63666A");
            SaveButton.BackgroundColor = Color.FromHex("42892E");
            Done.BackgroundColor = Color.FromHex("42892E");
            CancelButton.BackgroundColor = Color.FromHex("F6BE00");

            // Filling Form
            assessment = assessments;
            PageTitle.Text = assessment.AssessmentName;
            currentAssessmentID = assessments.AssessmentID;
            assessmentID.Text = assessments.AssessmentID.ToString();
            courseID.Text = assessments.CourseID.ToString();
            assessmentEntry.Text = assessments.AssessmentName;
            dateStartEntry.Date = assessments.AssessmentStart;
            dateStartEntry.Format = "dddd, MMMM dd, yyyy";
            dateEndEntry.Date = assessments.AssessmentEnd;
            dateEndEntry.Format = "dddd, MMMM dd, yyyy";
            assessmentType.SelectedItem = assessments.AssessmentType;
            notifications.IsToggled = assessments.AssessmentNotifications;
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

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            // Converts page to allow editing.
            assessmentEntry.Focus();
            assessmentEntry.IsEnabled = true;
            dateStartEntry.IsEnabled = true;
            dateEndEntry.IsEnabled = true;
            assessmentType.IsEnabled = true;
            notifications.IsEnabled = true;
            EditButton.IsEnabled = false;
            EditButton.IsVisible = false;
            SaveButton.IsEnabled = true;
            SaveButton.IsVisible = true;
            Done.IsEnabled = false;
            Done.IsVisible = false;
            CancelButton.IsEnabled = true;
            CancelButton.IsVisible = true;
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Warning", "Remove Assessment?", "Yes", "No");
            if (result)
            {
                await DatabaseService.RemoveAssessment(currentAssessmentID);
                await DisplayAlert("Confirmed", "Assessment Removed", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Cancel", "Nothing Removed", "OK");
            }
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (IsComplete() != true)
            {
                await DisplayAlert("Warning", "Check Fields", "OK");
            }
            else
            {
                int newID = Int32.Parse(assessmentID.Text);
                await DatabaseService.UpdateAssessment(newID, assessmentEntry.Text, dateStartEntry.Date, dateEndEntry.Date, assessmentType.SelectedItem.ToString(), notifications.IsToggled)
                    ;
                var result = await DisplayAlert("Complete", "Return Previous Page?", "Yes", "No");
                if (result)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    // Reverts page to view only.
                    PageTitle.Focus();
                    assessmentEntry.IsEnabled = false;
                    dateStartEntry.IsEnabled = false;
                    dateEndEntry.IsEnabled = false;
                    assessmentType.IsEnabled = false;
                    notifications.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    SaveButton.IsVisible = false;
                    EditButton.IsEnabled = true;
                    EditButton.IsVisible = true;
                    Done.IsEnabled = true;
                    Done.IsVisible = true;
                    CancelButton.IsEnabled = false;
                    CancelButton.IsVisible = false;
                }
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Cancel", "Cancel Assessment Modifications?", "Yes", "No");
            if (result)
            {
                // Reverts page to view only.
                assessmentEntry.IsEnabled = false;
                dateStartEntry.IsEnabled = false;
                dateEndEntry.IsEnabled = false;
                assessmentType.IsEnabled = false;
                notifications.IsEnabled = false;
                SaveButton.IsEnabled = false;
                SaveButton.IsVisible = false;
                EditButton.IsEnabled = true;
                EditButton.IsVisible = true;
                Done.IsEnabled = true;
                Done.IsVisible = true;
                CancelButton.IsEnabled = false;
                CancelButton.IsVisible = false;
            }
        }

        private async void Home_Clicked(object sender, EventArgs e)
        {
            if (SaveButton.IsEnabled ==true)
            {
                var result = await DisplayAlert("Home", "Return to Home Without Saving?", "Yes", "No");
                if (result)
                {
                    await Navigation.PushAsync(new TermListPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new TermListPage());
            }
        }

        private async void Profile_Clicked(object sender, EventArgs e)
        {
            if (SaveButton.IsEnabled == true)
            {
                var result = await DisplayAlert("Profile", "View Your Profile Without Saving?", "Yes", "No");
                if (result)
                {
                    await Navigation.PushAsync(new Profile());
                }
            }
            else
            {
                await Navigation.PushAsync(new TermListPage());
            }
        }

        private async void Preferences_Clicked(object sender, EventArgs e)
        {
            if (SaveButton.IsEnabled == true)
            {
                var result = await DisplayAlert("Preferences", "View App Preferences Without Saving?", "Yes", "No");
                if (result)
                {
                await Navigation.PushAsync(new Preferences());
                }
            }
            else
            {
                await Navigation.PushAsync(new TermListPage());
            }
}
    }
}
using MobileAppDev1.Models;
using MobileAppDev1.Services;
using MvvmHelpers;
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

    public partial class TermDetailsPage : ContentPage
	{
        private readonly int currentTermID;
        protected Term term;
        public TermDetailsPage (Term terms)
		{
			InitializeComponent ();
            // Style Changes
            PageTitle.TextColor = Color.FromHex("003057");
            AddButton.BackgroundColor = Color.FromHex("327DA9");
            EditButton.BackgroundColor = Color.FromHex("63666A");
            SaveButton.BackgroundColor = Color.FromHex("42892E");
            Done.BackgroundColor = Color.FromHex("42892E");
            CancelButton.BackgroundColor = Color.FromHex("F6BE00");

            // Filling Form
            term = terms;
            PageTitle.Text = terms.TermName;
            currentTermID = terms.TermID;
            termID.Text = terms.TermID.ToString();
            termEntry.Text = terms.TermName.ToString();
            dateStartEntry.Date = terms.TermStart;
            dateStartEntry.Format = "dddd, MMMM dd, yyyy";
            dateStartEntry.MinimumDate = new DateTime(2023, 01, 01);
            dateStartEntry.MaximumDate = new DateTime(2030, 06, 30);
            dateEndEntry.Date = terms.TermEnd;
            dateEndEntry.Format = "dddd, MMMM dd, yyyy";
            dateEndEntry.MinimumDate = new DateTime(2023, 07, 01);
            dateEndEntry.MaximumDate = new DateTime(2030, 12, 31);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CourseListView.ItemsSource = await DatabaseService.GetCourses(currentTermID);
        }

        // The following bool validates the entries
        private bool IsComplete()
        {
            if (string.IsNullOrEmpty(termEntry.Text))
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

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            // Converts page to allow editing.
            termEntry.Focus();
            termEntry.IsEnabled = true;
            dateStartEntry.IsEnabled = true;
            dateEndEntry.IsEnabled = true;
            EditButton.IsEnabled = false;
            EditButton.IsVisible = false;
            SaveButton.IsEnabled = true;
            SaveButton.IsVisible = true;
            Done.IsEnabled = false;
            Done.IsVisible = false;
            CancelButton.IsEnabled = true;
            CancelButton.IsVisible = true;
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void CourseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Course courses = (Course)e.CurrentSelection.FirstOrDefault();
                var courseDetailsPage = new CourseDetail(courses);
                await Navigation.PushAsync(new CourseDetail(courses));
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Warning", "Remove Term and Related Courses?", "Yes", "No");
            if (result)
            {
                await DatabaseService.RemoveTerm(currentTermID);
                await DisplayAlert("Confirmed", "Term Removed", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Cancel", "Nothing Removed", "OK");
            }
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCoursePage(term));
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (IsComplete() != true)
            {
                await DisplayAlert("Warning", "Check Fields", "OK");
            }
            else
            {
                await DatabaseService.UpdateTerm(currentTermID, termEntry.Text, dateStartEntry.Date, dateEndEntry.Date);
                var result = await DisplayAlert("Complete", "Return Previous Page?", "Yes", "No");
                if (result)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    PageTitle.Focus();
                    termEntry.IsEnabled = false;
                    dateStartEntry.IsEnabled = false;
                    dateEndEntry.IsEnabled = false;
                    EditButton.IsEnabled = true;
                    EditButton.IsVisible = true;
                    SaveButton.IsEnabled = false;
                    SaveButton.IsVisible = false;
                    Done.IsEnabled = true;
                    Done.IsVisible = true;
                    CancelButton.IsEnabled = false;
                    CancelButton.IsVisible = false;
                }
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Cancel", "Cancel Term Modifications?", "Yes", "No");
            if (result)
            {
                // Reverts page to view only.
                termEntry.IsEnabled = false;
                dateStartEntry.IsEnabled = false;
                dateEndEntry.IsEnabled = false;
                EditButton.IsEnabled = true;
                EditButton.IsVisible = true;
                SaveButton.IsEnabled = false;
                SaveButton.IsVisible = false;
                Done.IsEnabled = true;
                Done.IsVisible = true;
                CancelButton.IsEnabled = false;
                CancelButton.IsVisible = false;
            }
        }

        private async void Home_Clicked(object sender, EventArgs e)
        {
            if (SaveButton.IsEnabled == true)
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
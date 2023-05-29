using MobileAppDev1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MobileAppDev1.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using MobileAppDev1.Views;

namespace MobileAppDev1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseDetail : ContentPage
    {
        private readonly int currentCourseID;
        protected Course course;
        protected Term term;
        public CourseDetail(Course courses)
        {
            InitializeComponent();
            // Style Changes
            PageTitle.TextColor = Color.FromHex("003057");
            notifications.OnColor = Color.FromHex("003057");
            notifications.ThumbColor = Color.FromHex("327DA9");
            AddButton.BackgroundColor = Color.FromHex("327DA9");
            EditButton.BackgroundColor = Color.FromHex("63666A");
            SaveButton.BackgroundColor = Color.FromHex("42892E");
            Done.BackgroundColor = Color.FromHex("42892E");
            CancelButton.BackgroundColor = Color.FromHex("F6BE00");

            // Filling Form
            course = courses;
            PageTitle.Text = courses.CourseName;
            currentCourseID = courses.CourseID;
            courseID.Text = courses.CourseID.ToString();
            termIDEntry.Text = courses.TermID.ToString();
            courseNameEntry.Text = courses.CourseName;
            dateStartEntry.Date = courses.CourseStart;
            dateStartEntry.Format = "dddd, MMMM dd, yyyy";
            dateEndEntry.Date = courses.CourseEnd;
            dateEndEntry.Format = "dddd, MMMM dd, yyyy";
            courseStatus.SelectedItem = course.CourseStatus;
            courseNotesEntry.Text = courses.CourseNotes;
            notifications.IsToggled = courses.CourseNotifications;
            courseINEntry.Text = courses.CourseInstructorName;
            courseIPNEntry.Text = courses.CourseInstructorPhoneNumber;
            courseIEEntry.Text = courses.CourseInstructorEmail;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            AssessmentListView.ItemsSource = await DatabaseService.GetAssessments(currentCourseID);
        }

        private async void AssessmentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Assessment assessments = (Assessment)e.CurrentSelection.FirstOrDefault();
                var courseDetailsPage = new AssessmentDetail(assessments);
                await Navigation.PushAsync(new AssessmentDetail(assessments));
            }
        }

        // The following bool validates the entries
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
                var r = new Regex("^(\\+\\d{1,2}\\s?)?1?\\-?\\.?\\s?\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{4}$");

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

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            // Converts page to allow editing.
            courseNameEntry.Focus();
            courseNameEntry.IsEnabled = true;
            dateStartEntry.IsEnabled = true;
            dateEndEntry.IsEnabled = true;
            courseStatus.IsEnabled = true;
            courseNotesEntry.IsEnabled = true;
            notifications.IsEnabled = true;
            courseINEntry.IsEnabled = true;
            courseIPNEntry.IsEnabled = true;
            courseIEEntry.IsEnabled = true;
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
            var result = await DisplayAlert("Warning", "Remove Course and Related Assessments?", "Yes", "No");
            if (result)
            {
                await DatabaseService.RemoveCourse(currentCourseID);
                await DisplayAlert("Confirmed", "Course Removed", "OK");
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

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAssessmentPage(course));
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
                    int newID = Int32.Parse(courseID.Text);
                    await DatabaseService.UpdateCourse(newID, courseNameEntry.Text, dateStartEntry.Date, dateEndEntry.Date, courseStatus.SelectedItem.ToString(),
                        courseNotesEntry.Text, notifications.IsToggled, courseINEntry.Text, courseIPNEntry.Text, courseIEEntry.Text);
                    var result = await DisplayAlert("Complete", "Return Previous Page?", "Yes", "No");
                    if (result)
                    {
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        // Reverts page to view only.
                        PageTitle.Focus();
                        courseNameEntry.IsEnabled = false;
                        dateStartEntry.IsEnabled = false;
                        dateEndEntry.IsEnabled = false;
                        courseStatus.IsEnabled = false;
                        courseNotesEntry.IsEnabled = false;
                        notifications.IsEnabled = false;
                        courseINEntry.IsEnabled = false;
                        courseIPNEntry.IsEnabled = false;
                        courseIEEntry.IsEnabled = false;
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
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Cancel", "Cancel Course Modifications?", "Yes", "No");
            if (result)
            {
                // Reverts page to view only.
                courseNameEntry.IsEnabled = false;
                dateStartEntry.IsEnabled = false;
                dateEndEntry.IsEnabled = false;
                courseStatus.IsEnabled = false;
                courseNotesEntry.IsEnabled = false;
                notifications.IsEnabled = false;
                courseINEntry.IsEnabled = false;
                courseIPNEntry.IsEnabled = false;
                courseIEEntry.IsEnabled = false;
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

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(courseNotesEntry.Text))
            {
                await DisplayAlert("Warning", "No Notes To Share.", "OK");
            }
            else
            {
                await Share.RequestAsync(new ShareTextRequest { Text = courseNotesEntry.Text, Title = "Share Text" });
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
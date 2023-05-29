using MobileAppDev1.Models;
using MobileAppDev1.Services;
using Plugin.LocalNotifications;
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
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();

            // Style Change
            WelcomePage.BackgroundColor = Color.FromHex("003057");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await WelcomeMessage.FadeTo(1, 3000, Easing.SinIn);

            var courseList = await DatabaseService.GetCourses();
            var assessmentList = await DatabaseService.GetAssessments();
            var notifyRandom = new Random();
            var notifyId = notifyRandom.Next(1000);

            foreach (Course courseRecord in courseList)
            {
                if (courseRecord.CourseNotifications == true)
                {
                    if (courseRecord.CourseStart == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.CourseName} begins today!", notifyId);
                    }
                    else if (courseRecord.CourseEnd == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.CourseName} ends today!", notifyId);
                    }
                }
            }

            foreach (Assessment assessmentRecord in assessmentList)
            {
                if (assessmentRecord.AssessmentNotifications == true)
                {
                    if (assessmentRecord.AssessmentEnd == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{assessmentRecord.AssessmentName} is due today!", notifyId);
                    }
                }
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermListPage());
        }
    }
}
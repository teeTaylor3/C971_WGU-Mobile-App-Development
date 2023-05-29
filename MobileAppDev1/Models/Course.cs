using MvvmHelpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MobileAppDev1.Models
{
    public  class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseID { get; set; }
        public int TermID { get; set; } //Foreign Key for Term class/table
        public string CourseName { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
        public string CourseStatus { get; set; }
        public string CourseNotes { get; set; }
        public bool CourseNotifications { get; set; }
        public string CourseInstructorName { get; set; }
        public string CourseInstructorPhoneNumber { get; set; }
        public string CourseInstructorEmail { get; set; }
        //public ObservableCollection<Assessment> AssessmentList { get; set; } = new ObservableCollection<Assessment>();
    }
}

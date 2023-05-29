using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using SQLite;

namespace MobileAppDev1.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermID { get; set; }
        public string TermName { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
        //public ObservableCollection<Course> CourseList { get; set; } = new ObservableCollection<Course>();

    }
}

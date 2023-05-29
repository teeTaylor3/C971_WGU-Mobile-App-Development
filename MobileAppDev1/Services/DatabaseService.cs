using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MobileAppDev1.Models;
using MobileAppDev1.Views;

namespace MobileAppDev1.Services
{
    public static class DatabaseService
    {

        public static SQLiteAsyncConnection db;
        public static SQLiteConnection dbConnection;

        // Creates a connection to a local database
        static async Task Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TermAppDatabase.db");

            db = new SQLiteAsyncConnection(databasePath);
            dbConnection = new SQLiteConnection(databasePath);

            await db.CreateTableAsync<Term>();
            await db.CreateTableAsync<Course>();
            await db.CreateTableAsync<Assessment>();
        }

        #region Term Methods
        // Term CRUD
        public static async Task AddTerm(string name, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term
            {
                TermName = name,
                TermStart = start,
                TermEnd = end
            };

            var id = await db.InsertAsync(term);
        }

        public static async Task<IEnumerable<Term>> GetTerms(int termId)
        {
            await Init();

            var terms = await db.Table<Term>().Where(i => i.TermID == termId).ToListAsync();

            return terms;
        }

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();

            var terms = await db.Table<Term>().ToListAsync();

            return terms;
        }

        public static async Task UpdateTerm(int id, string name, DateTime start, DateTime end)
        {
            await Init();

            var termQuery = await db.Table<Term>()
                .Where(i => i.TermID == id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.TermName = name;
                termQuery.TermStart = start;
                termQuery.TermEnd = end;

                await db.UpdateAsync(termQuery);
            }

        }

        public static async Task RemoveTerm(int id)
        {
            await Init();
            await db.DeleteAsync<Term>(id);
        }
        #endregion



        #region Course Methods
        // Course CRUD
        public static async Task AddCourse(int termId, string name, DateTime start, DateTime end, string status, string notes,
            bool notifications, string instructorName, string instructorNumber, string instructorEmail)
        {
            await Init();
            var course = new Course()
            {
                TermID = termId,
                CourseName = name,
                CourseStart = start,
                CourseEnd = end,
                CourseStatus = status,
                CourseNotes = notes,
                CourseNotifications = notifications,
                CourseInstructorName = instructorName,
                CourseInstructorPhoneNumber = instructorNumber,
                CourseInstructorEmail = instructorEmail
            };

            var id = await db.InsertAsync(course);
        }

        public static async Task<IEnumerable<Course>> GetCourses(int termId)
        {
            await Init();

            var courses = await db.Table<Course>().Where(i => i.TermID == termId).ToListAsync();

            return courses;
        }

        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();

            var courses = await db.Table<Course>().ToListAsync();

            return courses;
        }

        public static async Task UpdateCourse(int id, string name, DateTime start, DateTime end, string status, string notes,
            bool notifications, string instructorName, string instructorNumber, string instructorEmail)
        {
            await Init();

            var courseQuery = await db.Table<Course>()
                .Where(i => i.CourseID == id)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.CourseName = name;
                courseQuery.CourseStart = start;
                courseQuery.CourseEnd = end;
                courseQuery.CourseStatus = status;
                courseQuery.CourseNotes = notes;
                courseQuery.CourseNotifications = notifications;
                courseQuery.CourseInstructorName = instructorName;
                courseQuery.CourseInstructorPhoneNumber = instructorNumber;
                courseQuery.CourseInstructorEmail = instructorEmail;

                await db.UpdateAsync(courseQuery);
            }
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            await db.DeleteAsync<Course>(id);
        }
        #endregion



        #region Assessment Methods
        // Assessment CRUD
        public static async Task AddAssessment(int courseId, string name, DateTime start,
            DateTime end, string type, bool notifications)
        {
            await Init();
            var assessment = new Assessment()
            {
                CourseID = courseId,
                AssessmentName = name,
                AssessmentStart = start,
                AssessmentEnd = end,
                AssessmentType = type,
                AssessmentNotifications = notifications
            };

            var id = await db.InsertAsync(assessment);
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments(int courseId)
        {
            await Init();

            var assessments = await db.Table<Assessment>().Where(i => i.CourseID == courseId).ToListAsync();

            return assessments;
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments()
        {
            await Init();

            var assessments = await db.Table<Assessment>().ToListAsync();

            return assessments;
        }

        public static async Task UpdateAssessment(int id, string name, DateTime start, DateTime end,
            string type, bool notifications)
        {
            await Init();

            var assessmentQuery = await db.Table<Assessment>()
                .Where(i => i.AssessmentID == id)
                .FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.AssessmentName = name;
                assessmentQuery.AssessmentStart = start;
                assessmentQuery.AssessmentEnd = end;
                assessmentQuery.AssessmentType = type;
                assessmentQuery.AssessmentNotifications = notifications;

                await db.UpdateAsync(assessmentQuery);
            }
        }

        public static async Task RemoveAssessment(int id)
        {
            await Init();

            await db.DeleteAsync<Assessment>(id);
        }
        #endregion

        #region DemoData

        public static async void LoadSampleData()
        {
            await Init();
  
            #region Term Data

            Term term = new Term
            {
                TermName = "Testing Term",
                TermStart = new DateTime(2023, 03, 01),
                TermEnd = new DateTime(2023, 09, 30)
            };

            await db.InsertAsync(term);

            #endregion

            #region Course Data

            Course course = new Course
            {
                CourseName = "Testing Course",
                CourseStart = term.TermStart,
                CourseEnd = term.TermEnd,
                CourseStatus = "In Progress",
                CourseNotes = "THIS IS FOR TESTING PURPOSES ONLY",
                CourseNotifications = false,
                CourseInstructorName = "Terrence Taylor",
                CourseInstructorPhoneNumber = "504-000-0000",
                CourseInstructorEmail = "ttay614@wgu.edu",
                TermID = term.TermID
            };

            await db.InsertAsync(course);

            #endregion

            #region Assessment Data

            Assessment assessment = new Assessment
            {
                AssessmentName = "Assessment 1",
                AssessmentStart = course.CourseStart,
                AssessmentEnd = course.CourseEnd,
                AssessmentType = "Performance",
                AssessmentNotifications = true,
                CourseID = course.CourseID
            };

            await db.InsertAsync(assessment);

            Assessment assessment2 = new Assessment
            {
                AssessmentName = "Assessment 2",
                AssessmentStart = course.CourseStart,
                AssessmentEnd = course.CourseEnd,
                AssessmentType = "Objective",
                AssessmentNotifications = false,
                CourseID = course.CourseID
            };

            await db.InsertAsync(assessment2);

            #endregion
        }

        public static async void RecreateTables()
        {
            await Init();

            await db.CreateTableAsync<Term>();
            await db.CreateTableAsync<Course>();
            await db.CreateTableAsync<Assessment>();
        }

        public static async void ClearTables()
        {
            await Init();

            await db.DeleteAllAsync<Term>();
            await db.DeleteAllAsync<Course>();
            await db.DeleteAllAsync<Assessment>();
        }

        public static async Task ClearSampleData()
        {
            await Init();

            await db.DropTableAsync<Assessment>();
            await db.DropTableAsync<Course>();
            await db.DropTableAsync<Term>();
            db = null;
            dbConnection = null;
        }
        #endregion
    }
}

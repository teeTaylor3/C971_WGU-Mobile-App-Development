using MobileAppDev1.Views;
using Xamarin.Forms;

namespace MobileAppDev1
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TermListPage),
               typeof(TermListPage));

            Routing.RegisterRoute(nameof(TermDetailsPage), 
                typeof(TermDetailsPage));

            Routing.RegisterRoute(nameof(AddTermPage),
             typeof(AddTermPage));

            Routing.RegisterRoute(nameof(AddCoursePage),
             typeof(AddCoursePage));

            Routing.RegisterRoute(nameof(AddAssessmentPage),
                typeof(AddAssessmentPage));
        }


    }
}

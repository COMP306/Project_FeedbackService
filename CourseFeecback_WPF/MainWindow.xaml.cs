using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography.X509Certificates;
using CourseFeecback_WPF.FeedbackService;

namespace CourseFeecback_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PermissiveCertificatePolicy.Enact("CN=HTTPS-Server");
           InitializeComponent();
             CourseObject[]  test = getAllCourseList();
        }

        private CourseObject[] getAllCourseList()
        {
            FeedbackServiceClient aClient = new FeedbackServiceClient("WS2007HttpBinding_IFeedbackService");
            aClient.ClientCredentials.Windows.ClientCredential.Domain = "JohnnyWalker";
            aClient.ClientCredentials.Windows.ClientCredential.UserName = "student";
            aClient.ClientCredentials.Windows.ClientCredential.Password = "password";
            return aClient.GetAllCourse();
        }
    }

    
}

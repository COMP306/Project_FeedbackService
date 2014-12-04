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
using System.ServiceModel;

namespace CourseFeecback_WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void cBtnSave_Click_Save(object sender, RoutedEventArgs e)
        {

            //after login

            FeedbackServiceClient aClient = setCredential();
            List<CourseObject> courseList = null;
            try
            {
                var retCourse = aClient.GetAllCourse();

                if (retCourse.Length > 0) // fake... just assume that database must have something inside..
                {
                    courseList = new List<CourseObject>();
                }
            }
            catch (FaultException<FaultFeedbackInfo> ef)
            {
                MessageBox.Show(string.Format("\nFaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", ef.Detail.Source, ef.Detail.Reason));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service Error !!!! ");
            }

            if (courseList != null)
            {

                DataEnv.username = cTboxUsername.Text;
                DataEnv.password = cTboxPassword.Password;
                Login obj = new Login();
                MainWindow objmain = new MainWindow();
                objmain.Show(); //after login Redirect to second window  
                obj.Hide();//after login hide the  Login window  
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
        }

        private FeedbackServiceClient setCredential()
        {
            FeedbackServiceClient aClient = new FeedbackServiceClient("WS2007HttpBinding_IFeedbackService");
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) => { return true; };
            aClient.ClientCredentials.Windows.ClientCredential.Domain = Environment.UserDomainName; // "AlexLiu-PC";
            aClient.ClientCredentials.Windows.ClientCredential.UserName = cTboxUsername.Text;
            aClient.ClientCredentials.Windows.ClientCredential.Password = cTboxPassword.Password;
            return aClient;
        }


    }
}

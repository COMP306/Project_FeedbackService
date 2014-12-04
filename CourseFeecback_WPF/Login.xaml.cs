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
        FeedbackServiceClient aClient;

        public Login()
        {
            InitializeComponent();
        }

        #region "On Load"
        void OnLoad(object sender, RoutedEventArgs e)
        {
            this.cTboxDomainname.Text = Environment.UserDomainName;

            try
            {
                aClient = ServiceHelper.setCredential(true);
                LoginCurrentUserBtn.Content = "Use Current Login : " + aClient.GetCurrentUser();
                LoginCurrentUserBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LoginCurrentUserBtn.Content = "";
                LoginCurrentUserBtn.IsEnabled = false;
            }
            finally
            {
                aClient.Close();
            }
        }
        #endregion

        #region "Curent User Login"
        private void cBtnCurrentUser_Click(object sender, RoutedEventArgs e)
        {
            DataEnv.useCurrentLogin = true;
            MainWindow objmain = new MainWindow();
            objmain.Show();     //  Redirect to Mian window  
            this.Close();   //  close Login window  
        }
        #endregion 

        #region "Login"
        private void cBtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //after login
            DataEnv.useCurrentLogin = false;
            aClient = ServiceHelper.setCredential();
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
                MessageBox.Show(string.Format("FaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", ef.Detail.Source, ef.Detail.Reason));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service Error !!!! " + ex.Message);
            }
            finally
            {
                aClient.Close();
            }

            if (courseList != null)
            {
                DataEnv.domain = cTboxDomainname.Text;
                DataEnv.username = cTboxUsername.Text;
                DataEnv.password = cTboxPassword.Password;
               // Login obj = new Login();
                MainWindow objmain = new MainWindow();
                objmain.Show(); //after login Redirect to second window  
                this.Close();//after login hide and close the  Login window  
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
        }
        #endregion




    }
}

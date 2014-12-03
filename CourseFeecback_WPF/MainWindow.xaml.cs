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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            PermissiveCertificatePolicy.Enact("CN=HTTPS-Server");
            InitializeComponent();
            initListBox();
            ListComment_Scrolldd.Visibility = Visibility.Collapsed;

        }

        enum saveStatus
        {
            newComment,
            updateComemnt
        }

        private void initListBox()
        {
            if (courseListBox.Items.Count != 0)
            {
                courseListBox.Items.Clear();
            }
            foreach (CourseObject co in getAllCourseList())
            {
                string courseName = co.Title;
                string coueseCode = co.Code;

                courseListBox.Items.Add(co.Code + " " + co.Title);
            }
        }

        #region private lists
        private List<CourseObject> currentCourseList = new List<CourseObject>();
        private int indexOfCourseList = -1;
        private int indexOfCommentList = -1;
        private saveStatus ss = saveStatus.newComment;
        private List<FeedbackObject> currentFeedbackList = new List<FeedbackObject>();
        #endregion private lists

        #region call services

        private FeedbackServiceClient setCredential()
        {
            FeedbackServiceClient aClient = new FeedbackServiceClient("WS2007HttpBinding_IFeedbackService");
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) => { return true; };
            aClient.ClientCredentials.Windows.ClientCredential.Domain = Environment.UserDomainName; // "AlexLiu-PC";
            aClient.ClientCredentials.Windows.ClientCredential.UserName = DataEnv.username;
            aClient.ClientCredentials.Windows.ClientCredential.Password = DataEnv.password;
            return aClient;
        }

        private List<CourseObject> getAllCourseList()
        {
            FeedbackServiceClient aClient = setCredential();
            List<CourseObject> courseList = new List<CourseObject>();
            try
            {
                var retCourse = aClient.GetAllCourse();

                foreach (var c in retCourse)
                {
                    courseList.Add(c);
                }
                currentCourseList = courseList;
            }
            catch (FaultException<FaultFeedbackInfo> e)
            {
                MessageBox.Show(string.Format("\nFaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service Error !!!! ");
            }
            return courseList;
        }

        private List<CourseObject> GetCourseByCodeOrTitle(string titleOrCode)
        {
            FeedbackServiceClient aClient = setCredential();
            List<CourseObject> courseList = new List<CourseObject>();
            try
            { //FaultFeedbackInfo
                var retCourse = aClient.GetCourseByCodeOrTitle(titleOrCode);

                foreach (var c in retCourse)
                {
                    courseList.Add(c);
                }
                currentCourseList = courseList;
            }
            catch (FaultException<FaultFeedbackInfo> e)
            {
                MessageBox.Show(string.Format("\nFaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service Error !!!! ");
            }
            return courseList;
        }
        private int UpdateByFeedBackID(int id, string content)
        {
            FeedbackServiceClient aClient = setCredential();
            int ret = -1;
            try
            {
                ret = aClient.UpdateByFeedBackID(id, content);
            }
            catch (FaultException<FaultFeedbackInfo> e)
            {
                MessageBox.Show(string.Format("\nFaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service Error !!!! ");
            }
            return ret;
        }

        private int PostFeedbackByCourseID(int courseId, string feedbackContent)
        {
            FeedbackServiceClient aClient = setCredential();
            int ret = -1;
            try
            {
                ret = aClient.PostFeedbackByCourseID(courseId, feedbackContent, ckAnoy.IsChecked.Value); ;
            }
            catch (FaultException<FaultFeedbackInfo> e)
            {
                MessageBox.Show(string.Format("\nFaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service Error !!!! ");
            }
            return ret;
        }
        private List<FeedbackObject> GetFeedbackByCourseID(int courseID)
        {
            FeedbackServiceClient aClient = setCredential();
            List<FeedbackObject> feedbaclList = new List<FeedbackObject>();

            try
            {
                var retFeedbacks = aClient.GetFeedbackByCourseID(courseID);
                foreach (var c in retFeedbacks)
                {
                    feedbaclList.Add(c);
                }
                currentFeedbackList = feedbaclList;
            }
            catch (FaultException<FaultFeedbackInfo> e)
            {
                MessageBox.Show(string.Format("\nFaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service Error !!!! ");
            }

            return feedbaclList;
        }
        private string getCurrentUser()
        {
            return string.Empty;
        }

        #endregion call services

        #region button clicks

        /// <summary>
        /// Search courses by name or code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            if (tbCourseName.Text == string.Empty)
            {
                MessageBox.Show("Please input course code of course name");
            }
            else
            {
                courseListBox.Items.Clear();
                List<CourseObject> searchResult = GetCourseByCodeOrTitle(tbCourseName.Text);
                if (searchResult.Count > 0)
                {
                    foreach (CourseObject co in searchResult)
                    {
                        string courseName = co.Title;
                        string coueseCode = co.Code;

                        courseListBox.Items.Add(co.Code + " " + co.Title);
                    }
                }
                else
                {
                    courseListBox.ItemsSource = null;
                }
            }
        }
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            tbCourseName.Text = String.Empty;
            this.initListBox();
            indexOfCourseList = -1;

            lbFeedback.ItemsSource = null;
            ListComment_Scrolldd.Visibility = Visibility.Hidden ;

        }

        /// <summary>
        /// Add a new Comment;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_NewComment(object sender, RoutedEventArgs e)
        {
            ss = saveStatus.newComment;
            if (indexOfCourseList == -1)
            {
                MessageBox.Show("Please select a course");
            }
            else
            {
                if (ListComment_Scrolldd.Visibility == Visibility.Visible)
                {
                    cTboxComment.Text = string.Empty;
                    cTboxUsername.Text = string.Empty;
                }
                ListComment_Scrolldd.Visibility = Visibility.Visible;
            }
        }


        private void cBtnSave_Click_Save(object sender, RoutedEventArgs e)
        {
            FeedbackObject fo = new FeedbackObject();
            if (indexOfCourseList == -1)
            {
                MessageBox.Show("Please select a course ");
                return;
            }
            CourseObject co = currentCourseList[indexOfCourseList];
            fo = currentFeedbackList[indexOfCommentList]; 
            String comment = cTboxComment.Text.ToString();
            
            if (ss == saveStatus.newComment) // post a  new comment
            {
                PostFeedbackByCourseID(co.ID, comment);
            }
            else // update an old comment
            {
                UpdateByFeedBackID(fo.ID, comment);
            }

            List<FeedbackObject> courseFeedback = GetFeedbackByCourseID(co.ID);
            foreach (var c in courseFeedback)
            {
                lbFeedback.ItemsSource = courseFeedback;
            }
        }

        #endregion button clicks



        private void ckAnoy_Click_1(object sender, RoutedEventArgs e)
        {
            if (ckAnoy.IsChecked.Value)
            {
                cTboxUsername.Text = string.Empty;
            }
            ckAnoy.IsChecked = ckAnoy.IsChecked.Value;
        }

        private void courseListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ss = saveStatus.updateComemnt;
            if (courseListBox.SelectedIndex != -1)
            {
                indexOfCourseList = courseListBox.SelectedIndex;
                CourseObject co = currentCourseList[courseListBox.SelectedIndex];
                List<FeedbackObject> courseFeedback = GetFeedbackByCourseID(co.ID);
                foreach (var c in courseFeedback)
                {
                    lbFeedback.ItemsSource = courseFeedback;
                }
            }
        }

        private void lbFeedback_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ss = saveStatus.updateComemnt;
            if (lbFeedback.SelectedIndex != -1)
            {
                indexOfCommentList = lbFeedback.SelectedIndex;
                FeedbackObject fo = currentFeedbackList[lbFeedback.SelectedIndex];
                cTboxUsername.Text = fo.StudentID.ToString();
                cTboxComment.Text = fo.Content.ToString();
                ckAnoy.IsChecked = fo.IsAnonymous;
                if (fo.IsAnonymous)
                {
                    cTboxUsername.Text = string.Empty;
                }
                ListComment_Scrolldd.Visibility = Visibility.Visible;
            }
        }
    }
}
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
            foreach (CourseObject co in getAllCourseList())
            {
                string courseName = co.Title;
                string coueseCode = co.Code;

                courseListBox.Items.Add(co.Code + " " + co.Title);
            }
        }

        #region private lists
        private List<CourseObject> currentCourseList = new List<CourseObject>();
        private int indexOfCourseList = 0;
        private int indexOfCommentList = 0;
        private saveStatus ss = saveStatus.newComment;
        private List<FeedbackObject> currentFeedbackList = new List<FeedbackObject>();
        #endregion private lists

        #region call services

        private FeedbackServiceClient setCredential()
        {
            FeedbackServiceClient aClient = new FeedbackServiceClient("WS2007HttpBinding_IFeedbackService");
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) => { return true; };
            aClient.ClientCredentials.Windows.ClientCredential.Domain = "AlexLiu-PC";
            aClient.ClientCredentials.Windows.ClientCredential.UserName = "alexliu";
            aClient.ClientCredentials.Windows.ClientCredential.Password = "Password";
            return aClient;
        }

        private List<CourseObject> getAllCourseList()
        {
            FeedbackServiceClient aClient = setCredential();
            var retCourse = aClient.GetAllCourse();
            List<CourseObject> courseList = new List<CourseObject>();
            foreach (var c in retCourse)
            {
                courseList.Add(c);
            }
            currentCourseList = courseList;
            return courseList;
        }

        private List<CourseObject> GetCourseByCodeOrTitle(string titleOrCode)
        {
            FeedbackServiceClient aClient = setCredential();
            var retCourse = aClient.GetCourseByCodeOrTitle(titleOrCode);
            List<CourseObject> courseList = new List<CourseObject>();
            foreach (var c in retCourse)
            {
                courseList.Add(c);
            }
            currentCourseList = courseList;
            return courseList;
        }
        private int UpdateByFeedBackID(int id, string content)
        {
            FeedbackServiceClient aClient = setCredential();
            return aClient.UpdateByFeedBackID(id, content);
        }

        private int PostFeedbackByCourseID(FeedbackObject feedback)
        {
            FeedbackServiceClient aClient = setCredential();
            return aClient.PostFeedbackByCourseID(feedback);
        }
        private List<FeedbackObject> GetFeedbackByCourseID(int courseID)
        {
            FeedbackServiceClient aClient = setCredential();
            List<FeedbackObject> feedbaclList = new List<FeedbackObject>();
            var retFeedbacks = aClient.GetFeedbackByCourseID(courseID);
            foreach (var c in retFeedbacks)
            {
                feedbaclList.Add(c);
            }
            currentFeedbackList = feedbaclList;
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
                //show alert
                //   MessageBox.Show("The Selected Index is" + listBox1.SelectedIndex);
            }
            else {
                courseListBox.Items.Clear();
                foreach (CourseObject co in GetCourseByCodeOrTitle(tbCourseName.Text))
                {
                    string courseName = co.Title;
                    string coueseCode = co.Code;

                    courseListBox.Items.Add(co.Code + " " + co.Title);
                }
            }
        }

        /// <summary>
        /// Add a new Comment;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_NewComment(object sender, RoutedEventArgs e)
        {
            ListComment_Scrolldd.Visibility = Visibility.Visible;
        }


        private void cBtnSave_Click_Save(object sender, RoutedEventArgs e)
        {
            if (ss == saveStatus.newComment) // post a  new comment
            {
                if (currentFeedbackList != null || currentFeedbackList.Count>=0) //wrong...
                {
                    FeedbackObject fo = new FeedbackObject();
                    FeedbackObject cf= currentFeedbackList[indexOfCommentList];
                    fo.CourseID = cf.CourseID;
                    fo.Content = cTboxComment.Text.ToString();
                    PostFeedbackByCourseID(fo);
                }
            }
            else // update an old comment
            { 
            }
        }

        #endregion button clicks

        #region list items click
        private void courseListBox_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                if (courseListBox.SelectedIndex != -1)
                {
                    CourseObject co = currentCourseList[courseListBox.SelectedIndex];
                    List<FeedbackObject> courseFeedback = GetFeedbackByCourseID(co.ID);
                    foreach (var c in courseFeedback)
                    {
                        lbFeedback.ItemsSource = courseFeedback;
                    }
                }
            }
        }

        #endregion list items click

    }
}

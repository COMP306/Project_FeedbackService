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

        #region private lists
        private List<CourseObject> currentCourseList = new List<CourseObject>();
        private List<FeedbackObject> currentFeedbackList = new List<FeedbackObject>();
        private int indexOfCourseList = -1;
        private int indexOfCommentList = -1;
        private ServiceHelper.saveStatus ss = ServiceHelper.saveStatus.newComment;
        #endregion

        public MainWindow()
        {
            PermissiveCertificatePolicy.Enact("CN=HTTPS-Server");
            InitializeComponent();
            initListBox();
            ListComment_Scrolldd.Visibility = Visibility.Collapsed;
        }


        private void initListBox()
        {
            if (courseListBox.Items.Count != 0)
            {
                courseListBox.Items.Clear();
            }
            currentCourseList = ServiceHelper.getAllCourseList();
            foreach (CourseObject co in currentCourseList)
            {
                string courseName = co.Title;
                string coueseCode = co.Code;

                courseListBox.Items.Add(co.Code + " " + co.Title);
            }
        }


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
                //List<CourseObject> searchResult = ServiceHelper.GetCourseByCodeOrTitle(tbCourseName.Text);
                currentCourseList = ServiceHelper.GetCourseByCodeOrTitle(tbCourseName.Text);
                if (currentCourseList.Count > 0)
                {
                    foreach (CourseObject co in currentCourseList)
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
            ss = ServiceHelper.saveStatus.newComment;
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

            if (ss == ServiceHelper.saveStatus.newComment) // post a  new comment
            {
                ServiceHelper.PostFeedbackByCourseID(co.ID, comment, ckAnoy.IsChecked.Value);
            }
            else // update an old comment
            {
                ServiceHelper.UpdateByFeedBackID(fo.ID, comment);
            }

            currentFeedbackList = ServiceHelper.GetFeedbackByCourseID(co.ID);
            //foreach (var c in currentFeedbackList)
            //{
                lbFeedback.ItemsSource = currentFeedbackList;
            //}
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
            ss = ServiceHelper.saveStatus.updateComemnt;
            if (courseListBox.SelectedIndex != -1)
            {
                indexOfCourseList = courseListBox.SelectedIndex;
                CourseObject co = currentCourseList[courseListBox.SelectedIndex];
                currentFeedbackList = ServiceHelper.GetFeedbackByCourseID(co.ID);
                //foreach (var c in currentFeedbackList)
                //{
                    lbFeedback.ItemsSource = currentFeedbackList;
                //}
            }
        }

        private void lbFeedback_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ss = ServiceHelper.saveStatus.updateComemnt;
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
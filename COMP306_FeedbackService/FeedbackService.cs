using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows;


namespace COMP306_FeedbackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FeedbackService : IFeedbackService
    {
        #region "GetAllCourse"
        /// <summary>
        /// query all available courses
        /// </summary>
        /// <returns>a list of CourseObject object</returns>
        //[PrincipalPermission(SecurityAction.Demand, Role="Feedbackusers")]
        public List<CourseObject> GetAllCourse()
        {
            List<CourseObject> coList = new List<CourseObject>();
            using (FeedbackEF.COMP306_FeedbackEntities fbEF = new FeedbackEF.COMP306_FeedbackEntities())
            {
                var courses = (from c in fbEF.vwCourses
                               select c).ToList();  // TO-DO:add critiera with CurrentPrincipal.Identity
                foreach (FeedbackEF.vwCourse courseEntity in courses)
                {
                    coList.Add(new CourseObject(courseEntity.ID, courseEntity.Code, courseEntity.Title));
                }
            }
            return coList;
        }
        #endregion

        #region "GetCourseByCodeOrTitle"
        /// <summary>
        /// query courses with certain code or title
        /// </summary>
        /// <param name="codeOrTitle">course code or course title</param>
        /// <returns>a list of CourseObject object</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public List<CourseObject> GetCourseByCodeOrTitle(string codeOrTitle)
        {
            List<CourseObject> coList = new List<CourseObject>();
            using (FeedbackEF.COMP306_FeedbackEntities fbEF = new FeedbackEF.COMP306_FeedbackEntities())
            {
                var courses = (from c in fbEF.vwCourses
                               where (c.Title.IndexOf(codeOrTitle)>-1 || c.Code.IndexOf(codeOrTitle)>-1)
                               select c).ToList();
                foreach (FeedbackEF.vwCourse courseEntity in courses)
                {
                    coList.Add(new CourseObject(courseEntity.ID, courseEntity.Code, courseEntity.Title));
                }
            }
            return coList;
        }
        #endregion

        #region "GetFeedbackByCourseID"
        /// <summary>
        /// query all feedback with certain course ID
        /// </summary>
        /// <param name="courseID">Course ID</param>
        /// <returns>a list of FeecbackObject object</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public List<FeedbackObject> GetFeedbackByCourseID(int courseID)
        {
            List<FeedbackObject> fbList = new List<FeedbackObject>();
            using (FeedbackEF.COMP306_FeedbackEntities fbEF = new FeedbackEF.COMP306_FeedbackEntities())
            {
                var feedbacks = (from f in fbEF.vwFeedbacks
                               where (f.CourseID == courseID)
                               select f).ToList();
                foreach (FeedbackEF.vwFeedback feedbackEntity in feedbacks)
                {
                    fbList.Add(new FeedbackObject(feedbackEntity.ID, feedbackEntity.FeedbackContent, feedbackEntity.StudentID, feedbackEntity.CourseID, feedbackEntity.PostDate));
                }
            }
            return fbList;
        }
        #endregion

        #region "PostFeedbackByCourseID"
        /// <summary>
        /// add feedback to a course
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns>returns: 0 sucess; -1 fail</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public int PostFeedbackByCourseID(FeedbackObject feedback)
        {
            int result = -1;
            using (FeedbackEF.COMP306_FeedbackEntities fbEF = new FeedbackEF.COMP306_FeedbackEntities())
            {
                //fbEF.spInsertFeedback(feedback.Content, feedback.CourseID, feedback.StudentID, feedback.PostDate);
                FeedbackEF.vwFeedback fb = new FeedbackEF.vwFeedback();
                fb.FeedbackContent = feedback.Content;
                fb.CourseID = feedback.CourseID;
                fb.PostDate = feedback.PostDate;
                fb.StudentID = feedback.StudentID;

                fbEF.vwFeedbacks.Add(fb);
                result = fbEF.SaveChanges();
            }
            return result;
        }
        #endregion

        #region "UpdateByFeedBackID"
        /// <summary>
        /// update feedback content with a certain ID
        /// </summary>
        /// <param name="id">feedback id</param>
        /// <param name="content">updated content</param>
        /// <returns>returns: 0 success; -1 fail</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public int UpdateByFeedBackID(int id, string content)
        {
            int result = -1;
            //FeedbackEF.vwFeedback fb;
            using (FeedbackEF.COMP306_FeedbackEntities fbEF = new FeedbackEF.COMP306_FeedbackEntities())
            {
                //var fb = fbEF.vwFeedbacks.Where(f => f.ID == id).FirstOrDefault();
                var fb = fbEF.vwFeedbacks.FirstOrDefault(f => f.ID == id);
                fb.FeedbackContent = content;               
                result = fbEF.SaveChanges();
            }
            return result;
        }
        #endregion

        #region "GetCurrentUser()"
        /// <summary>
        /// test current user
        /// </summary>
        /// <returns>user name</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public string GetCurrentUser()
        {
            WindowsPrincipal user = new WindowsPrincipal((WindowsIdentity)Thread.CurrentPrincipal.Identity);
            if (!(user.IsInRole("Feedbackusers")))
            { Console.WriteLine("Access denied"); }
            else
            { Console.WriteLine("Access grant to Feedbackusers"); }

            string userName = Thread.CurrentPrincipal.Identity.Name;
            Console.WriteLine("Username is : {0}", userName);

            return userName;
        }
        #endregion
    }
}

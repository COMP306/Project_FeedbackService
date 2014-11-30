using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using WCFFeedbackService.FeedbackObjects;
namespace WCFFeedbackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FeedbackService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FeedbackService.svc or FeedbackService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FeedbackService : IFeedbackService
    {
        #region "GetAllCourse"
        /// <summary>
        /// query all available courses
        /// </summary>
        /// <returns>a list of CourseObject object</returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public List<CourseObject> GetAllCourse()
        {
            List<CourseObject> coList = new List<CourseObject>();
            try
            {
                using (FeedbackEntities fbEF = new FeedbackEntities())
                {
                    var courses = (from c in fbEF.vwCourses
                                   select c).ToList();  // TO-DO:add critiera with CurrentPrincipal.Identity
                    foreach (vwCourses courseEntity in courses)
                    {
                        coList.Add(new CourseObject(courseEntity.ID, courseEntity.Code, courseEntity.Title));
                    }
                }
                if (coList.Count == 0)
                {   // no course found
                    FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                    ffi.Reason = "Zero course found.";
                    ffi.Source = "Feedback Service Database";
                    throw new FaultException<FaultFeedbackInfo>(ffi, " Zero course found.");
                }
            }
            catch (System.Data.EntityException ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = "Data connection error.";
                ffi.Source = "Feedback Service Database";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Data connection error.");
            }
            catch (Exception ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = ex.Message;
                ffi.Source = "Feedback Service";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Unexpected error." + ex.ToString());
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
        //[PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public List<CourseObject> GetCourseByCodeOrTitle(string codeOrTitle)
        {
            List<CourseObject> coList = new List<CourseObject>();
            try
            {
                using (FeedbackEntities fbEF = new FeedbackEntities())
                {
                    var courses = (from c in fbEF.vwCourses
                                   where (c.Title.IndexOf(codeOrTitle) > -1 || c.Code.IndexOf(codeOrTitle) > -1)
                                   select c).ToList();
                    foreach (vwCourses courseEntity in courses)
                    {
                        coList.Add(new CourseObject(courseEntity.ID, courseEntity.Code, courseEntity.Title));
                    }
                }
                if (coList.Count == 0)
                {   // no course found
                    FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                    ffi.Reason = "Zero course found.";
                    ffi.Source = "Feedback Service Database";
                    throw new FaultException<FaultFeedbackInfo>(ffi, " Zero course found.");
                }
            }
            catch (System.Data.EntityException ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = "Data connection error.";
                ffi.Source = "Feedback Service Database";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Data connection error.");
            }
            catch (Exception ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = ex.Message;
                ffi.Source = "Feedback Service";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Unexpected error." + ex.ToString());
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
        //[PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public List<FeedbackObject> GetFeedbackByCourseID(int courseID)
        {
            List<FeedbackObject> fbList = new List<FeedbackObject>();
            try
            {
                using (FeedbackEntities fbEF = new FeedbackEntities())
                {
                    var feedbacks = (from f in fbEF.vwFeedbacks
                                     where (f.TCourseID == courseID)
                                     select f).ToList();
                    foreach (vwFeedbacks feedbackEntity in feedbacks)
                    {
                        fbList.Add(new FeedbackObject(feedbackEntity.ID, 
                            feedbackEntity.FeedbackContent, 
                            feedbackEntity.StudentID, 
                            feedbackEntity.CourseCode,
                            feedbackEntity.CourseTitle,
                            feedbackEntity.TCourseID,
                            feedbackEntity.TStudentID,
                            feedbackEntity.PostDate, 
                            feedbackEntity.LastModify,
                            feedbackEntity.IsAnonymous=="Y"));
                    }
                    if (fbList.Count == 0)
                    {   // no feedback found
                        FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                        ffi.Reason = "No feedback found in course " + fbEF.TCourse.Single(t => t.ID==courseID).Code + ".";
                        ffi.Source = "Feedback Service Database";
                        throw new FaultException<FaultFeedbackInfo>(ffi, " No feedback found.");
                    }

                }
            }
            catch (System.Data.EntityException ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = "Data connection error.";
                ffi.Source = "Feedback Service Database";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Data connection error.");
            }
            catch (Exception ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = ex.Message;
                ffi.Source = "Feedback Service";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Unexpected error.");
            }
            return fbList;
        }
        #endregion


        #region "PostFeedbackByCourseID"
        /// <summary>
        /// add feedback to a course
        /// </summary>
        /// <param name="feedback">new feedback object need to post</param>
        /// <returns>returns: 1 sucess; 0 fail</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public int PostFeedbackByCourseID(int tcourse_id, string content, bool isAnonymous)
        {
            // only use feedback.content, and feedback.tcourseid
            // studentid is from WindowsPrincipal
            WindowsPrincipal user = new WindowsPrincipal((WindowsIdentity)Thread.CurrentPrincipal.Identity);
            if (!user.IsInRole("Feedbackusers"))
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = string.Format("User:{0} does not have right to post feedback.",user.Identity.Name);
                ffi.Source = "Feedback Service";
                throw new FaultException<FaultFeedbackInfo>(ffi, "User does not have right to post feedback.");
            }

            int result = -1;
            try
            {
                using (FeedbackEntities fbEF = new FeedbackEntities())
                {
                    var sc = from s in fbEF.vwStudentCourse
                             where (s.StudentID == user.Identity.Name && s.TCourseID == tcourse_id)
                             select s;
                    if (!sc.Any())
                    {   // student not enroll in course
                        FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                        ffi.Reason = string.Format("Student:{0} did not enroll in course:{1}.", user.Identity.Name, fbEF.TCourse.Single(t => t.ID == tcourse_id).Code);
                        ffi.Source = "Feedback Service";
                        throw new FaultException<FaultFeedbackInfo>(ffi, "Student not in course.");
                    }

                    TFeedback fb = new TFeedback
                    {
                        FeedbackContent = content,
                        TCourse = fbEF.TCourse.Single(c => c.ID == tcourse_id),
                        TStudent = fbEF.TStudent.Single(s => s.StudentID == user.Identity.Name),
                        PostDate = DateTime.Now,
                        LastModify = DateTime.Now,
                        IsAnonymous = (isAnonymous) ? "Y" : "N"
                    };

                    fbEF.TFeedback.Add(fb); 
                    result = fbEF.SaveChanges();
                }
            }
            catch (System.Data.EntityException ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = "Data connection error.";
                ffi.Source = "Feedback Service Database";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Data connection error.");
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = ex.Message;
                ffi.Source = "Feedback Service";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Unexpected error.");
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
        /// <returns>returns: 1 success; 0 fail</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public int UpdateByFeedBackID(int id, string content)
        {
            WindowsPrincipal user = new WindowsPrincipal((WindowsIdentity)Thread.CurrentPrincipal.Identity);
            if (!user.IsInRole("Feedbackusers"))
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = String.Format("User:{0} does not have right to update feedback.", user.Identity.Name);
                ffi.Source = "Feedback Service";
                throw new FaultException<FaultFeedbackInfo>(ffi, "User does not have right to update feedback.");
            }

            int result = -1;
            try
            {
                using (FeedbackEntities fbEF = new FeedbackEntities())
                {
                    var fb = fbEF.vwFeedbacks.FirstOrDefault(f => f.ID == id);
                    if (fb == null)
                    {
                        FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                        ffi.Reason = String.Format("Feedback ID:{0} not found.",id);
                        ffi.Source = "Feedback Service";
                        throw new FaultException<FaultFeedbackInfo>(ffi, "Data connection error.");
                    }

                    fb = fbEF.vwFeedbacks.FirstOrDefault(f => f.ID == id && f.LastModify >= DateTime.Now.AddMonths(-3));
                    fb.FeedbackContent = content;
                    fb.LastModify = DateTime.Now;
                    result = fbEF.SaveChanges();
                }
            }
            catch (System.Data.EntityException ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = "Data connection error.";
                ffi.Source = "Feedback Service Database";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Data connection error.");
            }
            catch (Exception ex)
            {
                FaultFeedbackInfo ffi = new FaultFeedbackInfo();
                ffi.Reason = ex.Message;
                ffi.Source = "Feedback Service";
                throw new FaultException<FaultFeedbackInfo>(ffi, "Unexpected error.");
            }
            return result;
        }
        #endregion

        #region "GetCurrentUser()"
        /// <summary>
        /// test current user
        /// </summary>
        /// <returns>user name</returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = "Feedbackusers")]
        public string GetCurrentUser()
        {
            WindowsPrincipal user = new WindowsPrincipal((WindowsIdentity)Thread.CurrentPrincipal.Identity);
            if (!(user.IsInRole("Feedbackusers")))
            { Console.WriteLine("You do not have Access."); }
            else
            { Console.WriteLine("You belong to group Feedbackusers"); }

            string userName = Thread.CurrentPrincipal.Identity.Name;
            Console.WriteLine("Username is : {0}", userName);

            return userName;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseFeecback_WPF.FeedbackService;
using System.ServiceModel;
using System.Windows;

namespace CourseFeecback_WPF
{
    static class ServiceHelper
    {
        internal enum saveStatus
        {
            newComment,
            updateComemnt
        }

        #region setCredential
        internal static FeedbackServiceClient setCredential()
        {
            return setCredential(DataEnv.useCurrentLogin);
        }
        internal static FeedbackServiceClient setCredential(bool useCurrentLogin)
        {
            PermissiveCertificatePolicy.Enact("CN=HTTPS-Server");
            FeedbackServiceClient aClient = new FeedbackServiceClient("WS2007HttpBinding_IFeedbackService");
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) => { return true; };
            if (!useCurrentLogin)
            {
                aClient.ClientCredentials.Windows.ClientCredential.Domain = DataEnv.domain;
                aClient.ClientCredentials.Windows.ClientCredential.UserName = DataEnv.username;
                aClient.ClientCredentials.Windows.ClientCredential.Password = DataEnv.password;
            }
            return aClient;
        }
        #endregion

        #region getAllCourseList
        internal static  List<CourseObject> getAllCourseList()
        {
            List<CourseObject> courseList = new List<CourseObject>();
            using (FeedbackServiceClient aClient = ServiceHelper.setCredential())
            {
                try
                {
                    var retCourse = aClient.GetAllCourse();

                    foreach (var c in retCourse)
                    {
                        courseList.Add(c);
                    }
                }
                catch (FaultException<FaultFeedbackInfo> e)
                {
                    MessageBox.Show(string.Format("FaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Service Error !!!! ");
                }
            }
            return courseList;
        }
        #endregion

        #region GetCourseByCodeOrTitle
        internal static  List<CourseObject> GetCourseByCodeOrTitle(string titleOrCode)
        {
            List<CourseObject> courseList = new List<CourseObject>();
            using (FeedbackServiceClient aClient = ServiceHelper.setCredential())
            {
                try
                { //FaultFeedbackInfo
                    var retCourse = aClient.GetCourseByCodeOrTitle(titleOrCode);

                    foreach (var c in retCourse)
                    {
                        courseList.Add(c);
                    }
                }
                catch (FaultException<FaultFeedbackInfo> e)
                {
                    MessageBox.Show(string.Format("FaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Service Error !!!! ");
                }
            }
            return courseList;
        }
        #endregion

        #region UpdateByFeedBackID
        internal static  int UpdateByFeedBackID(int id, string content)
        {
            int ret = -1;
            using (FeedbackServiceClient aClient = ServiceHelper.setCredential())
            {
                try
                {
                    ret = aClient.UpdateByFeedBackID(id, content);
                }
                catch (FaultException<FaultFeedbackInfo> e)
                {
                    MessageBox.Show(string.Format("FaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Service Error !!!! ");
                }
            }
            return ret;
        }
        #endregion

        #region PostFeedbackByCourseID
        internal static  int PostFeedbackByCourseID(int courseId, string feedbackContent, bool isAnoy)
        {
            int ret = -1;

            using (FeedbackServiceClient aClient = ServiceHelper.setCredential())
            {
                try
                {
                    ret = aClient.PostFeedbackByCourseID(courseId, feedbackContent, isAnoy); ;
                }
                catch (FaultException<FaultFeedbackInfo> e)
                {
                    MessageBox.Show(string.Format("FaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Service Error !!!! ");
                }
            }
            return ret;
        }
        #endregion

        #region GetFeedbackByCourseID
        internal static  List<FeedbackObject> GetFeedbackByCourseID(int courseID)
        {
            List<FeedbackObject> feedbaclList = new List<FeedbackObject>();

            using (FeedbackServiceClient aClient = ServiceHelper.setCredential())
            {

                try
                {
                    var retFeedbacks = aClient.GetFeedbackByCourseID(courseID);
                    foreach (var c in retFeedbacks)
                    {
                        feedbaclList.Add(c);
                    }
                    //currentFeedbackList = feedbaclList;
                }
                catch (FaultException<FaultFeedbackInfo> e)
                {
                    MessageBox.Show(string.Format("FaultException<FaultFeedbackInfo>:\n   Source : {0}\n   Reason : {1}\n", e.Detail.Source, e.Detail.Reason));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Service Error !!!! ");
                }
            }

            return feedbaclList;
        }
        #endregion

    }
}

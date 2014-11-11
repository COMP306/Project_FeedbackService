using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using FeedbackObjects;
/// <summary>
/// Summary description for IFeedbackService
/// </summary>
namespace FeedbackService
{
    public interface IFeedbackService
    {
        [OperationContract]
        [FaultContract(typeof(FaultFeedbackInfo))]
        List<CourseObject> GetAllCourse();

        [OperationContract]
        [FaultContract(typeof(FaultFeedbackInfo))]
        List<CourseObject> GetCourseByCodeOrTitle(string codeOrTitle);

        [OperationContract]
        [FaultContract(typeof(FaultFeedbackInfo))]
        List<FeedbackObject> GetFeedbackByCourseID(int courseID);

        [OperationContract]
        [FaultContract(typeof(FaultFeedbackInfo))]
        int PostFeedbackByCourseID(FeedbackObject feedback);

        [OperationContract]
        [FaultContract(typeof(FaultFeedbackInfo))]
        int UpdateByFeedBackID(int id, string content);

        [OperationContract]
        string GetCurrentUser();

    }
}
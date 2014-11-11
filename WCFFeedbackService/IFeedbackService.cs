using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFFeedbackService.FeedbackObjects;
namespace WCFFeedbackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFeedbackService" in both code and config file together.
    [ServiceContract]
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

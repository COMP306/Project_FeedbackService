using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace COMP306_FeedbackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class FeedbackService : IFeedbackService
    {

        public List<CourseObject> GetAllCourse()
        {
            throw new NotImplementedException();
        }

        public List<CourseObject> GetCourseByCodeOrTitle(string codeOrTitle)
        {
            throw new NotImplementedException();
        }

        public List<FeedbackObject> GetFeedbackByCourseID(int courseID)
        {
            throw new NotImplementedException();
        }

        public int PostFeedbackByCourseID(FeedbackObject feedback)
        {
            throw new NotImplementedException();
        }

        public int UpdateByFeedBackID(int id, string content)
        {
            throw new NotImplementedException();
        }
    }
}

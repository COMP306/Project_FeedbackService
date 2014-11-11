using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WCFFeedbackService.FeedbackObjects
{
    /// <summary>
    /// Summary description for FeedbackObjects
    /// </summary>
        [DataContract]
        public class CourseObject
        {
            int _id;
            string _code;
            string _title;
            [DataMember]
            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }
            [DataMember]
            public string Code
            {
                get { return _code; }
                set { _code = value; }
            }
            [DataMember]
            public string Title
            {
                get { return _title; }
                set { _title = value; }
            }
            public CourseObject(int id, string code, string title)
            {
                this._id = id;
                this._code = code;
                this._title = title;
            }
        }

        [DataContract]
        public class StudentObject
        {
            private int _id;
            private string _student_id;
            [DataMember]
            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }
            [DataMember]
            public string StudentID
            {
                get { return _student_id; }
                set { _student_id = value; }
            }
            public StudentObject(int id, string student_id)
            {
                this._id = id;
                this._student_id = student_id;
            }
        }

        [DataContract]
        public class FeedbackObject
        {
            private int _id;
            private string _content;
            private int _student_id;
            private int _course_id;
            private DateTime _post_date;
            [DataMember]
            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }
            [DataMember]
            public string Content
            {
                get { return _content; }
                set { _content = value; }
            }
            [DataMember]
            public int StudentID
            {
                get { return _student_id; }
                set { _student_id = value; }
            }
            [DataMember]
            public int CourseID
            {
                get { return _course_id; }
                set { _course_id = value; }
            }
            [DataMember]
            public DateTime PostDate
            {
                get { return _post_date; }
                set { _post_date = value; }
            }
            public FeedbackObject(int id, string content, int student_id, int course_id, DateTime post_date)
            {
                this._id = id;
                this._content = content;
                this._student_id = student_id;
                this._course_id = course_id;
                this._post_date = post_date;
            }
        }

        [DataContract]
        class FaultFeedbackInfo
        {
            [DataMember]
            public string Source;
            [DataMember]
            public string Reason;
        }
}
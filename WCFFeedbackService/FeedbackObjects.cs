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
            private string _student_id;
            private string _course_code;
            private string _course_title;
            private int _tcourse_id;
            private int _tstudent_id;
            private DateTime _post_date;
            private DateTime _last_modify;
            private bool _is_anonymous; 
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
            public string StudentID
            {
                get { return _student_id; }
                set { _student_id = value; }
            }
            [DataMember]
            public string Course_Code
            {
                get { return _course_code; }
                set { _course_code = value; }
            }
            [DataMember]
            public string Course_Title
            {
                get { return _course_title; }
                set { _course_title = value; }
            }
            [DataMember]
            public int TCourseID
            {
                get { return _tcourse_id; }
                set { _tcourse_id = value; }
            }
            [DataMember]
            public int TStudentID
            {
                get { return _tstudent_id; }
                set { _tstudent_id = value; }
            }
            [DataMember]
            public DateTime PostDate
            {
                get { return _post_date; }
                set { _post_date = value; }
            }
            [DataMember]
            public DateTime LastModify
            {
                get { return _last_modify; }
                set { _last_modify = value; }
            }
            [DataMember]
            public bool IsAnonymous
            {
                get { return _is_anonymous; }
                set { _is_anonymous = value; }
            }
            public FeedbackObject(int id, string content, string student_id, string course_code, string course_title, int tcourse_id, int tstudent_id,DateTime post_date,DateTime last_modify, bool is_anonymous)
            {
                this._id = id;
                this._content = content;
                this._student_id = student_id;
                this._course_code = course_code;
                this._course_title = course_title;
                this._tcourse_id = tcourse_id;
                this._tstudent_id = tstudent_id;
                this._post_date = post_date;
                this._last_modify = last_modify;
                this._is_anonymous = is_anonymous;
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
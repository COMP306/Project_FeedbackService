//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFFeedbackService
{
    using System;
    using System.Collections.Generic;
    
    public partial class TCourse
    {
        public TCourse()
        {
            this.TFeedback = new HashSet<TFeedback>();
            this.TStudentCourse = new HashSet<TStudentCourse>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
    
        public virtual ICollection<TFeedback> TFeedback { get; set; }
        public virtual ICollection<TStudentCourse> TStudentCourse { get; set; }
    }
}
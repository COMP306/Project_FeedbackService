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
    
    public partial class TFeedback
    {
        public int ID { get; set; }
        public string FeedbackContent { get; set; }
        public System.DateTime PostDate { get; set; }
        public System.DateTime LastModify { get; set; }
        public string IsAnonymous { get; set; }
    
        public virtual TCourse TCourse { get; set; }
        public virtual TStudent TStudent { get; set; }
    }
}

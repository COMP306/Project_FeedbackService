using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace COMP306_FeedbackService
{
    [DataContract]
    class FaultFeedbackInfo
    {
        [DataMember]
        public string Source;
        [DataMember]
        public string Reason;
    }
}

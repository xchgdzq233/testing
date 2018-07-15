using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FFX_P8_Image.Services.FileNet
{
    [DataContract]
    public class JitTicket
    {
        [DataMember]
        public string sGuid { get; set; }
        [DataMember]
        public string sDocName { get; set;  }
    }
}

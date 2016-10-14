using FFX_P8_Image.Services.FileNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FFX_P8_Image.Services
{
    [ServiceContract]
    public interface IFFXImageService
    {
        [OperationContract]
        List<string> GetTicketsList(string sTicketNum);
        //[OperationContract]
        //byte[] GetImage(string sGuid);
    }
}

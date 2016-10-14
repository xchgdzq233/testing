using FFX_P8_Image.Services.FileNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFX_P8_Image.Services
{
    public class FFXImageService : IFFXImageService, IDisposable
    {
        //readonly FileNetUtil fnUtil = FileNetUtil.GetInstance();

        public List<string> GetTicketsList(string sTicketNum)
        {
            //List<string> result = new List<string>();

            //List<string> lDocClasses = ConfigurationManager.AppSettings["FN_DocClasses"].Split(',').Select(p => p.Trim()).ToList();
            //foreach (string docClass in lDocClasses)
            //{
            //    string sql = "SELECT [This], [TicketNumber], [DocumentTitle], [DateCreated], [Id] FROM [" + docClass + "] WHERE [TicketNumber] = '' ORDER BY TicketNumber ASC";
            //    List<string> list = FileNetUtil.GetInstance().FNSearch(sql);
            //    if (list == null)
            //        continue;
            //    result.AddRange(list);
            //}

            //return result == null ? null : result;
            var p = Thread.CurrentPrincipal;
            string[] test = {sTicketNum + ".a", sTicketNum + ".b"};

            return test.ToList();
        }

        //public byte[] GetImage(string sGuid)
        //{


        //    return null;
        //}

        public void Dispose()
        {
        }
    }
}

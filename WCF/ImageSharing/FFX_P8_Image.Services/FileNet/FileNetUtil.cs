using FileNet.Api.Collection;
using FileNet.Api.Core;
using FileNet.Api.Query;
using FileNet.Api.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFX_P8_Image.Services.FileNet
{
    public class FileNetUtil
    {
        private static FileNetUtil _instance;
        private CEConnection _ceConn;
        private string sUri, sObjectStore, sUserName, sPassword;

        private FileNetUtil()
        {
            this._ceConn = CEConnection.GetInstance(ConfigurationManager.AppSettings["FN_URI"], ConfigurationManager.AppSettings["FN_ObjectStore"],
                ConfigurationManager.AppSettings["FN_UserName"], ConfigurationManager.AppSettings["FN_Password"]);
        }

        public static FileNetUtil GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FileNetUtil();
            }
            return _instance;
        }

        public List<string> FNSearch(string sql)
        {
            List<string> result = new List<string>();

            SearchSQL searchSql = new SearchSQL(sql);
            SearchScope searchScope = new SearchScope(this._ceConn.objectStore);
            IRepositoryRowSet rowSet = searchScope.FetchRows(searchSql, null, null, null);

            if (rowSet.IsEmpty())
                return null;

            System.Collections.IEnumerator enumerator = rowSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IRepositoryRow row = (IRepositoryRow)enumerator.Current;
                Id id = row.Properties.GetProperty("Id").GetIdValue();
                result.Add(id.ToString());
            }

            return result;
        }

        public byte[] GetImageByGuid(string sGuid)
        {
            Id ID = new Id(sGuid);
            IDocument doc = Factory.Document.FetchInstance(this._ceConn.objectStore, ID, null);

            return null;
        }
    }
}

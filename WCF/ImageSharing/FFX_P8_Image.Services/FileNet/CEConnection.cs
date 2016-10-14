using FileNet.Api.Authentication;
using FileNet.Api.Core;
using FileNet.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFX_P8_Image.Services.FileNet
{
    public class CEConnection
    {
        private static Object SyncRoot = new object();

        private static CEConnection _instance;
        public IConnection conn { get; private set; }
        private IDomain _domain;
        public IObjectStore objectStore { get; private set; }

        private CEConnection(string sUri, string sObjectStore, string sUserName, string sPassword)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            UsernameCredentials cred = new UsernameCredentials(sUserName, sPassword);
            ClientContext.SetProcessCredentials(cred);
            this.conn = Factory.Connection.GetConnection(sUri);
            this._domain = Factory.Domain.FetchInstance(this.conn, null, null);
            this.objectStore = Factory.ObjectStore.FetchInstance(this._domain, sObjectStore, null);
        }

        public static CEConnection GetInstance(string uri, string objectstore, string username, string password)
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new CEConnection(uri, objectstore, username, password);
                    }
                }
            }

            return _instance;
        }
    }
}

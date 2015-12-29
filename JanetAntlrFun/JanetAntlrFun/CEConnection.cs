using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Configuration;
using FileNet.Api.Authentication;
using FileNet.Api.Util;
using FileNet.Api.Core;
using FileNet.Api.Collection;

using FileNet.Api.Constants;


namespace JanetAntlrFun
{

    /// <summary>
    /// Content Engine FileNetUtility Classs (singleton) 
    /// </summary>
    public class CEConnection
    {
        /// <summary>
        /// Instance of Logger
        /// </summary>
      //  private static ILog logger = LogManager.GetLogger(typeof(CEConnection));

        /// <summary>
        /// singleton instance of CEConnection
        /// </summary>
        private static CEConnection instance;

        /// <summary>
        /// Domain
        /// </summary>
        private IDomain _domain;

        /// <summary>
        /// Get IDomain
        /// </summary>
        public IDomain Domain
        {
            get { return _domain; }
        }

        /// <summary>
        /// FileNetUtility
        /// </summary>
        private IConnection _connection;

        /// <summary>
        /// Get IConnection
        /// </summary>
        public IConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Object Store
        /// </summary>
        private IObjectStore _objectStore;

        /// <summary>
        /// Get IObject Store
        /// </summary>
        public IObjectStore ObjectStore
        {
            get { return _objectStore; }
        }

        /// <summary>
        /// Get Instance of  CEConnection
        /// </summary>
        /// <returns>Instance of CEConnection</returns>
        public static CEConnection getCEConnectionInstance()
        {
           // logger.Debug("Getting Instance of " + typeof(CEConnection));
            if (null == instance)
            {
                instance = new CEConnection(ConfigurationManager.AppSettings["FN_UserName"],
                    ConfigurationManager.AppSettings["FN_Password"],
                    ConfigurationManager.AppSettings["FN_URI"],
                    ConfigurationManager.AppSettings["FN_ObjectStore"]);
            }
            return instance;
        }

        /// <summary>
        /// Instance Retriever Property
        /// </summary>
        public static CEConnection Instance
        {
            get
            {
                return CEConnection.getCEConnectionInstance();
            }
        }


        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param keyName="userName">Username for Connect Engine</param>
        /// <param keyName="password">Password for Connect Engine</param>
        /// <param keyName="uri">URI</param>
        /// <param keyName="osName">Object Store Name</param>
        private CEConnection(string userName, string password, string uri, string osName)
        {
            //logger.Debug("Initializing " + typeof(CEConnection));
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

            UsernameCredentials creds = new UsernameCredentials(userName, password);

            // Associate this ClientContext with the whole process. 
            ClientContext.SetProcessCredentials(creds);

            // Get the connection and default domain.
            this._connection = Factory.Connection.GetConnection(uri);
            this._domain = Factory.Domain.GetInstance(this._connection, null);

            // Get an object store.
            this._objectStore = Factory.ObjectStore.FetchInstance(this._domain, osName, null);
        }

    }
}
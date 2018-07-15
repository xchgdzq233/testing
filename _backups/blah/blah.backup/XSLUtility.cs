
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Com.Fairfax.Filenet.Utilities.XSL
{
    /// <summary>
    /// Utility class to perform XSL Transformations
    /// </summary>
    public sealed class XSLUtility
    {
        /// <summary>
        /// instance of logger
        /// </summary>
        

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static XSLUtility _instance;

        /// <summary>
        /// Variable type serializer (this really does not belong here)
        /// </summary>
        private XmlSerializer varsSerializer;

        /// <summary>
        /// Gets the singleton instance of this.
        /// </summary>
        public static XSLUtility Instance
        {
            get
            {
                return _instance == null ? _instance = new XSLUtility() : _instance;
            }
        }

        /// <summary>
        /// Gets the singleton instance of this.
        /// </summary>
        /// <returns>Instance of XSLUtility</returns>
        public static XSLUtility GetInstance()
        {
            return XSLUtility.Instance;
        }

        /// <summary>
        /// Private constructor - initializes XSLUtility
        /// </summary>
        private XSLUtility()
        {
            
        }

        /// <summary>
        /// Performs a XSL Transformation 
        /// </summary>
        /// <param name="styleSheet">Style Sheet path relative to Application Base directory</param>
        /// <param name="xslArguments">XSL Arguments</param>
        /// <param name="inputStream">Input Stream</param>
        /// <param name="outputStream">Out put Stream as Ref (output Stream is initialized if it is null</param>
        public void Transform(String styleSheet, Dictionary<String, String> xslArguments, Stream inputStream, ref Stream outputStream)
        {
            try
            {
                
                XsltSettings xsltSettings = new XsltSettings();
                xsltSettings.EnableScript = true;
                xsltSettings.EnableDocumentFunction = true;
                XsltArgumentList xslArgumentList = new XsltArgumentList();
                foreach (String key in xslArguments.Keys)
                {
                    xslArgumentList.AddParam(key, "", xslArguments[key]);
                }
                XslCompiledTransform transformer = new XslCompiledTransform();
                String stylesheetpath = AppDomain.CurrentDomain.BaseDirectory + styleSheet;
                transformer.Load(stylesheetpath, xsltSettings, new XmlUrlResolver());
                if (null == outputStream)
                {
                    outputStream = new MemoryStream();
                }
                StreamWriter streamWriter = new StreamWriter(outputStream);
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;
                XmlReader xmlReader = XmlReader.Create(inputStream, xmlReaderSettings);
                XmlTextWriter writer = new XmlTextWriter(outputStream,System.Text.Encoding.UTF8);
                writer.Formatting = Formatting.None;
                transformer.Transform(xmlReader, xslArgumentList, writer);
                
            }
            catch (Exception exception)
            {
                
                throw exception;
            }

        }


        /// <summary>
        /// Runs a XSLT Transformation
        /// </summary>
        /// <param name="inputStream">Input  Stream</param>
        /// <param name="outputSream">outputStream - initialized to a new MemoryStream (strict out parameter)</param>
        /// <param name="xslArguments">XSLT Arguments</param>
        /// <param name="styleSheet">Reltaive path to stylesheet (from application directory)</param>
        public void Transform(Stream inputStream, out Stream outputSream, Dictionary<String, object> xslArguments, String styleSheet)
        {
            try
            {
                
                XsltSettings xsltSettings = new XsltSettings();
                xsltSettings.EnableScript = true;
                xsltSettings.EnableDocumentFunction = true;
                XsltArgumentList xslArgumentList = new XsltArgumentList();
                foreach (String key in xslArguments.Keys)
                {
                    xslArgumentList.AddParam(key, "", xslArguments[key]);
                }

                xsltSettings.EnableDocumentFunction = true;
                XslCompiledTransform transformer = new XslCompiledTransform();
                String styleSheetAbsolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, styleSheet);
                transformer.Load(styleSheetAbsolutePath, xsltSettings, new XmlUrlResolver());
                outputSream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(outputSream);

                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;
                XmlReader xmlReader = XmlReader.Create(inputStream, xmlReaderSettings);
                //reader.Settings.DtdProcessing = DtdProcessing.Ignore;
                XmlTextWriter writer = new XmlTextWriter(outputSream, System.Text.Encoding.UTF8);
                writer.Formatting = Formatting.None;
                transformer.Transform(xmlReader, xslArgumentList, writer);
            }
            catch (Exception exception)
            {
                
                throw ;
            }
        }





      

    }




}

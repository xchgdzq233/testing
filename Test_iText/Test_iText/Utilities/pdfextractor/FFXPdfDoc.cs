using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Test_iText.PDFUtil
{

    public class FFXPdfDoc
    {
        public static FFXPdfDoc _PDFDocInstance;
        public string sDocName { get; private set; }
        public string sDocPath { get; private set; }

        private FFXPdfDoc() { }

        public static FFXPdfDoc GetInstance(string sDocPath = "")
        {
            if (_PDFDocInstance == null)
            {
                _PDFDocInstance = new FFXPdfDoc();

                if (String.IsNullOrEmpty(sDocPath))
                {
                    throw new Exception("Please provide a pdf document path.");
                }
                _PDFDocInstance.sDocName = System.IO.Path.GetFileName(sDocPath);
                _PDFDocInstance.sDocPath = sDocPath;
            }
            return _PDFDocInstance;
        }

        private FFXPdfPage LoadPageContent(int iPageNum, PdfReader reader)
        {
            FFXPdfPage page = new FFXPdfPage(iPageNum, reader.GetPageSize(iPageNum));

            PdfDictionary resources = reader.GetPageN(iPageNum).GetAsDict(PdfName.RESOURCES);
            IRenderListener listener = new FFXPdfRenderListener(page);
            new PdfContentStreamProcessor(listener).ProcessContent(ContentByteUtils.GetContentBytesForPage(reader, iPageNum), resources);

            resources.Clear();
            return page;
        }

        public MemoryStream ExportPDF(FFXExportLevel exportLevel)
        {
            PdfReader reader = null;
            MemoryStream result = null;

            try
            {
                //prepare
                reader = new PdfReader(new RandomAccessFileOrArray(sDocPath), null);
                result = new MemoryStream();

                using (Stream stream = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine("<html><body>");
                    sw.Flush();
                    stream.Position = 0;
                    stream.CopyTo(result);
                }

                // each pdf page
                for (int i = 6; i <= 8; i++)
                // for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    FFXPdfPage page = LoadPageContent(i, reader);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        string sXsltPath = System.IO.Path.Combine(Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory).Where(s => s.Equals("pdfextractor")).First(), "XmlToHtml.xslt");
                        xslt.Load(sXsltPath);
                        xslt.Transform(page.ExportPage(exportLevel), null, ms);

                        ms.Position = 0;
                        ms.CopyTo(result);
                    }
                }

                using (Stream stream = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine("</body></html>");
                    sw.Flush();
                    stream.Position = 0;
                    stream.CopyTo(result);
                }
            }
            catch (Exception e)
            {
                result.Dispose();
                throw e;
            }
            finally
            {
                reader.Dispose();
            }

            result.Position = 0;
            return result;
        }

        //public StreamReader ExportPDF(FFXExportType exportType, FFXExportLevel exportLevel, string sExportFilePath = "")
        //{
        //    PdfReader reader = null;
        //    StreamWriter writerStream = null;
        //    XmlWriter writerXML = null;
        //    MemoryStream streamHTML = null;

        //    try
        //    {
        //        // prepare
        //        reader = new PdfReader(new RandomAccessFileOrArray(sDocPath), null);

        //        switch (exportType)
        //        {
        //            case FFXExportType.Text:
        //                //writerStream = new StreamWriter(sExportFilePath);
        //                break;
        //            case FFXExportType.XML:
        //                writerXML = XmlWriter.Create(sExportFilePath);
        //                writerXML.WriteStartDocument();
        //                writerXML.WriteStartElement("Document");
        //                writerXML.WriteAttributeString("DocName", this.sDocName);
        //                writerXML.WriteAttributeString("DocPath", this.sDocPath);
        //                break;
        //            default:
        //                streamHTML = new MemoryStream();
        //                writerStream = new StreamWriter(streamHTML);
        //                writerStream.Write("<html><body>");
        //                writerStream.Flush();
        //                break;
        //        }

        //        // each pdf page
        //        for (int i = 6; i <= 8; i++)
        //        // for (int i = 1; i <= reader.NumberOfPages; i++)
        //        {
        //            FFXPdfPage page = LoadPageContent(i, reader);

        //            switch (exportType)
        //            {
        //                case FFXExportType.Text:
        //                    writerStream.WriteLine(page.ToString());
        //                    writerStream.Flush();
        //                    break;
        //                case FFXExportType.XML:
        //                    page.ExportPage(exportLevel, writerXML);
        //                    break;
        //                default:
        //                    using (MemoryStream ms = new MemoryStream())
        //                    {
        //                        XslCompiledTransform xslt = new XslCompiledTransform();
        //                        xslt.Load(@"C:\Github\testing\Test_iText\Test_iText\PdfPage.xslt");
        //                        xslt.Transform(page.ExportPage(exportLevel), null, ms);

        //                        ms.Position = 0;
        //                        ms.WriteTo(streamHTML);
        //                    }
        //                    break;
        //            }
        //        }

        //        // close up
        //        if (exportType == FFXExportType.XML)
        //        {
        //            writerXML.WriteEndElement();
        //            writerXML.WriteEndDocument();
        //        }
        //        else if (exportType == FFXExportType.HTML)
        //        {
        //            writerStream.Write("</body></html>");
        //            writerStream.Flush();

        //            streamHTML.Position = 0;

        //        }
        //    }
        //    catch (Exception e) { throw e; }
        //    finally
        //    {
        //        reader.Close();
        //        //if (!object.ReferenceEquals(null, writerStream))
        //        //    writerStream.Close();
        //        //if (!object.ReferenceEquals(null, writerXML))
        //        //    writerXML.Close();
        //    }
        //    return result;
        //}
    }
}

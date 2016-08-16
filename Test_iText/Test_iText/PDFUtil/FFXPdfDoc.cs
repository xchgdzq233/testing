using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public void ExportPDF(FFXExportType exportType, FFXExportLevel exportLevel, string sExportFilePath)
        {
            PdfReader reader = null;
            StreamWriter writerStream = null;
            XmlWriter writerXML = null;

            try
            {
                // prepare
                reader = new PdfReader(new RandomAccessFileOrArray(sDocPath), null);
                if (exportType == FFXExportType.Text)
                    writerStream = new StreamWriter(sExportFilePath);
                else
                {
                    writerXML = XmlWriter.Create(sExportFilePath);
                    writerXML.WriteStartElement("Document");
                    writerXML.WriteAttributeString("DocName", this.sDocName);
                    writerXML.WriteAttributeString("DocPath", this.sDocPath);
                }

                // each pdf page
                for (int i = 6; i <= 8; i++)
                // for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    FFXPdfPage page = LoadPageContent(i, reader);
                    StringBuilder sb = new StringBuilder();

                    if (exportType == FFXExportType.Text)
                        writerStream.WriteLine(page.ToString());
                    else
                        page.ExportPage(exportLevel, writerXML);
                        //page.ExportPageToXML(FFXExportLevel.Page, sExportFilePath, writerXML);
                }

                // close up
                if (exportType == FFXExportType.XML)
                    writerXML.WriteEndElement();
            }
            catch (Exception) { }
            finally
            {
                reader.Close();
                if (!object.ReferenceEquals(null, writerStream))
                    writerStream.Close();
                if (!object.ReferenceEquals(null, writerXML))
                    writerXML.Close();
            }
        }
    }
}

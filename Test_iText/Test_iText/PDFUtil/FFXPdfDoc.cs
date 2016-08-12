using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
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

        public void ReadPDFToXML(string sXMLPath)
        {
            PdfReader reader = new PdfReader(new RandomAccessFileOrArray(sDocPath), null);
            PdfDictionary resources;

            using (XmlWriter writer = XmlWriter.Create(sXMLPath))
            {
                writer.WriteStartElement("Document");
                writer.WriteAttributeString("DocName", this.sDocName);
                writer.WriteAttributeString("DocPath", this.sDocPath);

                for (int i = 6; i <= 8; i++)
                // for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    FFXPdfPage page = new FFXPdfPage(i, reader.GetPageSize(i));

                    resources = reader.GetPageN(i).GetAsDict(PdfName.RESOURCES);
                    IRenderListener listener = new FFXPdfRenderListener(page);
                    new PdfContentStreamProcessor(listener).ProcessContent(ContentByteUtils.GetContentBytesForPage(reader, i), resources);

                    page.WritePageToXML(sXMLPath, writer);
                }

                writer.WriteEndElement();
            }
        }
    }
}

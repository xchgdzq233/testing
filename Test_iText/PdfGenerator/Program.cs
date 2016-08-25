using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace PdfGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string sInputHtmlPath = @"C:\FFX_Projects\NY_Legislative\Samples\result.html";
            string sOutputPdfPath = @"C:\FFX_Projects\NY_Legislative\Samples\result.pdf";

            Byte[] bytes;

            using (MemoryStream ms = new MemoryStream())
            using (Document doc = new Document())
            using (PdfWriter pdfWriter = PdfWriter.GetInstance(doc, ms))
            {
                doc.Open();

                using (StringReader sr = new StringReader(sInputHtmlPath))
                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, doc, sr);

                doc.Close();

                bytes = ms.ToArray();
            }

            File.WriteAllBytes(sOutputPdfPath, bytes);
        }
    }
}

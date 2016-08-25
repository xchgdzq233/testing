using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace WebEditor.Utilities
{

    public class FFXPdfDoc
    {
        public string sDocName { get; private set; }
        public string sDocPath { get; private set; }

        public FFXPdfDoc(string sDocPath = "")
        {
            this.sDocName = System.IO.Path.GetFileName(sDocPath);
            this.sDocPath = sDocPath;
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

        public MemoryStream ExportPDF()
        {
            PdfReader reader = null;
            MemoryStream result = null;

            try
            {
                //prepare
                reader = new PdfReader(new RandomAccessFileOrArray(this.sDocPath), null);
                result = new MemoryStream();

                using (Stream stream = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine("<html><head><meta charset=\"UTF-8\" /></head><body>"﻿);
                    sw.Flush();
                    stream.Position = 0;
                    stream.CopyTo(result);
                }

                // each pdf page
                for (int i = 6; i <= 8; i++)
                //for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    FFXPdfPage page = LoadPageContent(i, reader);

                    using (MemoryStream ms = new MemoryStream())
                    using (StreamWriter sw = new StreamWriter(ms))
                    {
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        string temp = Utilitiess.GetInstance().GetFilePath(@"Utilities/webeditor-html.xslt");
                        xslt.Load(Utilitiess.GetInstance().GetFilePath(@"Utilities/webeditor-html.xslt"));

                        XmlDocument xml = page.ExportPage(FFXExportLevel.Line);
                        xml.Save(this.sDocPath.Replace(".pdf", "-" + i.ToString()  + ".xml"));
                        xslt.Transform(xml, null, ms);

                        sw.WriteLine();
                        sw.Flush();
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
    }
}

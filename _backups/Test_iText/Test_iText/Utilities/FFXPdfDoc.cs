using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;
using System.Text;
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

        private void WriteDocHeaderFooter (string sContent, Stream streamResult)
        {
            using (Stream stream = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.WriteLine(sContent);
                sw.Flush();
                stream.Position = 0;
                stream.CopyTo(streamResult);
            }
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

                WriteDocHeaderFooter("<html>" + 
                                        "<head><meta charset=\"UTF-8\" /></head>" + 
                                        "<body><link type=\"text/css\" rel=\"Stylesheet\" href=\"" + Utilitiess.GetInstance().cssJustify + "\" />", 
                    result);

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

                WriteDocHeaderFooter("</body></html>", result);
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

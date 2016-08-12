using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test_iText.PDFUtil
{
    public class FFXPdfPage
    {
        public int iPageNumber { get; private set; }
        public float fPageHeight { get; private set; }
        public float fPageWidth { get; private set; }
        public LinkedList<FFXPdfLine> lLines { get; private set; }

        public FFXPdfPage(int iPageNumber, Rectangle rRect)
        {
            this.iPageNumber = iPageNumber;
            this.fPageHeight = rRect.Height;
            this.fPageWidth = rRect.Width;
            lLines = new LinkedList<FFXPdfLine>();
        }

        public void AddNewTokenToPage(FFXPdfToken token)
        {
            if (lLines.Count == 0)
            {
                lLines.AddLast(new FFXPdfLine(token));
                return;
            }

            foreach (FFXPdfLine line in lLines)
            {
                // find line
                if (line.IsInLine(token))
                {
                    line.AddNewTokenToLine(token);
                    return;
                }
            }

            lLines.AddLast(new FFXPdfLine(token));
        }

        public void WritePageToXML(string sXMLPath, XmlWriter writer)
        {
            writer.WriteStartElement("Page");
            writer.WriteAttributeString("PageNumber", iPageNumber.ToString());

            foreach (FFXPdfLine line in lLines)
            {
                line.WriteLineToXML(writer);
            }

            writer.WriteEndElement();
        }
    }
}

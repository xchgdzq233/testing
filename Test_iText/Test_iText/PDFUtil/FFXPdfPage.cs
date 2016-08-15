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
        public FFXPdfLine firstLine { get; private set; }

        public FFXPdfPage(int iPageNumber, Rectangle rRect)
        {
            this.iPageNumber = iPageNumber;
            this.fPageHeight = rRect.Height;
            this.fPageWidth = rRect.Width;
        }

        public void AddNewTokenToPage(FFXPdfToken token)
        {
            // check empty page
            if (firstLine == null)
            {
                firstLine = new FFXPdfLine(token);
                return;
            }

            FFXPdfLine curLine = firstLine;

            do
            {
                // find same line
                if (curLine.IsInLine(token))
                {
                    curLine.AddNewTokenToLine(token);
                    break;
                }

                // create new line before current line
                if (curLine.CompareTo(token) == 1)
                {
                    FFXPdfLine newLine = new FFXPdfLine(token);

                    // start of page
                    if (curLine.preLine == null)
                    {
                        firstLine = newLine;
                        newLine.AddLineAfter(curLine);
                    }
                    else
                        curLine.preLine.AddLineAfter(newLine);
                    break;
                }

                // end of page
                if (curLine.nextLine == null)
                {
                    FFXPdfLine newLine = new FFXPdfLine(token);
                    curLine.AddLineAfter(newLine);
                    break;
                }

                // move to next line
                curLine = curLine.nextLine;
            }
            while (curLine != null);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.firstLine.ToString());
            FFXPdfLine curLine = this.firstLine;

            while(curLine.nextLine != null)
            {
                sb.Append("\n" + curLine.nextLine.ToString());
                curLine = curLine.nextLine;
            }

            return sb.ToString();
        }

        public void ExportPageToXML(FFXExportLevel exportLevel, string sXMLPath, XmlWriter writer)
        {
            writer.WriteStartElement("Page");
            writer.WriteAttributeString("PageNumber", iPageNumber.ToString());

            FFXPdfLine curLine = firstLine;

            if (exportLevel == FFXExportLevel.Page)
                writer.WriteString(this.ToString());
            else
            {
                do
                {
                    curLine.ExportLineToXML(exportLevel, writer);
                    curLine = curLine.nextLine;
                }
                while (curLine != null);
            }
            writer.WriteEndElement();
        }
    }
}

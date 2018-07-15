using iTextSharp.text;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebEditor.Utilities
{
    public class FFXPdfPage
    {
        public int iPageNumber { get; private set; }
        public int iPageHeight { get; private set; }
        public int iPageWidth { get; private set; }

        public FFXPdfLine firstLine { get; private set; }

        public FFXPdfPage(int iPageNumber, Rectangle rRect)
        {
            this.iPageNumber = iPageNumber;
            this.iPageHeight = Utilitiess.GetInstance().ConvertToPx(rRect.Height); 
            this.iPageWidth = Utilitiess.GetInstance().ConvertToPx(rRect.Width);
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
            StringBuilder sb = new StringBuilder();

            FFXPdfLine curLine = firstLine;

            while (curLine != null)
            {
                sb.AppendLine(curLine.ToString());
                curLine = curLine.nextLine;
            }

            return sb.ToString();
        }

        public void ExportPage(FFXExportLevel exportLevel, XmlWriter writer)
        {
            writer.WriteStartElement("Page");
            writer.WriteAttributeString("PageNumber", iPageNumber.ToString());
            writer.WriteAttributeString("Height", this.iPageHeight.ToString());
            writer.WriteAttributeString("Width", this.iPageWidth.ToString());
            writer.WriteAttributeString("style", String.Format("width:{0}px", this.iPageWidth.ToString()));

            if (exportLevel == FFXExportLevel.Page)
            {
                writer.WriteAttributeString("FontFamily", firstLine.firstToken.sFontFamily);
                writer.WriteAttributeString("Bold", firstLine.firstToken.bFontBold.ToString());
                writer.WriteAttributeString("FontSize", firstLine.firstToken.iFontSize.ToString());
                writer.WriteAttributeString("X", firstLine.firstToken.iXCoord.ToString());
                writer.WriteAttributeString("Y", firstLine.firstToken.iYCoord.ToString());

                writer.WriteString(this.ToString());
            }
            else
            {
                FFXPdfLine curLine = firstLine;

                while (curLine != null)
                {
                    curLine.ExportLine(exportLevel, writer);
                    curLine = curLine.nextLine;
                }
            }

            writer.WriteEndElement();
        }

        public XmlDocument ExportPage(FFXExportLevel exportLevel)
        {
            XmlDocument xmlDoc = new XmlDocument();
            using (XmlWriter writer = xmlDoc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();

                this.ExportPage(exportLevel, writer);

                writer.WriteEndDocument();
            }

            return xmlDoc;
        }
    }
}

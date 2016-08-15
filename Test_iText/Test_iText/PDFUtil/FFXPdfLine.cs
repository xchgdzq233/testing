using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Test_iText.PDFUtil
{
    public class FFXPdfLine : IComparable<FFXPdfToken>
    {
        public int iLineNumber { get; private set; }
        public FFXPdfToken firstToken { get; private set; }

        public FFXPdfLine preLine { get; set; }
        public FFXPdfLine nextLine { get; set; }

        public FFXPdfLine(FFXPdfToken firstToken)
        {
            this.firstToken = firstToken;
        }

        public bool IsInLine(FFXPdfToken token)
        {
            return firstToken.IsSameLine(token);
        }

        public int CompareTo(FFXPdfToken token)
        {
            return IsInLine(token) ? 0 : (this.firstToken.iYCoord > token.iYCoord ? 1 : -1);
        }

        public void AddNewTokenToLine(FFXPdfToken token)
        {
            FFXPdfToken curToken = firstToken;

            do
            {
                // add new token before current token
                if (curToken.CompareTo(token) == 1)
                {
                    // start of line
                    if (curToken.preToken == null)
                    {
                        this.firstToken = token;
                        token.AddTokenAfter(curToken);
                    }
                    else
                        curToken.AddTokenBefore(token);
                    break;
                }

                // end of line
                if (curToken.nextToken == null)
                {
                    curToken.AddTokenAfter(token);
                    break;
                }

                // move to next token
                curToken = curToken.nextToken;
            }
            while (curToken != null);
        }

        public void AddLineBefore(FFXPdfLine newLine)
        {
            FFXPdfLine preLine = this.preLine;
            preLine.AddLineAfter(newLine);
        }

        public void AddLineAfter(FFXPdfLine newLine)
        {
            // check end of page
            if (this.nextLine != null)
            {
                newLine.nextLine = this.nextLine;
                this.nextLine.preLine = newLine;
            }

            this.nextLine = newLine;
            newLine.preLine = this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.firstToken.ToString());
            FFXPdfToken curToken = this.firstToken;

            while (curToken.nextToken != null)
            {
                sb.Append(" " + curToken.nextToken.ToString());
                curToken = curToken.nextToken;
            }

            return sb.ToString();
        }

        public void ExportLineToXML(FFXExportLevel exportLevel, XmlWriter writer)
        {
            writer.WriteStartElement("Line");

            FFXPdfToken curToken = firstToken;

            if (exportLevel == FFXExportLevel.Line)
            {
                writer.WriteAttributeString("FontFamily", firstToken.sFontFamily);
                writer.WriteAttributeString("Bold", firstToken.bFontBold.ToString());
                writer.WriteAttributeString("X", firstToken.iXCoord.ToString());
                writer.WriteAttributeString("Y", firstToken.iYCoord.ToString());

                writer.WriteString(this.ToString());
            }
            else
            {
                do
                {
                    curToken.ExportTokenToXML(writer);
                    curToken = curToken.nextToken;
                }
                while (curToken != null);
            }

            writer.WriteEndElement();
        }
    }
}

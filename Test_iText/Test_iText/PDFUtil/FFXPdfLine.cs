using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test_iText.PDFUtil
{
    public class FFXPdfLine
    {
        public int iLineNumber { get; private set; }
        public FFXPdfToken cFirstToken { get; private set; }
        public string sLineValue { get; private set; }

        public FFXPdfLine(FFXPdfToken cFirstToken)
        {
            this.cFirstToken = cFirstToken;
        }

        public bool IsInLine(FFXPdfToken token)
        {
            return cFirstToken.IsSameLine(token);
        }

        public void AddNewTokenToLine(FFXPdfToken token)
        {
            FFXPdfToken preToken, curToken;

            preToken = cFirstToken;
            curToken = cFirstToken;

            do
            {
                if (curToken.IsAfterToken(token))
                    break;
                preToken = curToken;
                curToken = curToken.nextToken;
            }
            while (curToken != null);

            preToken.nextToken = token;
            token.nextToken = curToken;
        }

        public void WriteLineToXML (XmlWriter writer)
        {
            FFXPdfToken curToken = cFirstToken;
            StringBuilder sb = new StringBuilder(curToken.sValue);

            while(curToken.nextToken != null)
            {
                curToken = curToken.nextToken;
                sb.Append(" " + curToken.sValue);
            }

            sLineValue = sb.ToString();

            writer.WriteStartElement("Line");
            writer.WriteAttributeString("FontFamily", cFirstToken.sFontFamily);
            writer.WriteAttributeString("Bold", cFirstToken.bFontBold.ToString());
            writer.WriteAttributeString("X", cFirstToken.iXCoord.ToString());
            writer.WriteAttributeString("Y", cFirstToken.iYCoord.ToString());
            writer.WriteString(sLineValue);
            writer.WriteEndElement();
        }
    }
}

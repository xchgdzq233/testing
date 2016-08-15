﻿using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test_iText.PDFUtil
{
    public class FFXPdfToken : IComparable<FFXPdfToken>
    {
        public string sValue { get; private set; }
        public string sFontFamily { get; private set; }
        public bool bFontBold { get; private set; }
        public int iXCoord { get; private set; }
        public int iYCoord { get; private set; }

        public FFXPdfToken preToken { get; set; }
        public FFXPdfToken nextToken { get; set; }

        public FFXPdfToken(TextRenderInfo renderInfo, float fPageHeight)
        {
            // get token text
            this.sValue = renderInfo.GetText();

            // get font
            this.bFontBold = false;
            this.sFontFamily = renderInfo.GetFont().PostscriptFontName;
            if (this.sFontFamily.Contains("Bold"))
            {
                this.bFontBold = true;
                this.sFontFamily = sFontFamily.Replace("-Bold", "");
            }

            // get coordinates
            Vector baseline = renderInfo.GetBaseline().GetStartPoint();
            Vector topRight = renderInfo.GetAscentLine().GetEndPoint();
            Rectangle rRect = new Rectangle(baseline[Vector.I1], baseline[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
            this.iXCoord = PDFUtils.GetInstance().ConvertToPx(rRect.Left);
            this.iYCoord = PDFUtils.GetInstance().ConvertToPx(fPageHeight - rRect.Bottom);
        }

        public bool IsSameLine(FFXPdfToken thatToken)
        {
            return this.iYCoord == thatToken.iYCoord;
        }

        public int CompareTo (FFXPdfToken thatToken)
        {
            return this.iXCoord == thatToken.iXCoord ? 0 : (this.iXCoord > thatToken.iXCoord ? 1 : -1);
        }

        public void AddTokenBefore (FFXPdfToken newToken)
        {
            FFXPdfToken preToken = this.preToken;
            preToken.AddTokenAfter(newToken);
        }

        public void AddTokenAfter (FFXPdfToken newToken)
        {
            // check end of line
            if (this.nextToken != null)
            {
                newToken.nextToken = this.nextToken;
                this.nextToken.preToken = newToken;
            }

            this.nextToken = newToken;
            newToken.preToken = this;

        }

        public override string ToString()
        {
            return this.sValue;
        }

        public void ExportTokenToXML (XmlWriter writer)
        {
            writer.WriteStartElement("Word");
            writer.WriteAttributeString("FontFamily", this.sFontFamily);
            writer.WriteAttributeString("Bold", this.bFontBold.ToString());
            writer.WriteAttributeString("X", this.iXCoord.ToString());
            writer.WriteAttributeString("Y", this.iYCoord.ToString());
            writer.WriteString(this.ToString());
            writer.WriteEndElement();
        }
    }
}

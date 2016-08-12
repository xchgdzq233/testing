using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_iText.PDFUtil
{
    public class FFXPdfToken
    {
        public string sValue { get; private set; }
        public string sFontFamily { get; private set; }
        public bool bFontBold { get; private set; }
        public Rectangle rRect { get; private set; }
        public int iXCoord { get; private set; }
        public int iYCoord { get; private set; }

        public FFXPdfToken preToken { get; set; }
        public FFXPdfToken nextToken { get; set; }

        public FFXPdfToken(TextRenderInfo renderInfo)
        {
            this.sValue = renderInfo.GetText();

            this.bFontBold = false;
            this.sFontFamily = renderInfo.GetFont().PostscriptFontName;
            if (this.sFontFamily.Contains("Bold"))
            {
                this.bFontBold = true;
                this.sFontFamily = sFontFamily.Replace("-Bold", "");
            }

            Vector baseline = renderInfo.GetBaseline().GetStartPoint();
            Vector topRight = renderInfo.GetAscentLine().GetEndPoint();
            this.rRect = new Rectangle(baseline[Vector.I1], baseline[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
            this.iXCoord = PDFUtils.GetInstance().ConvertToPx(rRect.Left);
            this.iYCoord = PDFUtils.GetInstance().ConvertToPx(rRect.Bottom);
        }

        public bool IsSameLine(FFXPdfToken token)
        {
            return this.iYCoord == token.iYCoord;
        }

        public bool IsAfterToken(FFXPdfToken token)
        {
            return this.iXCoord == token.iXCoord;
        }
    }
}

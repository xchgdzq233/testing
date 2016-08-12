using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.util;

namespace Test_iText
{
    public class PDFPage
    {
        public int iPageNumber { get; set; }
        public PDFLine cFirstLine { get; set; }

        public PDFPage(int iPageNumber)
        {
            this.iPageNumber = iPageNumber;
        }

        //public void AddNewToken(PDFToken token)
        //{
        //    if (cFirstLine == null)
        //    {
        //        cFirstLine = token;
        //        return;
        //    }

        //    PDFToken currentToken = cFirstLine;
        //    do
        //    {
        //        // find line
        //        if (currentToken.IsSameLine(token))
        //        {


        //            break;
        //        }

        //        currentToken = currentToken.nextLine;
        //    }
        //    while (currentToken != null);
        //}
    }

    public class PDFLine
    {
        public int iLineNumber { get; set; }
        public LinkedList<PDFToken> lTokens { get; set; }
        public PDFLine cNextLine { get; set; }

        public PDFLine (int iLineNumber)
        {
            this.iLineNumber = iLineNumber;
        }

        public bool IsInLine (PDFToken token)
        {


            return false;
        }
    }

    public class PDFToken
    {
        public string sValue { get; set; }
        public RectangleJ rect { get; set; }
        public PDFToken cNextToken { get; set; }

        public PDFToken(string value)
        {
            this.sValue = value;
        }

        public bool IsSameLine(PDFToken token)
        {
            return true;
        }
    }
}

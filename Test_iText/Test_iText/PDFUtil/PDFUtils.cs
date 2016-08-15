using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_iText.PDFUtil
{
    public enum FFXExportType
    {
        Text,
        XML,
        HTML
    }

    public enum FFXExportLevel
    {
        Page = 1,
        Line = 2,
        Word = 3
    }

    public class PDFUtils
    {
        private static PDFUtils _instance;

        private PDFUtils() { }

        public static PDFUtils GetInstance ()
        {
            if (_instance == null)
                _instance = new PDFUtils();
            return _instance;
        }

        public int ConvertToPx(float f)
        {
            return Convert.ToInt32(f * 1.77164021f);
        }
    }
}

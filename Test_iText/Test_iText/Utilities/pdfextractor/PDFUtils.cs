using System;
using System.IO;

namespace Test_iText.PDFUtil
{
    public enum FFXExportType
    {
        Text = 1,
        XML = 2,
        HTML = 3
    }

    public enum FFXExportLevel
    {
        Page = 1,
        Line = 2,
        Word = 3
    }

    public class ExportConfig
    {
        public FFXExportType exportType { get; private set; }
        public FFXExportLevel exportLevel { get; private set; }
        public string sExportFileName { get; private set; }

        public ExportConfig(FFXExportType exportType, FFXExportLevel exportLevel, string sExportFileName)
        {
            this.exportType = exportType;
            this.exportLevel = exportLevel;
            this.sExportFileName = Path.Combine(@"C:\FFX_Projects\NY_Legislative\Samples", sExportFileName);
        }
    }

    public class PDFUtils
    {
        private static PDFUtils _instance;

        private PDFUtils() { }

        public static PDFUtils GetInstance()
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

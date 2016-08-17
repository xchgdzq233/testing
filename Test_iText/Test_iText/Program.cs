using System;
using Test_iText.PDFUtil;

namespace Test_iText
{
    public class Program
    {
        static void Main(string[] args)
        {
            string sPDFPath = @"C:\FFX_Projects\NY_Legislative\Samples\12037-01-5.pdf";
            string sResultFolder = @"C:\FFX_Projects\NY_Legislative\Samples";

            //FFXPdfDoc.GetInstance(sPDFPath).ExportPDF(FFXExportType.Text, FFXExportLevel.Page, System.IO.Path.Combine(sResultFolder, "result.txt"));
            //FFXPdfDoc.GetInstance(sPDFPath).ExportPDF(FFXExportType.XML, FFXExportLevel.Word, System.IO.Path.Combine(sResultFolder, "result.xml"));
            FFXPdfDoc.GetInstance(sPDFPath).ExportPDF(FFXExportType.HTML, FFXExportLevel.Page, System.IO.Path.Combine(sResultFolder, "result.html"));
        }
    }
}

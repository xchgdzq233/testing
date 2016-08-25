using System;
using System.IO;
using System.Linq;
using WebEditor.Utilities;

namespace Test_iText
{
    public class Program
    {
        static void Main(string[] args)
        {
            string sPDFPath = @"C:\FFX_Projects\NY_Legislative\Samples\12037-01-5.pdf";
            //string sPDFPath = @"C:\FFX_Projects\NY_Legislative\Samples\S06012.pdf";
            
            string sResultFolder = @"C:\FFX_Projects\NY_Legislative\Samples";

            //FFXPdfDoc.GetInstance(sPDFPath).ExportPDF(FFXExportType.Text, FFXExportLevel.Page, System.IO.Path.Combine(sResultFolder, "result.txt"));
            //FFXPdfDoc.GetInstance(sPDFPath).ExportPDF(FFXExportType.XML, FFXExportLevel.Word, System.IO.Path.Combine(sResultFolder, "result.xml"));

            using (StreamReader sr = new StreamReader((new FFXPdfDoc(sPDFPath)).ExportPDF()))
            using (StreamWriter sw = new StreamWriter(Path.Combine(sResultFolder, "result.html")))
            {
                sw.WriteLine(sr.ReadToEnd());
                sw.Flush();
            }
        }
    }
}

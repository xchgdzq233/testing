using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.util;
using System.Xml;
using System.Xml.Xsl;
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
            FFXPdfDoc.GetInstance(sPDFPath).ExportPDF(FFXExportType.XML, FFXExportLevel.Page, System.IO.Path.Combine(sResultFolder, "result.xml"));
        }
    }
}

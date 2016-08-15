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
using Test_iText.PDFUtil;

namespace Test_iText
{
    public class Program
    {
        static void Main(string[] args)
        {
            string sPDFPath = @"C:\FFX_Projects\NY_Legislative\Samples\12037-01-5.pdf";
            string sResultPath = @"C:\FFX_Projects\NY_Legislative\Samples\result.xml";

            FFXPdfDoc.GetInstance(sPDFPath).ReadPDFTo(FFXExportType.XML, FFXExportLevel.Page, sResultPath);
        }
    }
}

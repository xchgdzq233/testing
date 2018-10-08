using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"\\vmware-host\Shared Folders\Downloads\eregs\PublishingOutputProposedRegulationPR2016-091.docx";
            Application app = new Application();
            Document doc = app.Documents.Open(path);

            int count = doc.Paragraphs.Count;
            for (int i = 1; i <= count; ++i)
            {
                string text = doc.Paragraphs[i].Range.Text.Trim();
                Console.WriteLine("P{0}: {1}\n", i, text);
            }

            doc.Close();
            app.Quit();
            Console.ReadKey();
        }
    }
}

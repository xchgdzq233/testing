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
            string path = @"C:\Users\AdminJX\Downloads\PublishingOutputProposedRegulationPR2016-091";
            Application app = new Application();
            Document doc = app.Documents.Open(path);

            int count = doc.Words.Count;
            for (int i = 1; i <= count; ++i)
            {
                string text = doc.Words[i].Text;
                Console.WriteLine("Word {0} = {1}", i, text);
            }

            app.Quit();
        }
    }
}

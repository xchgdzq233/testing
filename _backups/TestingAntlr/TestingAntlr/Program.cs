using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingAntlr.XMLSchema;

namespace TestingAntlr
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream inputStream = File.OpenRead("InputFile.txt");
            //AntlrInputStream input = new AntlrInputStream(inputStream);
            //FileNetQueryGrammarLexer lexer = new FileNetQueryGrammarLexer(input);
            //CommonTokenStream tokens = new CommonTokenStream(lexer);
            //FileNetQueryGrammarParser parser = new FileNetQueryGrammarParser(tokens);
            //IParseTree tree = parser.getUnSingedTempCodifiedSubjectMatters();
            //MyVisitor visitor = new MyVisitor();
            //Console.WriteLine(visitor.Visit(tree));
            
        }
    }
}

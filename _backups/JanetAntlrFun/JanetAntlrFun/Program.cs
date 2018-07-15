using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using FileNet.Api.Core;
using FileNet.Api.Meta;
using JanetAntlrGrammar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanetAntlrFun
{
    class Program
    {
        public static String structurePath, classPath;
        public static Boolean dumpFromFileNet = false, createCode = true;

        static void Main(string[] args)
        {
            structurePath = "docStructure.txt";
            classPath = "eRegsDoc.cs";

            if (dumpFromFileNet)
                DumpFromFileNet();
            if (createCode)
                CreateCode();
        }
        
        static void DumpFromFileNet()
        {
            Console.Title = "Connecting to FileNet...";
            File.Delete(structurePath);

            //connect to FileNet
            CEConnection conn = CEConnection.getCEConnectionInstance();
            Console.Title = "FileNet Connected";

            IClassDescription docClass = Factory.ClassDescription.FetchInstance(conn.ObjectStore, "{E889368E-1659-4933-B90A-726E74EDE363}", new FileNet.Api.Property.PropertyFilter());

            Console.Title = "Dumping Structure";
            WriteToFileAndScreen(docClass);
            Console.Title = "Dumping Finished";
        }

        static void WriteToFileAndScreen(IClassDescription docClass)
        {
            //class definition
            String line = docClass.SymbolicName + " ";
            if (!docClass.SymbolicName.Equals("Document"))
                line += ": " + docClass.SuperclassDescription.SymbolicName + " ";
            line += "{" + Environment.NewLine;
            File.AppendAllText(structurePath, line);

            //property definition
            foreach (IPropertyDescription property in docClass.PropertyDescriptions)
            {
                //system property
                if (property.IsSystemOwned == true)
                    continue;

                //inherited property
                Boolean newProperty = true;
                foreach (IPropertyDescription superclassProperty in docClass.SuperclassDescription.PropertyDescriptions)
                    if (superclassProperty.SymbolicName.Equals(property.SymbolicName))
                    {
                        newProperty = false;
                        break;
                    }

                //new property of the class
                if (newProperty)
                    File.AppendAllText(structurePath, "[" + property.SymbolicName + "]" + Environment.NewLine);

            }

            //close class
            File.AppendAllText(structurePath, "}" + Environment.NewLine + Environment.NewLine);

            //check super class
            if (!docClass.SymbolicName.Equals("Document"))
                WriteToFileAndScreen(docClass.SuperclassDescription);
        }

        static void CreateCode()
        {
            AntlrInputStream input = new AntlrInputStream(File.OpenRead(structurePath));
            
            eRegsGrammarLexer lexer = new eRegsGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            eRegsGrammarParser parser = new eRegsGrammarParser(tokens);

            IParseTree tree = parser.r();
            MyVisitor visitor = new MyVisitor();
            File.WriteAllText(classPath, visitor.Visit(tree).ToString());
            
        }
    }
}

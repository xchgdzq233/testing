using JanetAntlrGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanetAntlrFun
{
    public class MyVisitor : eRegsGrammarBaseVisitor<object>
    {
        public override object VisitR(eRegsGrammarParser.RContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");

            foreach (eRegsGrammarParser.ClassStructureContext classContext in context.classStructure())
                sb.AppendLine(VisitClassStructure(classContext).ToString());

            return sb;
        }

        public override object VisitClassStructure(eRegsGrammarParser.ClassStructureContext context)
        {
            StringBuilder sb = new StringBuilder();

            //class header
            sb.Append("public class " + context.Names().GetText());
            if (!Object.ReferenceEquals(null, context.parentClassName()))
                sb.Append(" : " + context.parentClassName().Names().GetText());
            sb.AppendLine(" {");

            //properties
            foreach (eRegsGrammarParser.PropertyNamesContext propertyContext in context.propertyNames())
                sb.AppendLine(VisitPropertyNames(propertyContext).ToString());

            //close class
            sb.AppendLine("}");

            return sb;
        }

        public override object VisitPropertyNames(eRegsGrammarParser.PropertyNamesContext context)
        {
            return String.Format(@"public {0} {1} {{ get; set; }}", getDataType(context.dataType().GetText()), context.Names().GetText());;
        }

        public static String getDataType(String input)
        {
            switch (input)
            {
                case "STRING": return "String";
                case "OBJECT": return "Object";
                case "GUID": return "Guid";
                case "DATE": return "DateTime";
                case "BINARY": return "Int32";
                case "BOOLEAN": return "Boolean";
                default: return "";
            }
                
        }
    }
}

using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingAntlrGrammar;

namespace TestingAntlr.XMLSchema
{
    public class MyVisitor : XMLSchemaBaseVisitor<object>
    {
        public override object VisitSchemaHeader(XMLSchemaParser.SchemaHeaderContext context)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IParseTree tree in context.children)
            {
                if (!(tree is XMLSchemaParser.NodeContext))
                    continue;
                object child = VisitChildren((XMLSchemaParser.NodeContext)tree);
                if (!Object.ReferenceEquals(null, child))
                    sb.AppendLine(child.ToString());
            }
            return sb.ToString();
        }

        public override object VisitNode(XMLSchemaParser.NodeContext context)
        {
            StringBuilder sb = new StringBuilder();
            if (context.children[1].GetText().Equals("/"))
                return null;
            if (context.NodeType().GetText().Equals("element"))
                CreateElClass(context, ref sb);
            return sb.ToString();
        }
    }
}

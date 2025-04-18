using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterC_
{
    internal interface Node
    {
        public String token_literal();
    }

    internal interface Statement : Node
    {
        public void statement_node();
    }

    internal interface Expression : Node
    {
        public void expression_node();
    }

    internal struct Identifier : Expression
    {
        public Token tok;
        public String value;
        public void expression_node() { }
        public String token_literal() { return tok.m_Literal; }
    }
    internal class LetStatement : Statement
    {
        public Token tok;
        public Identifier name;
        public Expression value;

        public void statement_node() { }
        public String token_literal() { return tok.m_Literal; }
    }

    internal struct Program : Node
    {
        public List<Statement> statements;

        public String token_literal()
        {
            if(statements != null)
            {
                if (statements.Count > 0)
                {
                    return statements[0].token_literal();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}

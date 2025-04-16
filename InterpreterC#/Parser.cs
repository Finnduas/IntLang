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
        String token_literal();
    }

    internal interface Statement : Node
    {
        void statement_node();
    }

    internal interface Expression : Node
    {
        void expression_node();
    }

    internal struct Program : Node
    {
        Statement[] statements;

        public String token_literal()
        {
            if(statements.Length > 0)
            {
                return statements[0].token_literal();
            } 
            else
            {
                return "";
            }
        }
    }
    internal struct Identifier : Expression
    {
        Token tok;
        String value;
        public void expression_node() { }
        public String token_literal() { return tok.m_Literal; }
    }
    internal struct LetStatement : Statement
    {
        Token tok;
        Identifier name;
        Expression value;
        public void statement_node() { }
        public String token_literal() { return tok.m_Literal; }
    }

    internal class Parser
    {

    }
}

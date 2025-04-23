using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InterpreterC_
{
    internal interface Node
    {
        public String token_literal();
        public String _string();
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

        public String _string()
        {
            return value;
        }
    }

    internal struct IntegerLiteral : Expression
    {
        public Token tok;
        public int value;
        public void expression_node() { }
        public String token_literal() { return tok.m_Literal; }

        public String _string()
        {
            return token_literal();
        }
    }

    internal class ExpressionStatement : Statement
    {
        public Token tok;
        public Expression express;

        public void statement_node() { }
        public String token_literal() { return tok.m_Literal; }

        public String _string()
        {
            if(express != null)
            {
                return express._string();
            }

            return "";
        }
    }

    internal class LetStatement : Statement
    {
        public Token tok;
        public Identifier name;
        public Expression value;


        public String _string()
        {
            String s = "";

            s += tok.m_Literal + " ";
            s += name._string();
            s += " = ";
            if(value != null)
            {
                s += value._string();
            }
            s += ";";

            return s;
        }
        public void statement_node() { }
        public String token_literal() { return tok.m_Literal; }
    }

    internal class ReturnStatement : Statement
    {
        public Token tok;
        public Expression returnValue;

        public String token_literal()
        {
            return tok.m_Literal;
        }
        public void statement_node() { }

        public String _string()
        {
            String s = "";

            s += tok.m_Literal + " ";
            if (returnValue != null)
            {
                s += returnValue._string();
            }
            s += ";";

            return s;
        }
    }

    internal struct Program : Node
    {
        public List<Statement> statements;

        public List<Statement> get_statements()
        {
            return statements;
        }

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

        public String _string()
        {
            String s = "";

            for(int i = 0; i < statements.Count(); ++i)
            {
                s += statements[i]._string();
            }

            return s;
        }
    }
}

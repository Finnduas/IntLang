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
        public String token_literal() { return tok.literal; }

        public String _string()
        {
            return value;
        }
    }

    internal struct PrefixExpression : Expression
    {
        public Token tok;
        public Expression right;
        public String _operator;
        public void expression_node() { }
        public String token_literal() { return tok.literal; }

        public String _string()
        {
            return "(" + _operator + right._string() + ")";
        }
    }

    internal struct InfixExpression : Expression
    {
        public Token tok;
        public Expression left;
        public String _operator;
        public Expression right;
        public void expression_node() { }
        public String token_literal() { return tok.literal; }

        public String _string()
        {
            return "(" + left._string() + " " + _operator + " " + right._string() + ")";
        }
    }

    internal struct IntegerLiteral : Expression
    {
        public Token tok;
        public int value;
        public void expression_node() { }
        public String token_literal() { return tok.literal; }

        public String _string()
        {
            return token_literal();
        }
    }

    struct Boolean : Expression
    {
        public Token tok;
        public bool value;

        public Boolean(Token pT, bool pV)
        {
            tok = pT;
            value = pV;
        }

        public void expression_node(){ }
        public String token_literal()
        {
            return tok.literal;
        }
        public String _string()
        {
            return tok.literal;
        }
    }

    internal class ExpressionStatement : Statement
    {
        public Token tok;
        public Expression express;

        public void statement_node() { }
        public String token_literal() { return tok.literal; }

        public String _string()
        {
            if(express != null)
            {
                return express._string();
            }

            return "";
        }
    }

    internal class VarStatement : Statement
    {
        public Token tok;
        public Identifier name;
        public Expression value;


        public String _string()
        {
            String s = "";

            s += tok.literal + " ";
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
        public String token_literal() { return tok.literal; }
    }

    internal class ReturnStatement : Statement
    {
        public Token tok;
        public Expression returnValue;

        public String token_literal()
        {
            return tok.literal;
        }
        public void statement_node() { }

        public String _string()
        {
            String s = "";

            s += tok.literal + " ";
            if (returnValue != null)
            {
                s += returnValue._string();
            }
            s += ";";

            return s;
        }
    }

    internal class BlockStatement : Statement
    {
        public Token tok;
        public List<Statement> statements;
        public void statement_node() { }
        public String token_literal()
        {
            return tok.literal;
        }
        public String _string()
        {
            String tmp = "";
            for(int i = 0; i < statements.Count; ++i)
            {
                tmp += statements[i]._string();
            }
            return tmp;
        }
    }

    internal class FunctionLiteral : Expression
    {
        public Token tok;
        public List<Identifier>? parameters;
        public BlockStatement? contents;

        public void expression_node() { }

        public String token_literal()
        {
            return tok.literal;
        }
        public String _string()
        {
            String tmp = "";
            tmp += "fn (";
            if(parameters != null) 
            { 
                for(int i = 0; i < parameters.Count; ++i)
                {
                    tmp += parameters[i]._string();
                    tmp += ", ";
                }
            }
            tmp += ") { ";
            if(contents != null)
            {
                contents._string();
            }
            tmp += " }";
            
            return tmp;
        }
    }

    internal class CallExpression : Expression
    {
        public Token tok;
        public FunctionLiteral? func;
        public List<Expression>? args;

        public void expression_node() { }
        public String token_literal()
        {
            return tok.literal;
        }
        public String _string()
        {
            String str = "";
            str += func._string();
            str += "(";
            for(int i = 0; i < args.Count; ++i)
            {
                str += args[i]._string();
            }
            str += ")";
            return str;
        }
    }
    internal class IfExpression : Expression
    {
        public Token tok;
        public Expression? condition;
        public BlockStatement? consequence;
        public BlockStatement? alternative;

        public void expression_node() { }
        public String token_literal()
        {
            return tok.literal;
        }
        public String _string()
        {
            String tmp = "";
            tmp += "if";
            tmp += "(";
            tmp += condition!._string();
            tmp += ")";
            tmp += " {";
            tmp += consequence!._string();
            tmp += " }";

            if (alternative != null) {
                tmp += "else";
                tmp += "{";
                tmp += alternative._string();
                tmp += "}";
            }

            return tmp;
    
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

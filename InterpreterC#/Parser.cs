using Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterC_
{
    internal class Parser
    {

        private delegate Expression parse_prefix_expression();
        private delegate Expression parse_infix_expression(Expression before);

        private Dictionary<String, parse_prefix_expression> prefixParsingFuncs;
        private Dictionary<String, parse_infix_expression> infixParsingFuncs;

        private LexerManager lexMan;

        private Token curToken;
        private Token peekToken;

        private List<String> errors;

        public Parser(LexerManager pLexMan)
        {
            errors = new();
            lexMan = pLexMan;
            next_token();
            next_token();
        }

        private void register_prefix_parsing_func(String tokType, parse_prefix_expression prefixFunc)
        {
            prefixParsingFuncs.Add(tokType, prefixFunc);
        }

        private void register_infix_parsing_func(String tokType, parse_infix_expression infixFunc)
        {
            infixParsingFuncs.Add(tokType, infixFunc);
        }

        public List<String> get_errors()
        {
            return errors;
        }

        public void peek_error(String tokType)
        {
            errors.Add("expected next token to be " + tokType + ", got " + peekToken.m_Type);
        }

        public bool check_for_parser_errors()
        {
            if (errors.Count == 0)
            {
                return true;
            }
            else { 

            Console.WriteLine("Provided code has ERRORs:");
            for (int i = 0; i < errors.Count; ++i)
            {
                Console.WriteLine(errors[i]);
            }
            return false;
            }
        }


        public void next_token()
        {
            curToken = peekToken;
            peekToken = lexMan.next_token();
        }

        private bool curToken_is(String tokType)
        {
            if (tokType == curToken.m_Type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool peekToken_is(String tokType)
        {
            if(tokType == peekToken.m_Type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool expected_peek(String tokType)
        {
            if(peekToken_is(tokType)) {
                next_token();
                return true;
            }
            else
            {
                peek_error(tokType);
                return false;
            }
        }
        private LetStatement parse_let_statement()
        {
            LetStatement stmt = new();
            stmt.tok = curToken;

            if(!expected_peek(TokTypes.IDENT))
            {
                return null;
            }
            stmt.name = new Identifier();
            stmt.name.tok = curToken;
            stmt.name.value = curToken.m_Literal;

            //TODO: expressions should be parsed!!!, for now they're just skipped
            while(curToken.m_Type != TokTypes.SEMICOLON)
            {
                next_token();
            }

            return stmt;
        }

        private ReturnStatement parse_return_statement()
        {
            ReturnStatement stmt = new();
            stmt.tok = curToken;

            next_token();
            
            //TODO: expression should be parsed! But I to lazy so skip.

            while(curToken.m_Type != TokTypes.SEMICOLON)
            {
                next_token();
            }
            return stmt;
        }

        private Expression parse_expression(int precedence)
        {
            parse_prefix_expression prefix = prefixParsingFuncs[curToken.m_Type];
            if(prefix == null)
            {
                return null;
            }
            Expression leftExpress = prefix();

            return leftExpress;
        }

        private ExpressionStatement parse_expression_statement()
        {
            ExpressionStatement stmt = new();
            stmt.tok = curToken;
            stmt.express = parse_expression(Precedence.LOWEST);

            if(peekToken_is(TokTypes.SEMICOLON))
            {
                next_token();
            }

            return stmt;
        }

        private Statement parse_statement()
        {
            switch(curToken.m_Type)
            {
                case TokTypes.LET:
                    return parse_let_statement();
                case TokTypes.RETURN:
                    return parse_return_statement();
                default:
                    return parse_expression_statement();
            }
        }

        public Program parse_program()
        {
            Program program = new();
            program.statements = new();

            while (!curToken_is(TokTypes.EOF))
            {
                Statement stmt = parse_statement();
                if (stmt != null)
                {
                    program.statements.Add(stmt);
                }
                next_token();
            }
            return program;
        }
    }
}

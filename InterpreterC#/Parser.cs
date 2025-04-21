using Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterC_
{
    internal class Parser
    {
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

        bool curToken_is(String tokType)
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

        bool peekToken_is(String tokType)
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

        bool expected_peek(String tokType)
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
        public LetStatement parse_let_statement()
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

        public ReturnStatement parse_return_statement()
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

        public Statement parse_statement()
        {
            switch(curToken.m_Type)
            {
                case TokTypes.LET:
                    return parse_let_statement();
                case TokTypes.RETURN:
                    return parse_return_statement();
                default:
                    return null;
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

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

        public Parser(LexerManager pLexMan)
        {
            lexMan = pLexMan;
            next_token();
            next_token();
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
            while(!(curToken.m_Type == TokTypes.SEMICOLON))
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
                default:
                    return null;
            }
        }
        public void append(ref List<Statement> statements, Statement stmt)
        {
            statements.Add(stmt);
        }

        public Program parse_program()
        {
            Program program = new();
            program.statements = new();
            
            while(curToken.m_Type != TokTypes.EOF)
            {
                Statement stmt = parse_statement();
                if(stmt != null)
                {
                    append(ref program.statements, stmt);
                }
                next_token();
            }
            return program;
        }
    }
}

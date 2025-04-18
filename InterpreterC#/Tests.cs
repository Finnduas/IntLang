using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterpreterC_;
using Lookup;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;

namespace Tests
{

    struct TestToken
    {
        public String expectedType;
        public String expectedLiteral;
        public TestToken (String pET, String pEL)
        {
            expectedType = pET;
            expectedLiteral = pEL;
        }
    }
    internal class _Tests
    {
        public void test_lexer(String input, TestToken[] tests, int j)
        {
            LexerManager lexerManager = new();
            lexerManager.init_lexer(input);

            for (int i = 0; i<tests.Length; i++)
            {
                Token tok = lexerManager.next_token();

                if(tok.m_Type != tests[i].expectedType)
                {
                    throw new Exception("ERROR: tokenType wrong; expexted: " + (tests[i].expectedType) + " received: " + tok.m_Type + " at i = " + i);
                }

                if (tok.m_Literal != tests[i].expectedLiteral)
                {
                    throw new Exception("ERROR: tokenLiteral wrong; expexted: " + (tests[i].expectedLiteral) + " received: " + tok.m_Literal + " at i = " + i);
                }
            }

            Console.WriteLine(j +" - ok");
        }
        internal struct TestIdentifier
        {
            public String expectedIdentifier;

            public TestIdentifier(String pEI)
            {
                expectedIdentifier = pEI;
            }
        }
        public void test_parser(String input, TestIdentifier[] testIdentifiers, int j)
        {
            LexerManager lexMan = new();
            lexMan.init_lexer(input);
            Parser par = new(lexMan);

            InterpreterC_.Program program = par.parse_program();
            if(program.token_literal() == "")
            {
                throw new Exception("ERROR: program does not contain anything!");
            }
            
            if(program.statements.Count != 3)
            {
                throw new Exception("ERROR: program.statements does not contain 3 members");
            }

            for(int i = 0; i < testIdentifiers.Length; i++)
            {
                Statement stmt = program.statements[i];
                test_let_statement(stmt, testIdentifiers[i].expectedIdentifier);
            }

            Console.WriteLine(j + " - ok");
        }

        void test_let_statement(Statement stmt, String exIdent)
        {
            if(stmt.token_literal() != "let")
            {
                throw new Exception("ERROR: statement is not 'let', got: " + stmt.token_literal());
            }

            bool ok = stmt is LetStatement letStmt;
            if (!ok)
            {
                throw new Exception("ERROR: stmt is not LetStatement, got: " + stmt);
            }
            else
            {
                letStmt = (LetStatement)stmt;
            }

            if(letStmt.name.value != exIdent)
            {
                throw new Exception("ERROR: Identifier was not expected Identifier. Expected: " + exIdent + " | Received: " + letStmt.name.value);
            }

            if (letStmt.name.token_literal() != exIdent)
            {
                throw new Exception("ERROR: Identifiers token literal was NOT Identifiers value. Expected: " + exIdent + " | Received: " + letStmt.name.token_literal() + " - is the stmt to letStmt conversion faulty?");
            }
        }
    }
}

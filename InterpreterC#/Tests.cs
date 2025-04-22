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
        public TestToken(String pET, String pEL)
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

            for (int i = 0; i < tests.Length; i++)
            {
                Token tok = lexerManager.next_token();

                if (tok.m_Type != tests[i].expectedType)
                {
                    throw new Exception("ERROR: tokenType wrong; expexted: " + (tests[i].expectedType) + " received: " + tok.m_Type + " at i = " + i);
                }

                if (tok.m_Literal != tests[i].expectedLiteral)
                {
                    throw new Exception("ERROR: tokenLiteral wrong; expexted: " + (tests[i].expectedLiteral) + " received: " + tok.m_Literal + " at i = " + i);
                }
            }

            Console.WriteLine(j + " - ok");
        }
        internal struct TestIdentifier
        {
            public String expectedIdentifier;

            public TestIdentifier(String pEI)
            {
                expectedIdentifier = pEI;
            }
        }
        public void test_parser(String input, TestIdentifier[] testIdentifiers, int testIndex)
        {
            LexerManager lexMan = new();
            lexMan.init_lexer(input);
            Parser par = new(lexMan);

            InterpreterC_.Program program = par.parse_program();
            if (program.token_literal() == "")
            {
                throw new Exception("ERROR: program does not contain anything!");
            }

            if (program.statements.Count != 3)
            {
                throw new Exception("ERROR: program.statements does not contain 3 members");
            }

            for (int i = 0; i < testIdentifiers.Length; i++)
            {
                Statement stmt = program.statements[i];
                switch (testIndex)
                {
                    case 0:
                        test_let_statement(stmt, testIdentifiers[i].expectedIdentifier);
                        break;
                    case 1:
                        test_return_statement(stmt);
                        break;
                }
            }


            bool ok = par.check_for_parser_errors();
            if (!ok)
            {
                throw new Exception("The provided code has ERRORs -> check the console for further information");
            }
            Console.WriteLine(testIndex + " - ok");
        }
     void test_let_statement(Statement stmt, String exIdent)
        {
            if (stmt.token_literal() != "let")
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

            if (letStmt.name.value != exIdent)
            {
                throw new Exception("ERROR: Identifier was not expected Identifier. Expected: " + exIdent + " | Received: " + letStmt.name.value);
            }

            if (letStmt.name.token_literal() != exIdent)
            {
                throw new Exception("ERROR: Identifiers token literal was NOT Identifiers value. Expected: " + exIdent + " | Received: " + letStmt.name.token_literal() + " - is the stmt to letStmt conversion faulty?");
            }
        }

        void test_return_statement(Statement stmt)
        {

            if (stmt.token_literal() != "return")
            {
                throw new Exception("ERROR: statement is not 'return', got: " + stmt.token_literal());
            }

            bool ok = stmt is ReturnStatement retStmt;
            if (!ok)
            {
                throw new Exception("ERROR: stmt is not ReturnStatement, got: " + stmt);
            }
        }

        public void test__string()
        { 
            InterpreterC_.Program pro = new();
            pro.statements = new();
            LetStatement letStmt = new();

            letStmt.tok = new(TokTypes.LET, "let");
            
            letStmt.name = new Identifier();
            letStmt.name.tok = new(TokTypes.IDENT, "myVar");
            letStmt.name.value = "myVar";

            InterpreterC_.Identifier ident = new();
            ident.tok = new(TokTypes.IDENT, "myOtherVar");
            ident.value = "myOtherVar";
            letStmt.value = ident;
            
            
            pro.statements.Add(letStmt);

            if(pro._string() != "let myVar = myOtherVar;")
            {
                throw new Exception("ERROR: _string function does not work! Expected: " + "let myVar = myOtherVar;" + ", received: " + pro._string());
            }
            Console.WriteLine("_string() - ok");
        }

        public void test_expression_parsing()
        {
            String input = "foobar;";

            InterpreterC_.LexerManager lexMan = new();
            lexMan.init_lexer(input);
            InterpreterC_.Parser par = new(lexMan);

            InterpreterC_.Program pro = par.parse_program();
            par.check_for_parser_errors();
            if(pro.statements.Count != 1)
            {
                throw new Exception("ERROR: program has to few or to many statements, received: " + pro.statements.Count);
            }
            bool ok = pro.statements[0] is InterpreterC_.ExpressionStatement exStmt;
            exStmt = (InterpreterC_.ExpressionStatement)pro.statements[0];
            if (!ok)
            {
                throw new Exception("ERROR: statement is not an ExpressionStatement, received: " + pro.statements[0]);
            }

            ok = exStmt.express is InterpreterC_.Identifier ident;
            ident = (Identifier)exStmt.express;
            if(!ok)
            {
                throw new Exception("ERROR: expression was not an identifier");
            }
            if(ident.value != "foobar")
            {
                throw new Exception("ERROR: identifier does not have correct value, received: " + ident.value);
            }
            if (ident.token_literal() != "foobar")
            {
                throw new Exception("ERROR: identifier does not have correct token literal, received: " + ident.token_literal());
            }

        }
    }
}

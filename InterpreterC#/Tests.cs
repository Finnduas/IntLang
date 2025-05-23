﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterpreterC_;
using Lookup;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace Tests
{
    //TODO: make the testing structure homogenous
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
                par.check_for_parser_errors(); throw new Exception("ERROR: program does not contain anything!");
            }

            if (program.statements.Count != 3)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: program.statements does not contain 3 members");
            }

            for (int i = 0; i < testIdentifiers.Length; i++)
            {
                Statement stmt = program.statements[i];
                switch (testIndex)
                {
                    case 0:
                        test_let_statement(stmt, testIdentifiers[i].expectedIdentifier, ref par);
                        break;
                    case 1:
                        test_return_statement(stmt, ref par);
                        break;
                }
            }


            bool ok = par.check_for_parser_errors();
            if (!ok)
            {
                par.check_for_parser_errors(); throw new Exception("The provided code has ERRORs -> check the console for further information");
            }
            Console.WriteLine(testIndex + " - ok");
        }
        void test_let_statement(Statement stmt, String exIdent, ref Parser par)
        {
            if (stmt.token_literal() != "let")
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: statement is not 'let', got: " + stmt.token_literal());
            }

            bool ok = stmt is LetStatement letStmt;
            if (!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: stmt is not LetStatement, got: " + stmt);
            }
            else
            {
                letStmt = (LetStatement)stmt;
            }

            if (letStmt.name.value != exIdent)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: Identifier was not expected Identifier. Expected: " + exIdent + " | Received: " + letStmt.name.value);
            }

            if (letStmt.name.token_literal() != exIdent)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: Identifiers token literal was NOT Identifiers value. Expected: " + exIdent + " | Received: " + letStmt.name.token_literal() + " - is the stmt to letStmt conversion faulty?");
            }
        }

        void test_return_statement(Statement stmt, ref Parser par)
        {

            if (stmt.token_literal() != "return")
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: statement is not 'return', got: " + stmt.token_literal());
            }

            bool ok = stmt is ReturnStatement retStmt;
            if (!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: stmt is not ReturnStatement, got: " + stmt);
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

            if (pro._string() != "let myVar = myOtherVar;")
            {
                throw new Exception("ERROR: _string function does not work! Expected: " + "let myVar = myOtherVar;" + ", received: " + pro._string());
            }
            Console.WriteLine("3 - ok");
        }

        public void test_identifier_expression_parsing()
        {
            String input = "foobar;";

            LexerManager lexMan = new();
            lexMan.init_lexer(input);
            Parser par = new(lexMan);

            InterpreterC_.Program pro = par.parse_program();
            par.check_for_parser_errors();
            if (pro.statements.Count != 1)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: program has to few or to many statements, received: " + pro.statements.Count);
            }
            bool ok = pro.statements[0] is InterpreterC_.ExpressionStatement exStmt;
            exStmt = (InterpreterC_.ExpressionStatement)pro.statements[0];
            if (!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: statement is not an ExpressionStatement, received: " + pro.statements[0]);
            }

            ok = exStmt.express is InterpreterC_.Identifier ident;
            ident = (Identifier)exStmt.express;
            if (!ok)
            {
                par.check_for_parser_errors(); par.check_for_parser_errors(); throw new Exception("ERROR: expression was not an identifier");
            }

            test_identifier(ident, "foobar", ref par);

            Console.WriteLine("4 - ok");
        }

        public void test_integer_expression_parsing()
        {
            String input = "5;";

            InterpreterC_.LexerManager lexMan = new();
            lexMan.init_lexer(input);
            InterpreterC_.Parser par = new(lexMan);

            InterpreterC_.Program pro = par.parse_program();
            if (pro.statements.Count != 1)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: program has to few or to many statements, received: " + pro.statements.Count);
            }
            bool ok = pro.statements[0] is InterpreterC_.ExpressionStatement exStmt;
            exStmt = (InterpreterC_.ExpressionStatement)pro.statements[0];
            if (!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: statement is not an ExpressionStatement, received: " + pro.statements[0]);
            }

            ok = exStmt.express is InterpreterC_.IntegerLiteral intLit;
            intLit = (IntegerLiteral)exStmt.express;
            if (!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: expression was not an identifier");
            }
            if (intLit.value != 5)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: identifier does not have correct value, received: " + intLit.value);
            }
            if (intLit.token_literal() != "5")
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: identifier does not have correct token literal, received: " + intLit.token_literal());
            }
            Console.WriteLine("5 - ok");
        }

        struct prefixTest
        {
            public String input;
            public String _operator;
            public int integerValue;
        }
        public void test_parsing_prefix_expressions()
        {

            prefixTest[] prefixTests = new prefixTest[2];
            prefixTests[0] = new();
            prefixTests[1] = new();

            prefixTests[0].input = "!5";
            prefixTests[0]._operator = "!";
            prefixTests[0].integerValue = 5;

            prefixTests[1].input = "-15";
            prefixTests[1]._operator = "-";
            prefixTests[1].integerValue = 15;

            // 0 = {"!5", "!", 5}; 1 = {"-15", "-", 15};

            for (int i = 0; i < prefixTests.Length; ++i)
            {
                LexerManager lexMan = new();
                lexMan.init_lexer(prefixTests[i].input);
                Parser par = new(lexMan);
                InterpreterC_.Program pro = par.parse_program();

                if (pro.statements.Count != 1)
                {
                    par.check_for_parser_errors(); throw new Exception("ERROR: program contains " + pro.statements.Count + " statements; expected: 1");
                }

                bool ok = pro.statements[0] is ExpressionStatement stmt;
                stmt = (ExpressionStatement)pro.statements[0];
                if (!ok)
                {
                    par.check_for_parser_errors(); throw new Exception("ERROR: statement is not ExpressionStatement");
                }

                ok = stmt.express is PrefixExpression exp;
                exp = (PrefixExpression)stmt.express;
                if (!ok)
                {
                    par.check_for_parser_errors(); throw new Exception("ERROR: ExpressionStatements expression is not PrefixExpression, got: " + stmt.express);
                }
                if (exp._operator != prefixTests[i]._operator)
                {
                    par.check_for_parser_errors(); throw new Exception("ERROR: expression does not have correct operator, expected: " + prefixTests[i]._operator + ", got: " + exp._operator);
                }
                test_integer_literal(exp.right, prefixTests[i].integerValue, ref par);
            }

            Console.WriteLine("6 - ok");
        }

        public void test_integer_literal(InterpreterC_.Expression exp, int value, ref Parser par)
        {
            bool ok = exp is InterpreterC_.IntegerLiteral intLit;
            intLit = (IntegerLiteral)exp;
            if (!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: exp is not IntegerLiteral, got: " + exp);
            }
            if (intLit.value != value)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: IntegerLiteral did not have expected value of " + value + ", received: " + intLit.value);
            }
            if (intLit.token_literal() != value.ToString())
            {
                par.check_for_parser_errors(); throw new Exception("FATAL ERROR: something has gone seriously wrong... Got: " + intLit.token_literal());
            }
        }

        void test_identifier(InterpreterC_.Expression exp, String value, ref Parser par)
        {
            bool ok = exp is Identifier ident;
            ident = (Identifier)exp;
            if(!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: expression is not identifier!");
            }
            if(ident.value != value)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: identifier does not have expected value of " + value + ", received " + ident.value);
            }
            if (ident.token_literal() != value)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: identifiers token literal does not have expected value of " + value + ", received " + ident.token_literal());
            }
        }

        void test_literalExpression(ref Parser par, Expression exp, String expected, String type)
        {
            switch(type)
            {
                case "int":
                    int tmp = int.Parse(expected);
                    test_integer_literal(exp, tmp, ref par);
                    break;
                case "string":
                    test_identifier(exp, expected, ref par);
                    break;
            }
        }

        void test_infix_expression(Expression exp, String left, String leftType, String _operator, String right, String rightType, ref Parser par)
        {
            if (left == null) 
            { 
                throw new ArgumentNullException(nameof(left));
            }
            if (right == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            bool ok = exp is InfixExpression infx;
            infx = (InfixExpression)exp;
            if(!ok)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: Expression is not InfixExpression");
            }

            test_literalExpression(ref par, infx.left, left, leftType);
            if (infx._operator != _operator)
            {
                par.check_for_parser_errors(); throw new Exception("ERROR: operator is not expected operator ( " + _operator + " )");
            }
            test_literalExpression(ref par, infx.right, right, rightType);
        }

        internal struct infixTest
        {
            public String input;
            public int leftValue;
            public String _operator;
            public int rightValue;

            public infixTest(String pIn, int pLV, String pOp, int pRV)
            {
                input = pIn;
                leftValue = pLV;
                _operator = pOp;
                rightValue = pRV;
            }
        }

        public void test_parsing_infix_expressions()
        {
            infixTest[] infixTests = new infixTest[8];
            infixTests[0] = new("5 + 5", 5, "+", 5);
            infixTests[1] = new("5 - 5", 5, "-", 5);
            infixTests[2] = new("5 * 5", 5, "*", 5);
            infixTests[3] = new("5 / 5", 5, "/", 5);
            infixTests[4] = new("5 < 5", 5, "<", 5);
            infixTests[5] = new("5 > 5", 5, ">", 5);
            infixTests[6] = new("5 == 5", 5, "==", 5);
            infixTests[7] = new("5 != 5", 5, "!=", 5);


            for (int i = 0; i < infixTests.Length; ++i)
            {
                InterpreterC_.LexerManager lexMan = new();
                lexMan.init_lexer(infixTests[i].input);
                InterpreterC_.Parser par = new(lexMan);
                InterpreterC_.Program pro = par.parse_program();

                if(pro.statements.Count != 1)
                {
                    par.check_for_parser_errors(); throw new Exception("ERROR: expexted one statement, received: " + pro.statements.Count);
                }
                bool ok = pro.statements[0] is ExpressionStatement expStmt;
                expStmt = (ExpressionStatement)pro.statements[0];
                if(!ok)
                {
                    par.check_for_parser_errors(); throw new Exception("ERROR: Statement is not ExpressionStatement, received: " + pro.statements[0]);
                }

                test_infix_expression
                    (
                    expStmt.express, 
                    infixTests[i].leftValue.ToString(), 
                    "int", 
                    infixTests[i]._operator, 
                    infixTests[i].rightValue.ToString(), 
                    "int", 
                    ref par
                    );
            }

            Console.WriteLine("7 - ok");
        }

        struct PrecedenceTest
        {
            public String input;
            public String expected;

            public PrecedenceTest(String pIn, String pEx)
            {
                input = pIn;
                expected = pEx;
            }
        }
        public void test_precedence_parsing()
        {
            PrecedenceTest[] precedenceTests = new PrecedenceTest[4];
            precedenceTests[0] = new("!-a", "(!(-a))");
            precedenceTests[1] = new("-a * b / c", "(((-a) * b) / c)");
            precedenceTests[2] = new("a * b / c + e * 5 - f", "((((a * b) / c) + (e * 5)) - f)");
            precedenceTests[3] = new("a + b; a * b", "(a + b)(a * b)");

            for (int i = 0; i < precedenceTests.Length; ++i)
            {
                InterpreterC_.LexerManager lexMan = new();
                lexMan.init_lexer(precedenceTests[i].input);
                InterpreterC_.Parser par = new(lexMan);
                InterpreterC_.Program pro = par.parse_program();

                String actual = pro._string();
                if (actual != precedenceTests[i].expected)
                {
                    throw new Exception("ERROR: failed to parse precedence correctly! Expected: " + precedenceTests[i].expected + ", got: " + actual);
                }
            }

            Console.WriteLine("8 - ok");
        }
    }
}

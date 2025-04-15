using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterpreterC_;
using Consts;
using System.Runtime.InteropServices;

namespace Tests
{

    struct Test
    {
        public String expectedType;
        public String expectedLiteral;
        public Test (String pET, String pEL)
        {
            expectedType = pET;
            expectedLiteral = pEL;
        }
    }
    internal class LexerTests
    {
        public void test_next_token(String input, Test[] tests, int j)
        {
            Console.WriteLine("Running tests...");

            Interpreter interpreter = new();
            interpreter.init_lexer(input);

            for (int i = 0; i<tests.Length; i++)
            {
                Token tok = interpreter.next_token(input);

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
    }
}

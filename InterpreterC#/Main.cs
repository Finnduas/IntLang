using Lookup;
using InterpreterC_;
using Tests;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Reflection;
using System.Runtime.InteropServices;

LexerManager lexerManager = new();
_Tests tests = new();
String code = "";//read_code("C:\\Users\\mikaa\\C\\GitHub\\IntLang\\InterpreterC#\\assets\\examplePrograms\\creation_of_some_variables.simp");
static String read_code(String path)
{
    String code;
    String line;

    StreamReader sr = new(path);
    line = sr.ReadLine();

    if (line == null) //assert
    {
        throw new Exception("ERROR: reading of file in specified directory failed OR file is empty");
    }

    code = line;
    while (line != null)
    {
        line = sr.ReadLine();
        code += "\n";
        code += line;
    }
    sr.Close();

    return code;
}

void lexer_tests() // TODO: make that shit smaller, or remove
{
    tests.test_lexer();
}

void parser_tests()
{
    tests.test_parser();

    tests.test__string();

    tests.test_identifier_expression_parsing();

    tests.test_integer_expression_parsing();

    tests.test_parsing_prefix_expressions();

    tests.test_parsing_infix_expressions();

    tests.test_precedence_parsing();

    tests.test_boolean_expression_parsing();
    
    tests.test_if_else_Expression();

    tests.test_function_literals();
}

void run_tests() 
{
    Console.WriteLine("Running lexer tests...");
    lexer_tests();
    Console.WriteLine("");
    Console.WriteLine("Lexer tests passed!\n...");

    Console.WriteLine("Running parser tests...");
    parser_tests();
    Console.WriteLine("");
    Console.WriteLine("Parser tests passed!");

    Console.WriteLine("---------------------------------");
    Console.WriteLine("All tests passed!");
}

//-------------------------------------------------------------------------------------

run_tests();
LexerManager lexLexMan = new();
///* //Benchmarks
BenchmarkSwitcher
    .FromAssembly(Assembly.GetExecutingAssembly())
    .Run(args);
//*/

if (code != "" && code != null)
{
    //TODO: interpret the FILE
}
else //start a ReadEvalPrintLoop
{
    Console.WriteLine("");
    Console.WriteLine("No code provided: Entering interactive mode...");
    
REPLStart:
    String input = "";
    Token tok = new();
    
    Console.Write(">> ");
    input = Console.ReadLine();
    
    if(input == null || input == "")
    {
        Console.WriteLine("-> No input provided");
        goto REPLStart;
    }
    else
    {
        lexerManager.init_lexer(input);
        tok = lexerManager.next_token();
        while (tok.m_Type != TokTypes.EOF)
        {
            Console.WriteLine("Type: " + tok.m_Type + " | Literal: " + tok.m_Literal);
            tok = lexerManager.next_token(); 
        }
        Console.WriteLine("Type: " + tok.m_Type + " | Literal: " + tok.m_Literal);
        goto REPLStart;
    }
}
using Lookup;
using InterpreterC_;
using Tests;


Interpreter interpreter = new();
LexerTests lexerTests = new();
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

void run_tests() 
{
    Console.WriteLine("Running tests...");
    String testInput0 = "=+(){},;*/-";
    Test[] expectedTestResult0 =
        [
        new(TokTypes.ASSIGN, "="),
            new(TokTypes.PLUS, "+"),
            new(TokTypes.LPAREN, "("),
            new(TokTypes.RPAREN, ")"),
            new(TokTypes.LBRACE, "{"),
            new(TokTypes.RBRACE, "}"),
            new(TokTypes.COMMA, ","),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.ASTERISK, "*"),
            new(TokTypes.SLASH, "/"),
            new(TokTypes.MINUS, "-"),
            new(TokTypes.EOF, "")
        ];

    lexerTests.test_next_token(testInput0, expectedTestResult0, 0);

    String testInput1 =
    "let five = 5;"
    + "let ten = 10;"
    + "let add = fn(x, y) {"
    + "x + y;"
    + "};"
    + "let result = add(five, ten) ;";
    Test[] expectedTestResult1 =
        [
        new(TokTypes.LET, "let"),
        new(TokTypes.IDENT, "five"),
        new(TokTypes.ASSIGN, "="),
        new(TokTypes.INT, "5"),
        new(TokTypes.SEMICOLON, ";"),
        new(TokTypes.LET, "let"),
        new(TokTypes.IDENT, "ten"),
        new(TokTypes.ASSIGN, "="),
        new(TokTypes.INT, "10"),
        new(TokTypes.SEMICOLON, ";"),
        new(TokTypes.LET, "let"),
        new(TokTypes.IDENT, "add"),
        new(TokTypes.ASSIGN, "="),
        new(TokTypes.FUNCTION, "fn"),
        new(TokTypes.LPAREN, "("),
        new(TokTypes.IDENT, "x"),
        new(TokTypes.COMMA, ","),
        new(TokTypes.IDENT, "y"),
        new(TokTypes.RPAREN, ")"),
        new(TokTypes.LBRACE, "{"),
        new(TokTypes.IDENT, "x"),
        new(TokTypes.PLUS, "+"),
        new(TokTypes.IDENT, "y"),
        new(TokTypes.SEMICOLON, ";"),
        new(TokTypes.RBRACE, "}"),
        new(TokTypes.SEMICOLON, ";"),
        new(TokTypes.LET, "let"),
        new(TokTypes.IDENT, "result"),
        new(TokTypes.ASSIGN, "="),
        new(TokTypes.IDENT, "add"),
        new(TokTypes.LPAREN, "("),
        new(TokTypes.IDENT, "five"),
        new(TokTypes.COMMA, ","),
        new(TokTypes.IDENT, "ten"),
        new(TokTypes.RPAREN, ")"),
        new(TokTypes.SEMICOLON, ";"),
        ];

    lexerTests.test_next_token(testInput1, expectedTestResult1, 1);

    String testInput2 =
    "10 == 10;"
    +"10 != 9;"
    +"!-/*5;"
    + "5 < 10 > 5;"
    + "\n"
    + "if (5 < 10) {\n"
    + "return true;"
    + "} else {\n"
    + "return false;"
    + "}"
    +"let five = 5;"
    + "let ten = 10;"
    + "let add = fn(x, y) {"
    + "x + y;"
    + "};"
    + "let result = add(five, ten) ;"
    ;
    Test[] expectedTestResult2 =
        [
            new(TokTypes.INT, "10"),
            new(TokTypes.EQ, "=="),
            new(TokTypes.INT, "10"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.INT, "10"),
            new(TokTypes.NOT_EQ, "!="),
            new(TokTypes.INT, "9"),
            new(TokTypes.SEMICOLON, ";"),

            new(TokTypes.BANG, "!"),
            new(TokTypes.MINUS, "-"),
            new(TokTypes.SLASH, "/"),
            new(TokTypes.ASTERISK, "*"),
            new(TokTypes.INT, "5"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.INT, "5"),
            new(TokTypes.LT, "<"),
            new(TokTypes.INT, "10"),
            new(TokTypes.GT, ">"),
            new(TokTypes.INT, "5"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.IF, "if"),
            new(TokTypes.LPAREN, "("),
            new(TokTypes.INT, "5"),
            new(TokTypes.LT, "<"),
            new(TokTypes.INT, "10"),
            new(TokTypes.RPAREN, ")"),
            new(TokTypes.LBRACE, "{"),
            new(TokTypes.RETURN, "return"),
            new(TokTypes.TRUE, "true"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.RBRACE, "}"),
            new(TokTypes.ELSE, "else"),
            new(TokTypes.LBRACE, "{"),
            new(TokTypes.RETURN, "return"),
            new(TokTypes.FALSE, "false"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.RBRACE, "}"),

            new(TokTypes.LET, "let"),
            new(TokTypes.IDENT, "five"),
            new(TokTypes.ASSIGN, "="),
            new(TokTypes.INT, "5"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.LET, "let"),
            new(TokTypes.IDENT, "ten"),
            new(TokTypes.ASSIGN, "="),
            new(TokTypes.INT, "10"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.LET, "let"),
            new(TokTypes.IDENT, "add"),
            new(TokTypes.ASSIGN, "="),
            new(TokTypes.FUNCTION, "fn"),
            new(TokTypes.LPAREN, "("),
            new(TokTypes.IDENT, "x"),
            new(TokTypes.COMMA, ","),
            new(TokTypes.IDENT, "y"),
            new(TokTypes.RPAREN, ")"),
            new(TokTypes.LBRACE, "{"),
            new(TokTypes.IDENT, "x"),
            new(TokTypes.PLUS, "+"),
            new(TokTypes.IDENT, "y"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.RBRACE, "}"),
            new(TokTypes.SEMICOLON, ";"),
            new(TokTypes.LET, "let"),
            new(TokTypes.IDENT, "result"),
            new(TokTypes.ASSIGN, "="),
            new(TokTypes.IDENT, "add"),
            new(TokTypes.LPAREN, "("),
            new(TokTypes.IDENT, "five"),
            new(TokTypes.COMMA, ","),
            new(TokTypes.IDENT, "ten"),
            new(TokTypes.RPAREN, ")"),
            new(TokTypes.SEMICOLON, ";"),
        ];

    lexerTests.test_next_token(testInput2, expectedTestResult2, 2);
    Console.WriteLine("All tests passed!");
}

run_tests();

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
        interpreter.init_lexer(input);
        tok = interpreter.next_token();
        while (tok.m_Type != TokTypes.EOF)
        {
            tok = interpreter.next_token();
            Console.WriteLine("Type: " + tok.m_Type + " | Literal: " + tok.m_Literal);
        }
        goto REPLStart;
    }
}
namespace Lookup
{
    public struct TokTypes
    {
        public const String ILLEGAL = "ILLEGAL";
        public const String EOF = "EOF";

        public const String IDENT = "IDENT";
        public const String INT = "INT";

        public const String ASSIGN = "=";
        public const String PLUS = "+";
        public const String MINUS = "-";
        public const String BANG = "!";
        public const String ASTERISK = "*";
        public const String SLASH = "/";

        public const String LT = "<";
        public const String GT = ">";
        public const String EQ = "==";
        public const String NOT_EQ = "!=";


        public const String COMMA = ",";
        public const String SEMICOLON = ";";

        public const String LPAREN = "(";
        public const String RPAREN = ")";
        public const String LBRACE = "{";
        public const String RBRACE = "}";

        public const String FUNCTION = "FUNCTION";
        public const String LET = "LET";
        public const String TRUE = "TRUE";
        public const String FALSE = "FALSE";
        public const String IF = "IF";
        public const String ELSE = "ELSE";
        public const String RETURN = "RETURN";
    }

    public class KeyWords
    {

        public Dictionary<String, String> keyWords = new Dictionary<String, String>
        {
            {"fn" , TokTypes.FUNCTION},
            {"let" , TokTypes.LET},
            {"true" , TokTypes.TRUE},
            {"false" , TokTypes.FALSE},
            {"if" , TokTypes.IF},
            {"else" , TokTypes.ELSE},
            {"return" , TokTypes.RETURN},

        };

        public KeyWords()
        {

        }

        public String lookup(String literal)
        {
            if (keyWords.TryGetValue(literal, out String result))
            {
                return result;
            }
            else
            {
                return TokTypes.IDENT;
            }
        }
    }

    public struct Precedence
    {
        public const int LOWEST = 0;
        public const int EQUALS = 1;
        public const int LESSGREATER = 2;
        public const int SUM = 3;
        public const int PRODUCT = 4;
        public const int PREFIX = 5;
        public const int CALL = 6;
    }
}
namespace Consts
{
    public struct TokTypes
    {
        public const String ILLEGAL = "ILLEGAL";
        public const String EOF = "EOF";

        public const String IDENT = "IDENT";
        public const String INT = "INT";

        public const String ASSIGN = "=";
        public const String PLUS = "+";

        public const String COMMA = ",";
        public const String SEMICOLON = ";";

        public const String LPAREN = "(";
        public const String RPAREN = ")";
        public const String LBRACE = "[";
        public const String RBRACE = "]";

        public const String FUNCTION = "FUNCTION";
        public const String LET = "LET";
    }

    public class KeyWords
    {

        public Dictionary<String, String> keyWords = new Dictionary<String, String>
        {
            {"fn" , TokTypes.FUNCTION},
            {"let" , TokTypes.LET}
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
}
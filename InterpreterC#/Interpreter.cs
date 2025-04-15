using Consts;

namespace InterpreterC_
{
    struct Token
    {
        public String m_Type;
        public String m_Literal;

        public Token(String pT, String pL)
        {
            m_Type = pT;
            m_Literal = pL;
        }
    }

    struct Lexer
    {
        public String input;
        public int position;
        public int readPosition;
        public char ch;

        public Lexer() 
        {
            input = "";
            position = 0;
            readPosition = 0;
            ch = (char)0;
        }

    }

    internal class Interpreter
    {
        KeyWords keyWords = new();
        Lexer lex = new();
        public void init_lexer(String code)
        {
            lex.input = code;
            read_char(ref lex);
        }

        public Token next_token(String code)
        {
            Token tok = new();

            switch(lex.ch)
            {
                case '=':
                    tok = new(TokTypes.ASSIGN, lex.ch.ToString());
                    break;
                case ';':
                    tok = new(TokTypes.SEMICOLON, lex.ch.ToString());
                    break;
                case '(':
                    tok = new(TokTypes.LPAREN, lex.ch.ToString());
                    break;
                case ')':
                    tok = new(TokTypes.RPAREN, lex.ch.ToString());
                    break;
                case ',':
                    tok = new(TokTypes.COMMA, lex.ch.ToString());
                    break;
                case '+':
                    tok = new(TokTypes.PLUS, lex.ch.ToString());
                    break;
                case '{':
                    tok = new(TokTypes.LBRACE, lex.ch.ToString());
                    break;
                case '}':
                    tok = new(TokTypes.RBRACE, lex.ch.ToString());
                    break;
                case (char)0:
                    tok = new(TokTypes.EOF, "");
                    break;
                default:
                    if(is_letter(lex.ch))
                    {
                        tok.m_Literal = read_identifier(ref lex);
                        tok.m_Type = keyWords.lookup(tok.m_Literal); 
                        return tok;
                    }
                    else
                    {
                        tok = new(TokTypes.ILLEGAL, lex.ch.ToString());
                    }
                    break;
            }
            
            read_char(ref lex);
            return tok;
        }
        private bool is_letter(char ch)
        {
            if('a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private String read_identifier(ref Lexer l)
        {
            int startPosition = l.position;
            while(is_letter(l.ch))
            {
                read_char(ref l);
            }
            return l.input.Substring(startPosition, l.position);
        } 

        private void read_char(ref Lexer l)
        {
            if(l.readPosition >= l.input.Length)
            {
                l.ch = (char)0;
            }
            else
            {
                l.ch = l.input[l.readPosition];
            }
            l.position = l.readPosition;
            (l.readPosition)++;
        }
    }
}

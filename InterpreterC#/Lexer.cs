using Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterC_
{
    internal struct Lexer
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

    internal class LexerManager
    {
        readonly KeyWords keyWords = new();
        Lexer lex = new();
        public void init_lexer(String code)
        {
            lex.position = 0;
            lex.readPosition = 0;
            lex.input = code;
            read_char(ref lex);
        }

        public Token next_token()
        {
            Token tok = new();

            eat_white_space(ref lex);

            switch (lex.ch)
            {
                case '=':
                    if (peek_char(lex) == '=')
                    {
                        char tmpCh = lex.ch;
                        read_char(ref lex);
                        String tmpLiteral = tmpCh.ToString() + lex.ch;
                        tok = new(TokTypes.EQ, tmpLiteral);
                    }
                    else
                    {
                        tok = new(TokTypes.ASSIGN, lex.ch.ToString());
                    }
                    break;
                case '!':
                    if (peek_char(lex) == '=')
                    {
                        char tmpCh = lex.ch;
                        read_char(ref lex);
                        String tmpLiteral = tmpCh.ToString() + lex.ch;
                        tok = new(TokTypes.NOT_EQ, tmpLiteral);
                    }
                    else
                    {
                        tok = new(TokTypes.BANG, lex.ch.ToString());
                    }
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
                case '-':
                    tok = new(TokTypes.MINUS, lex.ch.ToString());
                    break;
                case '/':
                    tok = new(TokTypes.SLASH, lex.ch.ToString());
                    break;
                case '*':
                    tok = new(TokTypes.ASTERISK, lex.ch.ToString());
                    break;
                case '<':
                    tok = new(TokTypes.LT, lex.ch.ToString());
                    break;
                case '>':
                    tok = new(TokTypes.GT, lex.ch.ToString());
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
                    if (is_letter(lex.ch))
                    {
                        tok.literal = read_identifier(ref lex);
                        tok.type = keyWords.lookup(tok.literal);
                        return tok;
                    }
                    else if (is_digit(lex.ch))
                    {
                        tok.type = TokTypes.INT;
                        tok.literal = read_number(ref lex);
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

        private static bool is_letter(char ch)
        {
            if ('a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool is_digit(char ch)
        {
            if ('0' <= ch && ch <= '9')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void eat_white_space(ref Lexer l)
        {
            while (l.ch == ' ' || l.ch == '\t' || l.ch == '\r' || l.ch == '\n')
            {
                read_char(ref l);
            }
        }

        private static String read_identifier(ref Lexer l)
        {
            int startPosition = l.position;
            while (is_letter(l.ch))
            {
                read_char(ref l);
            }
            return l.input[startPosition..l.position];
        }

        private static String read_number(ref Lexer l)
        {
            int startPosition = l.position;
            while (is_digit(l.ch))
            {
                read_char(ref l);
            }
            return l.input[startPosition..l.position];
        }

        private static char peek_char(Lexer l)
        {
            if (l.readPosition >= l.input.Length)
            {
                return (char)0;
            }
            else
            {
                return l.input[l.readPosition];
            }
        }

        private static void read_char(ref Lexer l)
        {
            if (l.readPosition >= l.input.Length)
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

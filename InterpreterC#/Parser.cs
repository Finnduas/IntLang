using Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterC_
{
    internal class Parser
    {

        private delegate Expression parse_prefix_expression();
        private delegate Expression parse_infix_expression(Expression before);

        private Dictionary<String, parse_prefix_expression> prefixParsingFuncs = new();
        private Dictionary<String, parse_infix_expression> infixParsingFuncs = new();

        private Dictionary<String, int> precedences = new();

        private LexerManager lexMan;

        private Token curToken;
        private Token peekToken;

        private List<String> errors;

        public Parser(LexerManager pLexMan)
        {
            errors = new();
            lexMan = pLexMan;
            next_token();
            next_token();

            init_precedences();

            register_prefix_parsing_func(TokTypes.IDENT, parse_identifier);
            register_prefix_parsing_func(TokTypes.INT, parse_IntegerLiteral);
            register_prefix_parsing_func(TokTypes.MINUS, _parse_prefix_expression);
            register_prefix_parsing_func(TokTypes.BANG, _parse_prefix_expression);
            register_prefix_parsing_func(TokTypes.TRUE, parse_boolean);
            register_prefix_parsing_func(TokTypes.FALSE, parse_boolean);
            register_prefix_parsing_func(TokTypes.LPAREN, parse_grouped_expression);
            register_prefix_parsing_func(TokTypes.IF, parse_if_expression);
            register_prefix_parsing_func(TokTypes.FUNCTION, parse_function_literal);

            register_infix_parsing_func(TokTypes.LT, _parse_infix_expression);
            register_infix_parsing_func(TokTypes.GT, _parse_infix_expression);
            register_infix_parsing_func(TokTypes.PLUS, _parse_infix_expression);
            register_infix_parsing_func(TokTypes.MINUS, _parse_infix_expression);
            register_infix_parsing_func(TokTypes.EQ, _parse_infix_expression);
            register_infix_parsing_func(TokTypes.NOT_EQ, _parse_infix_expression);
            register_infix_parsing_func(TokTypes.ASTERISK, _parse_infix_expression);
            register_infix_parsing_func(TokTypes.SLASH, _parse_infix_expression);
        }

        private void init_precedences()
        {
            precedences.Add(TokTypes.EQ, Precedence.EQUALS);
            precedences.Add(TokTypes.NOT_EQ, Precedence.EQUALS);
            precedences.Add(TokTypes.LT, Precedence.LESSGREATER);
            precedences.Add(TokTypes.GT, Precedence.LESSGREATER);
            precedences.Add(TokTypes.PLUS, Precedence.SUM);
            precedences.Add(TokTypes.MINUS, Precedence.SUM);
            precedences.Add(TokTypes.SLASH, Precedence.PRODUCT);
            precedences.Add(TokTypes.ASTERISK, Precedence.PRODUCT);
        }

        private int peek_precedence()
        {
            if (precedences.ContainsKey(peekToken.m_Type))
            {
                return precedences[peekToken.m_Type];
            }

            return Precedence.LOWEST;
        }
        private int cur_precedence()
        {
            if (precedences.ContainsKey(curToken.m_Type))
            {
                return precedences[curToken.m_Type];
            }

            return Precedence.LOWEST;
        }

        // expression parsing -----------------------------------------------------------------------------
        private BlockStatement parse_block_statement()
        {
            BlockStatement block = new();
            block.tok = curToken;
            block.statements = new();

            next_token();

            while (!curToken_is(TokTypes.RBRACE) && !curToken_is(TokTypes.EOF)) 
            {
                Statement stmt = parse_statement();
                if(stmt != null) 
                {
                    block.statements.Add(stmt);
                }
                next_token();
            }

            return block;
        }

        private Expression parse_function_literal()
        {
            FunctionLiteral fnLit = new();
            fnLit.tok = curToken;
            fnLit.parameters = new();
            fnLit.contents = new();

            if(!expected_peek(TokTypes.LPAREN))
            {
                return null;
            }
            next_token();

            Identifier ident = (Identifier)parse_identifier();
            fnLit.parameters.Add(ident);
            while(peekToken_is(TokTypes.COMMA))
            {
                next_token();
                next_token();
                ident = (Identifier)parse_identifier();
                fnLit.parameters.Add(ident);
            }

            if (!expected_peek(TokTypes.RPAREN))
            {
                return null;
            }
            if (!expected_peek(TokTypes.LBRACE))
            {
                return null;
            }

            fnLit.contents = parse_block_statement();

            return fnLit;
        }

        private Expression parse_if_expression()
        {
            IfExpression exp = new();
            exp.tok = curToken;

            if(!expected_peek(TokTypes.LPAREN))
            {
                return null;
            }

            next_token();

            exp.condition = parse_expression(Precedence.LOWEST);

            if(!expected_peek(TokTypes.RPAREN))
            {
                return null;
            }

            if(!expected_peek(TokTypes.LBRACE))
            {
                return null;
            }

            exp.consequence = parse_block_statement();

            if(peekToken_is(TokTypes.ELSE)) 
            {
                next_token();

                if (!expected_peek(TokTypes.LBRACE))
                {
                    return null;
                }

                exp.alternative = parse_block_statement();

            }

            return exp;
        }

        private Expression parse_grouped_expression()
        {
            next_token();
            Expression exp = parse_expression(Precedence.LOWEST);
            if(!expected_peek(TokTypes.RPAREN))
            {
                return null;
            }

            return exp;
        }

        private Expression parse_boolean()
        {
            Boolean _bool = new(curToken,  false ? curToken.m_Literal == TokTypes.TRUE : true);
            return _bool;
        }
        
        private Expression parse_identifier()
        {
            Identifier ident = new();
            ident.tok = curToken;
            ident.value = curToken.m_Literal;

            return ident;
        }

        private Expression parse_IntegerLiteral()
        {
            IntegerLiteral intLit = new();
            intLit.tok = curToken;
            try
            {
            intLit.value = int.Parse(curToken.m_Literal);
            }
            catch (OverflowException OEx)
            {
                errors.Add("Overflow when parsing Integerliteral to int in value field! - " + OEx.Message);
                return null;
            }
            catch (FormatException FEx)
            {
                errors.Add("Format wrong when parsing Integerliteral to int in value field! - " + FEx.Message);
                return null;
            }

            return intLit;
        }

        private Expression _parse_prefix_expression()
        {
            PrefixExpression exp = new();
            exp.tok = curToken;
            exp._operator = curToken.m_Literal;

            next_token();

            exp.right = parse_expression(Precedence.PREFIX);

            return exp;
        }

        private Expression _parse_infix_expression(Expression left)
        {
            InfixExpression exp = new();
            exp.tok = curToken;
            exp.left = left;
            exp._operator = curToken.m_Literal;

            int precedence = cur_precedence();
            next_token();
            exp.right = parse_expression(precedence);

            return exp;
        }
       
        //-------------------------------------------------------------------------------------------------

        private void register_prefix_parsing_func(String tokType, parse_prefix_expression prefixFunc)
        {
            prefixParsingFuncs.Add(tokType, prefixFunc);
        }

        private void register_infix_parsing_func(String tokType, parse_infix_expression infixFunc)
        {
            infixParsingFuncs.Add(tokType, infixFunc);
        }

        public List<String> get_errors()
        {
            return errors;
        }

        public void peek_error(String tokType)
        {
            errors.Add("expected next token to be " + tokType + ", got " + peekToken.m_Type);
        }

        public bool check_for_parser_errors()
        {
            if (errors.Count == 0)
            {
                return true;
            }
            else { 

            Console.WriteLine("Provided code has ERRORs:");
            for (int i = 0; i < errors.Count; ++i)
            {
                Console.WriteLine(errors[i]);
            }
            return false;
            }
        }

        public void next_token()
        {
            curToken = peekToken;
            peekToken = lexMan.next_token();
        }

        private bool curToken_is(String tokType)
        {
            if (tokType == curToken.m_Type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool peekToken_is(String tokType)
        {
            if(tokType == peekToken.m_Type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool expected_peek(String tokType)
        {
            if(peekToken_is(tokType)) {
                next_token();
                return true;
            }
            else
            {
                peek_error(tokType);
                return false;
            }
        }
        private VarStatement parse_var_statement()
        {
            VarStatement stmt = new();
            stmt.tok = curToken;

            if(!expected_peek(TokTypes.IDENT))
            {
                return null;
            }
            stmt.name = new Identifier();
            stmt.name.tok = curToken;
            stmt.name.value = curToken.m_Literal;

            //TODO: expressions should be parsed!!!, for now they're just skipped
            while(curToken.m_Type != TokTypes.SEMICOLON)
            {
                next_token();
            }

            return stmt;
        }

        private ReturnStatement parse_return_statement()
        {
            ReturnStatement stmt = new();
            stmt.tok = curToken;

            next_token();
            
            //TODO: expression should be parsed! But I to lazy so skip.

            while(curToken.m_Type != TokTypes.SEMICOLON)
            {
                next_token();
            }
            return stmt;
        }

        private Expression parse_expression(int precedence)
        {
            bool ok = prefixParsingFuncs.ContainsKey(curToken.m_Type);
            if(!ok)
            {
                errors.Add("No parsing function found for token of type " + curToken.m_Type);
                return null;
            }
            parse_prefix_expression prefix = prefixParsingFuncs[curToken.m_Type];
            
            Expression leftExpress = prefix();

            while (!peekToken_is(TokTypes.SEMICOLON) && precedence < peek_precedence())
            {
                bool parsingFuncExists = infixParsingFuncs.ContainsKey(peekToken.m_Type);
                if (!parsingFuncExists)
                {
                    return leftExpress;
                }

                parse_infix_expression infix = infixParsingFuncs[peekToken.m_Type];

                next_token();

                leftExpress = infix(leftExpress);
            }

            return leftExpress;
        }

        private ExpressionStatement parse_expression_statement()
        {
            ExpressionStatement stmt = new();
            stmt.tok = curToken;
            stmt.express = parse_expression(Precedence.LOWEST);

            if(peekToken_is(TokTypes.SEMICOLON))
            {
                next_token();
            }

            return stmt;
        }

        private Statement parse_statement()
        {
            switch(curToken.m_Type)
            {
                case TokTypes.VAR:
                    return parse_var_statement();
                case TokTypes.RETURN:
                    return parse_return_statement();
                default:
                    return parse_expression_statement();
            }
        }

        public Program parse_program()
        {
            Program program = new();
            program.statements = new();

            while (!curToken_is(TokTypes.EOF))
            {
                Statement stmt = parse_statement();
                if (stmt != null)
                {
                    program.statements.Add(stmt);
                }
                next_token();
            }
            return program;
        }
    }
}

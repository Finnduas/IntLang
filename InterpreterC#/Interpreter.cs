using Lookup;

namespace InterpreterC_
{
    struct Token
    {
        public String type;
        public String literal;

        public Token(String pT, String pL)
        {
            type = pT;
            literal = pL;
        }
    }   
}

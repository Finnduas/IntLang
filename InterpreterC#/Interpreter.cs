using Lookup;

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
}

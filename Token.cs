namespace ImprovedSimpleLexer
{
    public sealed class Token
    {
        public TYPE type;
        public string lexem;

        public Token(TYPE type, string lexem)
        {
            this.type = type;
            if (lexem == "Int" || lexem == "Float" || lexem == "Bool")
                this.lexem = lexem.ToLower();
            else
                this.lexem = lexem;

        }

        public sealed override string ToString()
        {
            return "(" + lexem + "," + type + ")";
        }
    }


}

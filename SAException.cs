using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedSimpleLexer
{
    public class SAException : Exception
    {
        public SAException(string errorDesc) : base(String.Format(errorDesc)) { }
    }

    public class notDeclared : SAException
    {
        public notDeclared(int col, int line) : base(String.Format($"Identificator didnt declared line: {line}, col: {col}")) { }
    }

    public class notComparable : SAException
    {
        public notComparable(int col, int line) : base(String.Format($"Uncomparable types line: {line}, col: {col}")) { }
    }

    public class alreadyDeclared : SAException
    {
        public alreadyDeclared(int col, int line) : base(String.Format($"Identificator has already declared line: {line}, col: {col}")) { }
    }
}

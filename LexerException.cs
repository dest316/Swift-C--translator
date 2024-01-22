using System;

namespace ImprovedSimpleLexer
{
    public class LexerException: Exception
    {
        public LexerException(string errorDesc): base(String.Format(errorDesc)) { }
    }
    public sealed class WrongID : LexerException
    {
        public WrongID(int col, int line) : base(String.Format($"Wrong ID line: {line}, col: {col}")) { }
    }

    public sealed class WrongNumeric : LexerException
    {
        public WrongNumeric(int col, int line) : base(String.Format($"Wrong numeric line: {line}, col: {col}")) { }
    }

    public sealed class EOF : LexerException
    {
        public EOF() : base(String.Format("EOF")) { }
    }

    public sealed class WrongOp : LexerException
    {
        public WrongOp(int col, int line) : base(String.Format($"Wrong operator line: {line}, col: {col}")) { }
    }
}

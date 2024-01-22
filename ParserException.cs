using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedSimpleLexer
{
    public class ParserException : Exception
    {
        public ParserException(string errorDesc) : base(String.Format(errorDesc)) { }
    }

    public sealed class ExpectedSEMI : ParserException
    {
        public ExpectedSEMI(int col, int line) : base(String.Format($"Expected ; line: {line}, col: {col}")) { }
    }

    public sealed class ExpectedLBR : ParserException
    {
        public ExpectedLBR(int col, int line) : base(String.Format($"Expected ( line: {line}, col: {col}")) { }
    }

    public sealed class ExpectedRBR : ParserException
    {
        public ExpectedRBR(int col, int line) : base(String.Format($"Expected ) line: {line}, col: {col}")) { }
    }

    public sealed class ExpectedLFBR : ParserException
    {
        public ExpectedLFBR(int col, int line) : base(String.Format($"Expected LFBR line: {line}, col: {col}")) { }
    }

    public sealed class ExpectedRFBR : ParserException
    {
        public ExpectedRFBR(int col, int line) : base(String.Format($"Expected RFBR line: {line}, col: {col}")) { }
    }

    public sealed class ExpectedID : ParserException
    {
        public ExpectedID(int col, int line) : base(String.Format($"Expected identifier line: {line}, col: {col}")) { }
    }

    public sealed class ExpectedCOLON : ParserException
    {
        public ExpectedCOLON(int col, int line) : base(String.Format($"Expected : line: {line}, col: {col}")) { }
    }
    public sealed class ExpectedAssign : ParserException
    {
        public ExpectedAssign(int col, int line) : base(String.Format($"Expected assign line: {line}, col: {col}")) { }
    }
    public sealed class ExpectedVar : ParserException
    {
        public ExpectedVar(int col, int line) : base(String.Format($"Expected var line: {line}, col: {col}")) { }
    }

    public sealed class ExpectedSupportedType : ParserException
    {
        public ExpectedSupportedType(int col, int line) : base(String.Format($"Expected int, float, bool type line: {line}, col: {col}")) { }
    }
}

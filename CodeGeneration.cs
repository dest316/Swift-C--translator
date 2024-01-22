using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedSimpleLexer
{
    public class CodeGeneration
    {
        private readonly Node source;
        public CodeGeneration(Node source)
        {
            this.source = source;
        }

        public override string ToString()
        {
            return source.getGeneratedText();
        }
    }
}

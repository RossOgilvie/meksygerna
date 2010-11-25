using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Terminals
{
    public abstract class Terminal
    {
        protected int _Length;
        public int Length { get { return _Length; } }

        public abstract override string ToString();
        public abstract string ToPolish();
        public abstract string Verbose();
        public abstract string ToLatex();
        public abstract double Evaluate();

        public abstract Terminal(Lexer lex, int startPosition, TerminalStorage ts);
    }
}

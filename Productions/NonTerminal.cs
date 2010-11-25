using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    public abstract class NonTerminal
    {
        protected int _Length;
        public int Length { get { return _Length; } }

        public abstract override string ToString();
        public abstract string ToPolish();
        public abstract string Verbose();
        public abstract string ToLatex();
        public abstract double Evaluate();

        /// <summary>
        /// A method to fill out this object by eating through the token stream. It returns true if it matched it's construction, advanced all the tokens it used.
        /// Else it returns false, with the Lexer at the position is started at.
        /// </summary>
        /// <param name="lex"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public abstract bool CreateNonTerminal(Lexer lex, ProductionStorage ps);
    }
}

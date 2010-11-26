using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    class A:NonTerminal
    {
        string _Letter = "";

        public override string ToString()
        {
            return _Letter;
        }

        public override string ToPolish()
        {
            return this.ToString();
        }

        public override string Verbose()
        {
            return this.ToString();
        }

        public override string ToLatex()
        {
            return this.ToString();
        }

        public override double Evaluate()
        {
            throw new ArgumentException("Cannot evaluate A: " + _Letter);
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ts)
        {
            if (lex.Current.Type == Selmaho.A)
            {
                switch (lex.Current.Value.Length)
                {
                    case 1:
                        _Letter = lex.Advance()[0].ToString();
                        break;
                    default:
                        _Letter = lex.Advance();
                        break;
                }
                _Length = 1;
                return true;
            }

            return false;
        }
    }
}

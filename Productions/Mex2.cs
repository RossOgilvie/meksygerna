using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// mex-2 =	polish | operand
    /// </summary>
    class Mex2:NonTerminal
    {
        NonTerminal _Result;

        public override string ToString()
        {
            return _Result != null?_Result.ToString():"Mex2 not yet initialised";
        }

        public override string ToPolish()
        {
            return _Result != null ? _Result.ToPolish() : "Mex2 not yet initialised";
        }

        public override string Verbose()
        {
            return _Result != null ? _Result.Verbose() : "Mex2 not yet initialised";
        }

        public override string ToLatex()
        {
            return _Result != null ? _Result.ToLatex() : "Mex2 not yet initialised";
        }

        public override double Evaluate()
        {
            if (_Result != null)
                return _Result.Evaluate();
            else
                throw new ArgumentException("Mex2 not yet initialised");
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            Polish _polish;
            Operand _operand;
            if (ps.MatchProduction<Polish>(out _polish))
            {
                _Result = _polish;
                return true;
            }
            else if (ps.MatchProduction<Operand>(out _operand))
            {
                _Result = _operand;
                return true;
            }

            return false;
        }
    }
}

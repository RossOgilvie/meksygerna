using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    class Operator:NonTerminal
    {
        string _Operator;

        public override string ToString()
        {
            return _Operator;
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
            throw new ArgumentException("Cannot evaluate operator: " + _Operator);
        }

        /// <summary>
        /// Return the result of this operator applied to the given arguments, whose values are determined by Evaluate()
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public double Evaluate(List<NonTerminal> args)
        {
            throw new NotImplementedException();
        }

        public override bool CreateTerminal(Lexer lex, ProductionStorage ts)
        {
            if (lex.Current.Type == Selmaho.Operator)
            {
                _Operator = lex.Advance();
            }

            return false;
        }
    }
}

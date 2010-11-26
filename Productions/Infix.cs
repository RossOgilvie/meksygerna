using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    class Infix : Expression
    {
        public Infix(NonTerminal left, Operator op, NonTerminal right)
        {
            _operator = op;
            _args.Add(left);
            _args.Add(right);
            _Length = _operator.Length + left.Length + right.Length;
        }

        public override string Verbose()
        {
            return base.Verbose("i");
        }

        /// <summary>
        /// This isn't a parsing class. It is semantic.
        /// </summary>
        /// <param name="lex"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    class InfixBO : Expression
    {
        public InfixBO(NonTerminal left, Operator op, NonTerminal right)
        {
            _operator = op;
            _args.Add(left);
            _args.Add(right);
        }

        public override string Verbose()
        {
            return base.Verbose("ib");
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

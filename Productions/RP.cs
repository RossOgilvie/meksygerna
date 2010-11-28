using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    class RP : Expression
    {
        public RP(List<NonTerminal> operands ,Operator op)
        {
            _operator = op;
            _args = operands;
            int operand_length = 0;
            foreach (NonTerminal o in operands)
                operand_length += o.Length;
        }

        public override string Verbose()
        {
            return base.Verbose("r");
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

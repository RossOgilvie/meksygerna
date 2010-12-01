using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// polish = operator operand + KUhE?
    /// </summary>
    class Polish:Expression
    {
        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            int save = lex.Position;

            //try to match an option PEhO
            ps.MatchProduction(Selmaho.PEhO);

            if (ps.MatchProduction<Operator>(out _operator))
            {
                //try to get as many op as we can
                do
                {
                    Operand _o;
                    if (ps.MatchProduction<Operand>(out _o))
                    {
                        _args.Add(_o);
                    }
                    else
                        break;
                }
                while (true);

                // Polish expressions have to have at least one argument
                if (_args.Count == 0)
                {
                    lex.Seek(save);
                    return false;
                }

                //try to grab an optional KUhE
                ps.MatchProduction(Selmaho.KUhE);

                return true;
            }

            //we failed.
            return false;
        }

        public override string Verbose()
        {
            return base.Verbose("p");
        }
    }
}

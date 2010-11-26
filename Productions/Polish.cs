using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// polish = operator mex-2 * KUhE?
    /// </summary>
    class Polish:Expression
    {
        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            if (ps.Retrieve<Operator>(out _operator))
            {
                //matched an operator. This means we have a polish expression.
                _Length += _operator.Length;
                //try to get as many mex2 as we can
                while (true)
                {
                    Mex2 m;
                    if (ps.Retrieve<Mex2>(out m))
                    {
                        _args.Add(m);
                        _Length += m.Length;
                    }
                    else
                        break;
                }

                //try to grab an optional KUhE
                KUhE k;
                ps.Retrieve<KUhE>(out k);
                _Length += k != null ? k.Length : 0;

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

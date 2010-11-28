using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// polish = operator mex-2 + KUhE?
    /// </summary>
    class Polish:Expression
    {
        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            int save = lex.Position;
            if (ps.MakeProduction<Operator>(out _operator))
            {
                //matched an operator. This means we probably have a polish expression.
                _Length += _operator.Length;
                //try to get as many mex2 as we can
                do
                {
                    Mex2 m;
                    if (ps.MakeProduction<Mex2>(out m))
                    {
                        _args.Add(m);
                        _Length += m.Length;
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
                KUhE k;
                ps.MakeProduction<KUhE>(out k);
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

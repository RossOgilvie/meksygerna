using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// lerfu-string = ( BY | A BU ) ( BY | A BU )* BOI?
    /// </summary>
    class LerfuString:NonTerminal
    {
        string _String = "";

        public override string ToString()
        {
            return _String;
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
            throw new ArgumentException("Cannot evaluate LerfuString: " + _String);
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            BY by = new BY();
            A a = new A();
            BU bu = new BU();

            // get the leading ( BY | A BU )
            if (ps.MatchProduction<BY>(out by))
            {
                _String += by.ToString();
            }
            else if (ps.MatchProduction<A>(out a))
            {
                if (ps.MatchProduction<BU>(out bu))
                {
                    _String += a.ToString();
                }
            }
            else
            {
                return false;
            }

            while(true)
            {
                if (ps.MatchProduction<BY>(out by))
                {
                    _String += by.ToString();
                }
                else if (ps.MatchProduction<A>(out a))
                {
                    if (ps.MatchProduction<BU>(out bu))
                    {
                        _String += a.ToString();
                    }
                }
                else
                    break;
            }

            //eat the BOI that might be there
            BOI boi = new BOI();
            ps.MatchProduction<BOI>(out boi);

            return true;
        }
    }
}

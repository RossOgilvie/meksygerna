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
            if (ps.MakeProduction<BY>(out by))
            {
                _String += by.ToString();
                _Length += by.Length;
            }
            else if (ps.MakeProduction<A>(out a))
            {
                _Length += a.Length;

                if (ps.MakeProduction<BU>(out bu))
                {
                    _String += a.ToString();
                    _Length += bu.Length;
                }
            }
            else
            {
                return false;
            }

            while(true)
            {
                if (ps.MakeProduction<BY>(out by))
                {
                    _String += by.ToString();
                    _Length += by.Length;
                }
                else if (ps.MakeProduction<A>(out a))
                {
                    _Length += a.Length;

                    if (ps.MakeProduction<BU>(out bu))
                    {
                        _String += a.ToString();
                        _Length += bu.Length;
                    }
                }
                else
                    break;
            }

            //eat the BOI that might be there
            BOI boi = new BOI();
            ps.MakeProduction<BOI>(out boi);
            _Length += boi != null ? boi.Length : 0;

            return true;
        }
    }
}

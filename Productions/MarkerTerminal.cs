using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// This represents Terminals who have are just markers and don't have 'content'
    /// </summary>
    class MarkerTerminal:NonTerminal
    {
        Selmaho _Type = Selmaho.Unknown;

        public MarkerTerminal(Selmaho type)
        {
            _Type = type;
        }

        public override string ToString()
        {
            return _Type.ToString();
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
            throw new ArgumentException("Cannot evaluate " + _Type.ToString());
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ts)
        {
            if (lex.Current.Type == _Type)
            {
                //eat the marker
                lex.Advance();
                return true;
            }

            return false;
        }
    }

    class BOI : MarkerTerminal
    {
        public BOI() : base(Selmaho.BOI) { }
    }
    
    class BU : MarkerTerminal
    {
        public BU() : base(Selmaho.BU) { }
    }

    class KUhE : MarkerTerminal
    {
        public KUhE() : base(Selmaho.KUhE) { }
    }

    class VEI : MarkerTerminal
    {
        public VEI() : base(Selmaho.VEI) { }
    }

    class VEhO : MarkerTerminal
    {
        public VEhO() : base(Selmaho.VEhO) { }
    }
}

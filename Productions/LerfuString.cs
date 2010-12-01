using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// lerfu-string = ( BY | anyword BU ) ( BY | anyword BU )* BOI?
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
            string by;

            // get the leading ( BY | A BU )
            if (ps.MatchProduction(Selmaho.BY, out by))
            {
                if (by.Length == 2)
                    by = by[0].ToString();
                _String += by;
            }
            else if (lex.Peek.Type == Selmaho.BU)
            {
                string l = lex.Advance();
                if (l.Length > 1)
                    l = "(" + l + ")";
                _String += l;
                lex.Advance();//eat the bu
            }
            else
            {
                return false;
            }

            while(true)
            {
                if (ps.MatchProduction(Selmaho.BY, out by))
                {
                    if (by.Length == 2)
                        by = by[0].ToString();
                    _String += by;
                }
                else if (lex.Peek.Type == Selmaho.BU)
                {
                    string l = lex.Advance();
                    if (l.Length > 1)
                        l = "(" + l + ")";
                    _String += l;
                    lex.Advance();//eat the bu
                }
                else
                    break;
            }

            //eat the BOI that might be there
            ps.MatchProduction(Selmaho.BOI);

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// operand = number | lerfu-string | VEI# mex /VEhO#/
    /// </summary>
    class Operand:NonTerminal
    {
        NonTerminal result;

        public override string ToString()
        {
            return result != null ? result.ToString() : "Mex1 not yet iniatialised.";
        }

        public override string ToPolish()
        {
            return result != null ? result.ToPolish() : "Mex1 not yet iniatialised.";
        }

        public override string Verbose()
        {
            return result != null ? result.Verbose() : "Mex1 not yet iniatialised.";
        }

        public override string ToLatex()
        {
            if (result != null)
            {
                string output = "";
                output += result.GetType() == typeof(Mex) ? "\\left( " : "";
                output += result.ToLatex();
                output += result.GetType() == typeof(Mex) ? " \\right) " : "";
                return output;
            }
            else return "Mex1 not yet iniatialised.";
        }

        public override double Evaluate()
        {
            if (result != null)
                return result.Evaluate();
            else
                throw new NullReferenceException("Mex1 not yet iniatialised.");
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            int save = lex.Position;

            Number _n;
            LerfuString _l;
            Mex _m;

            if (ps.MatchProduction<Number>(out _n))
            {
                result = _n;
                return true;
            }
            else if (ps.MatchProduction<LerfuString>(out _l))
            {
                result = _l;
                return true;
            }
            else if (ps.MatchProduction(Selmaho.VEI) && ps.MatchProduction<Mex>(out _m))
            {
                result = _m;
                ps.MatchProduction(Selmaho.VEhO);
                return true;
            }

            lex.Seek(save);
            return false;
        }
    }
}

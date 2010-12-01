using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// operand = lerfu-string | number | polish | VEI# mex /VEhO#/
    /// </summary>
    class Operand:NonTerminal
    {
        NonTerminal result;

        public override string ToString()
        {
            return result != null ? result.ToString() : "Operand not yet iniatialised.";
        }

        public override string ToPolish()
        {
            return result != null ? result.ToPolish() : "Operand not yet iniatialised.";
        }

        public override string Verbose()
        {
            return result != null ? result.Verbose() : "Operand not yet iniatialised.";
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
            else return "Operand not yet iniatialised.";
        }

        public override double Evaluate()
        {
            if (result != null)
                return result.Evaluate();
            else
                throw new NullReferenceException("Operand not yet iniatialised.");
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            int save = lex.Position;

            Number _n;
            LerfuString _l;
            Mex _m;
            Polish _p;

            if (ps.MatchProduction<LerfuString>(out _l))
            {
                result = _l;
                return true;
            }
            else if (ps.MatchProduction<Number>(out _n))
            {
                result = _n;
                return true;
            }
            else if (ps.MatchProduction<Polish>(out _p))
            {
                result= _p;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// mex-1 =	mex-2 ( operator BO mex-2 )*
    /// </summary>
    class Mex1 : NonTerminal
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
            return result != null ? result.ToLatex() : "Mex1 not yet iniatialised.";
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
            Mex2 left;
            if (ps.MatchProduction<Mex2>(out left))
            {
                result = left;

                while (true)
                {
                    Operator op;
                    Mex2 right;

                    //( operator bo mex2 )
                    int save = lex.Position;
                    if (ps.MatchProduction<Operator>(out op) && ps.MatchProduction(Selmaho.BO) && ps.MatchProduction<Mex2>(out right))
                    {
                        //if we're done this, then we have the whole bracket.
                        //turn it into an infixbo
                        result = new InfixBO(result, op, right);
                    }
                    else
                    {
                        lex.Seek(save);
                        break;
                    }

                }

                return true;
            }
            else
                return false; //you have to match a Mex2 first
        }
    }
}

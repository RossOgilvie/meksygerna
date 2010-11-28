using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// mex = mex-1 ( operator mex-1 | operand * operator KUhE? ) *
    /// </summary>
    class Mex :NonTerminal
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
            Mex1 m1;
            if (ps.MakeProduction<Mex1>(out m1))
            {
                result = m1;

                //( operator mex-1 | operand * operator KUhE? ) *
                while (true)
                {
                    Operator op;
                    Mex1 m12;
                    List<NonTerminal> o = new List<NonTerminal>();

                    //( operator mex1 )
                    int save = lex.Position;
                    if (ps.MakeProduction<Operator>(out op) && ps.MakeProduction<Mex1>(out m12))
                    {
                        //if we're done this, then we have the whole bracket.
                        //turn it into an infix
                        result = new Infix(result, op, m12);
                    }
                        //(operand * operator KUhE?)
                    else
                    {
                        //undo any damage from the first bit
                        lex.Seek(save);
                        //put the leading mex1 into the operand list
                        o.Add(m1);

                        //operand *
                        while (true)
                        {
                            Operand oper;
                            if(ps.MakeProduction<Operand>(out oper))
                                o.Add(oper);
                            else
                                break;
                        }

                        //operator
                        if (ps.MakeProduction<Operator>(out op))
                        {
                            result = new RP(o, op);
                        }
                        else
                        {
                            lex.Seek(save);
                            break; // havent matched an infix or an rp
                        }

                        KUhE k;
                        ps.MakeProduction<KUhE>(out k);
                    }
                }

                return true;
            }
            else
                return false; //you have to match a Mex1 first
        }
    }
}

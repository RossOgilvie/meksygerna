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
            int save = lex.Position;

            Number _n;
            LerfuString _l;
            VEI _v;
            Mex _m;

            if (ps.Retrieve<Number>(out _n))
            {
                result = _n;
                _Length = result.Length;
                return true;
            }
            else if (ps.Retrieve<LerfuString>(out _l))
            {
                result = _l;
                _Length = _l.Length;
                return true;
            }
            else if (ps.Retrieve<VEI>(out _v) && ps.Retrieve<Mex>(out _m))
            {
                result = _m;
                _Length = result.Length;

                VEhO _vo;
                ps.Retrieve<VEhO>(out _vo);
                _Length += _vo != null ? _vo.Length : 0;
                return true;
            }

            lex.Seek(save);
            return false;
        }

        private enum OperandType
        {
            Number, LerfuString, Vei, Undefined
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// mex-2 =	polish | operand
    /// </summary>
    class Mex2:NonTerminal
    {
        Operand _operand;
        Polish _polish;
        Mex2Type _type = Mex2Type.Unknown;
        enum Mex2Type
        {
            Polish, Operand, Unknown
        }

        public override string ToString()
        {
            switch (_type)
            {
                case Mex2Type.Operand:
                    return _operand.ToString();
                case Mex2Type.Polish:
                    return _polish.ToString();
                default:
                    return "Mex2 not yet initialised";
            }
        }

        public override string ToPolish()
        {
            switch (_type)
            {
                case Mex2Type.Operand:
                    return _operand.ToPolish();
                case Mex2Type.Polish:
                    return _polish.ToPolish();
                default:
                    return "Mex2 not yet initialised";
            }
        }

        public override string Verbose()
        {
            switch (_type)
            {
                case Mex2Type.Operand:
                    return _operand.Verbose();
                case Mex2Type.Polish:
                    return _polish.Verbose();
                default:
                    return "Mex2 not yet initialised";
            }
        }

        public override string ToLatex()
        {
            switch (_type)
            {
                case Mex2Type.Operand:
                    return _operand.ToLatex();
                case Mex2Type.Polish:
                    return _polish.ToLatex();
                default:
                    return "Mex2 not yet initialised";
            }
        }

        public override double Evaluate()
        {
            switch (_type)
            {
                case Mex2Type.Operand:
                    return _operand.Evaluate();
                case Mex2Type.Polish:
                    return _polish.Evaluate();
                default:
                    throw new ArgumentException("Mex2 not yet initialised");
            }
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            if (ps.MakeProduction<Polish>(out _polish))
            {
                _type = Mex2Type.Polish;
                _Length = _polish.Length;
                return true;
            }
            else if (ps.MakeProduction<Operand>(out _operand))
            {
                _type = Mex2Type.Operand;
                _Length = _operand.Length;
                return true;
            }

            return false;
        }
    }
}

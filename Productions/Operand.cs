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
        OperandType _type = OperandType.Undefined;
        Number _n;
        LerfuString _l;

        public override string ToString()
        {
            switch (_type)
            {
                case OperandType.Number:
                    return _n.ToString();
                case OperandType.LerfuString:
                    return _l.ToString();
                default:
                    return "Unknown operand type";
            }

        }

        public override string ToPolish()
        {
            switch (_type)
            {
                case OperandType.Number:
                    return _n.ToPolish();
                case OperandType.LerfuString:
                    return _l.ToPolish();
                default:
                    return "Unknown operand type";
            }
        }

        public override string Verbose()
        {
            switch (_type)
            {
                case OperandType.Number:
                    return _n.Verbose();
                case OperandType.LerfuString:
                    return _l.Verbose();
                default:
                    return "Unknown operand type";
            }
        }

        public override string ToLatex()
        {
            switch (_type)
            {
                case OperandType.Number:
                    return _n.ToLatex();
                case OperandType.LerfuString:
                    return _l.ToLatex();
                default:
                    return "Unknown operand type";
            }
        }

        public override double Evaluate()
        {
            switch (_type)
            {
                case OperandType.Number:
                    return _n.Evaluate();
                case OperandType.LerfuString:
                    return _l.Evaluate();
                default:
                    throw new ArgumentException("Unknown operand type");
            }
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            if (ps.Retrieve<Number>(out _n))
            {
                _type = OperandType.Number;
                _Length = _n.Length;
                return true;
            }
            else if (ps.Retrieve<LerfuString>(out _l))
            {
                _type = OperandType.LerfuString;
                _Length = _l.Length;
                return true;
            }

            //TODO: Add Vei support when you have mex type.

            return false;
        }

        private enum OperandType
        {
            Number, LerfuString, Vei, Undefined
        }
    }
}

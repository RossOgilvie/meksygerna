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
        InfixBO _ib;
        Mex2 _m2;
        Mex1Type _type = Mex1Type.Unknown;

        public override string ToString()
        {
            switch (_type)
            {
                case Mex1Type.InfixBO:
                    return _ib.ToString();
                case Mex1Type.Mex2:
                    return _m2.ToString();
                default:
                    return "Mex1 not yet iniatialised.";
            }
        }

        public override string ToPolish()
        {
            switch (_type)
            {
                case Mex1Type.InfixBO:
                    return _ib.ToPolish();
                case Mex1Type.Mex2:
                    return _m2.ToPolish();
                default:
                    return "Mex1 not yet iniatialised.";
            }
        }

        public override string Verbose()
        {
            switch (_type)
            {
                case Mex1Type.InfixBO:
                    return _ib.Verbose();
                case Mex1Type.Mex2:
                    return _m2.Verbose();
                default:
                    return "Mex1 not yet iniatialised.";
            }
        }

        public override string ToLatex()
        {
            switch (_type)
            {
                case Mex1Type.InfixBO:
                    return _ib.ToLatex();
                case Mex1Type.Mex2:
                    return _m2.ToLatex();
                default:
                    return "Mex1 not yet iniatialised.";
            }
        }

        public override double Evaluate()
        {
            switch (_type)
            {
                case Mex1Type.InfixBO:
                    return _ib.Evaluate();
                case Mex1Type.Mex2:
                    return _m2.Evaluate();
                default:
                    throw new NullReferenceException("Mex1 not yet iniatialised.");
            }
        }

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            if (ps.Retrieve<Mex2>(out _m2))
            {
                _Length = _m2.Length;
                _type = Mex1Type.Mex2;

                while (true)
                {
                    Operator op;
                    BO bo;
                    Mex2 m2;

                    //( operator bo mex2 )
                    if (ps.Retrieve<Operator>(out op) && ps.Retrieve<BO>(out bo) && ps.Retrieve<Mex2>(out m2))
                    {
                        //if we're done this, then we have the whole bracket.
                        //turn it into an infixbo
                        if (_type == Mex1Type.Mex2)
                            _ib = new InfixBO(_m2, op, m2);
                        else
                            _ib = new InfixBO(_ib, op, m2);

                        _type = Mex1Type.InfixBO;
                        _Length = _ib.Length;
                    }
                    else
                        break;

                }

                return true;
            }
            else
                return false; //you have to match a Mex2 first
        }

        enum Mex1Type { InfixBO, Mex2, Unknown }
    }
}

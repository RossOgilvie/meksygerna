using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    class PA:NonTerminal
    {
        private double _Number = 0;

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ts)
        {
            if (lex.Current.Type == Selmaho.PA)
            {
                string n = lex.Advance();
                switch (n)
                {
                    case "no":
                        _Number = 0;
                        break;
                    case "pa":
                        _Number = 1;
                        break;
                    case "re":
                        _Number = 2;
                        break;
                    case "ci":
                        _Number = 3;
                        break;
                    case "vo":
                        _Number = 4;
                        break;
                    case "mu":
                        _Number = 5;
                        break;
                    case "xa":
                        _Number = 6;
                        break;
                    case "ze":
                        _Number = 7;
                        break;
                    case "bi":
                        _Number = 8;
                        break;
                    case "so":
                        _Number = 9;
                        break;
                    default:
                        _Number = Convert.ToDouble(n);
                        break;
                }

                //if we manage to make a number, then return true
                return true;
            }

            //if we didn't, then return false
            return false;
        }

        public override string ToString()
        {
            return _Number.ToString();
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
            return _Number;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// number = PA PA*
    /// </summary>
    class Number:NonTerminal
    {
        double _Number = 0;

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

        public override bool CreateNonTerminal(Lexer lex, ProductionStorage ps)
        {
            // Don't have fraction or decimal handling yet

            //get the leading PA
            PA n;
            if (ps.Retrieve<PA>(out n))
            {
                _Number += n.Evaluate();
                _Length += n.Length;
            }
            else
                return false;

            while (true)
            {
                if (ps.Retrieve<PA>(out n))
                {
                    //shift the digits up one place value
                    _Number *= 10;
                    //add the digit to the units
                    _Number += n.Evaluate();
                    _Length += n.Length;
                }
                else
                    break; //if we didn't match a PA, then stop trying.
            }

            //eat the BOI that might be there
            BOI boi = new BOI();
            ps.Retrieve<BOI>(out boi);
            _Length += boi != null ? boi.Length : 0;

            return true;
        }
    }
}
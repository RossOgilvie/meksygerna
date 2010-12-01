using System;
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
        List<string> PAs = new List<string>();

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
            // Don't have fraction yet

            //get the leading PA
            string p;
            if (ps.MatchProduction(Selmaho.PA, out p))
                PAs.Add(p);
            else
                return false;

            while (true)
            {
                if (ps.MatchProduction(Selmaho.PA, out p))
                    PAs.Add(p);
                else
                    break; //if we didn't match a PA, then stop trying.
            }

            //eat the BOI that might be there
            ps.MatchProduction(Selmaho.BOI);

            ConstructNumber();

            return true;
        }

        void ConstructNumber()
        {
            _Number = 0;

            // number pi number
            int split = PAs.FindIndex(x => x == "pi");
            //split will return -1 if it fails to find a pi

            //whole part. work from the units up
            int index = 0;
            int whole = 0;
            //if split is -1, then do the whole list
            for (int i = split == -1 ? PAs.Count -1: split - 1; i >= 0; i--)
            {
                if (PAs[i] == "ki'o")
                {
                    //move up to the next multiple of 3
                    // if index = 7. 7 % 3 = 1. So we need to add 3 - 1 = 2 to get to the next multiple of 3.
                    index += 3 - (index % 3);
                }
                else
                {
                    whole += getValue(PAs[i]) * ((int)Math.Pow(10, index));
                    index++;
                }
            }
            _Number += whole;

            if (split != -1)
            {
                //Remove all those ones we just used.
                PAs.RemoveRange(0, split + 1);
                //find the next pi after this (we only deal with one pi, and ignore anything later)
                split = PAs.FindIndex(x => x == "pi");
                split = split == -1 ? PAs.Count : split;

                //decimal part
                //ki'o handling in fraction is stupid and i don't understand it
                index = 0;
                double part = 0;
                for (int i = 0; i < split; i++)
                {
                    part += getValue(PAs[i]) * Math.Pow(10, -index - 1);
                    index++;
                }

                _Number += part;
            }
        }

        int getValue(string s)
        {
            int _Number;
            switch (s)
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
                    _Number = Convert.ToInt32(s);
                    break;
            }
            return _Number;
        }
    }
}

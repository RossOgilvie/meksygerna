using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MexGrammar.Productions;

namespace MexGrammar
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            List<string> input = new List<string>();
#if (!DEBUG)
            input.Add("");
            //for now we just take all the args and make a string out of them
            //in the future, we might support flags etc
            foreach (string s in args)
                input[0] += s + " ";
#else
            input.Add(".abu pi'ibo xy. te'abo re su'i by. pi'ibo xy. su'i cy.");
            input.Add("1 by. + ku'e * 3");
            input.Add("1 2 + * 3");
            input.Add("1 2 +");
            input.Add("1 + 2 + 3");
            input.Add("va'a 2 su'i 1");
            input.Add("+ 1 2 ku'e - 3");
            input.Add("vei pe'o va'a 2 ku'e su'i pe'o fe'a vei 2 te'abo re vu'u vo pi'ibo 1 pi'ibo 1 ve'o ku'e ve'o fe'i re pi'ibo 1");
            input.Add("vei va'a by ku'e su'i fe'a vei by. te'abo re vu'u vo pi'ibo .abu pi'ibo cy ve'o ve'o fe'i re pi'ibo .abu");
            input.AddRange(new string[]{"biboi ciboi gei",
"pa vu'u re",
"pa su'i pa",
"pa fe'i re pi'ibo .abu",
"fe'i re pi'ibo .abu",
"ci su'i vo pi'i bo mu",
"ci su'i vo pi'i mu",
"ci vu'u re",
"ci vu'u vo",
"fu'a ciboi muboi vu'u",
"fu'a reboi 1 va'a",
"fu'a reboi ci pi'i voboi mu pi'i su'i",
"fu'a reboi ci su'i"});
#endif
            Console.WriteLine(Selmaho.VEhO.ToString());

            foreach (string s in input)
            {
                //Do the parse
                //Output the result of doing the mex and the polish form of it
                try
                {
                    Lexer lex = new Lexer(s);
                    Parser par = new Parser(lex);
                    try
                    {
                        Console.WriteLine(par.Result);
                    }
                    catch { } //expressions with letters in them throw an exception.
                    Console.WriteLine(par.Result.OutputPolish());
                    Console.WriteLine(par.Result.OutputPolishVerbose());
                    Console.WriteLine(par.Result.OutputLatex());
                    Console.WriteLine();
                }
                catch (ParseError pe)
                {
                    Console.WriteLine(pe.Message + " ... " + pe.token.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

#if (DEBUG)
            Console.ReadLine();
#endif
        }
        */

        static void Main()
        {
            string s = "by. cy. dy. .abu.ebu";
            Lexer l = new Lexer(s);
            ProductionStorage ps = new ProductionStorage(l);
            LerfuString n;
            if (ps.Retrieve<LerfuString>(out n))
            {
                Console.WriteLine(l.Current.Value);
                Console.WriteLine(n.ToString());
            }

            Console.ReadLine();
        }
    }
}

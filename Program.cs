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


        static void Main(string[] args)
        {
            //string s = "1 + vei va'a 2 ku'e ve'o pi'i xa";
            //Lexer l = new Lexer(s);
            //ProductionStorage ps = new ProductionStorage(l);
            //Mex n;
            //if (ps.Retrieve<Mex>(out n))
            //{
                //Console.WriteLine("Current Token: " + l.Current.Value);
                //Console.WriteLine("Length of production: " + n.Length.ToString());
                //Console.WriteLine(n.Verbose());
                //try
                //{
                //    Console.WriteLine("Answer: " + n.Evaluate());
                //}
                //catch { }
            //}

            TestLatex();
#if (DEBUG)
            Console.ReadLine();
#endif
        }

        static void TestLatex()
        {
            string j = "";
            j += @"\documentclass{article}\begin{document}";

            foreach (string k in test)
            {
                string i = k.Split('|')[0];
                Lexer l = new Lexer(i);
                ProductionStorage ps = new ProductionStorage(l);
                Mex n;
                if (ps.MakeProduction<Mex>(out n))
                {
                    j += i + " == $";
                    j += n.ToLatex();
                    j+= "$";
                    try
                    {
                        j += " = " + n.Evaluate().ToString();
                    }
                    catch { }
                    j += "\r\n";
                }
            }

            j += @"\end{document}";
            Console.WriteLine(j);
        }

        static string[] test = new string[]
        {
            "pa su'i pa | 1 + 1 = 2",
"ci su'i vo pi'i mu | 3 + 4 * 5 (=35)",
"ci su'i vo pi'i bo mu | 3 + 4 * bo 5 (= 23)",
"vei ny. su'i pa ve'o pi'i vei ny. su'i pa | (n+1)(n+1)",
"ny. te'a re su'i re pi'ibo ny. su'i pa | n^2 + 2 *n + 1",
"su'i paboi reboi ci | +(1,2,3) = 6",
"py. su'i va'a ny. ku'e su'i zy | p + -n + z",
"pe'o su'i paboi reboi ciboi ku'e",
"py. su'i pe'o va'a ny. ku'e su'i zy",
"ci vu'u re",
"fu'a reboi ci su'i",
"reboi ci su'i | not CLL",
"fu'a reboi ci pi'i voboi mu pi'i su'i",
".abu pi'ibo vei xy. te'a re ve'o su'i by. pi'ibo xy. su'i cy. |quadratic root",
"vei va'a by. ku'e su'i fe'a vei by. te'abo re vu'u vo pi'ibo .abu pi'ibo cy. ve'o ve'o fe'i re pi'ibo .abu |quad formula"
        };
    }
}

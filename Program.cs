using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar
{
    class Program
    {
        static void Main(string[] args)
        {
            Normal();
            //TestLatex();
            //TimeTest();

#if (DEBUG)
            Console.ReadLine();
#endif
        }

        static void Normal()
        {
            string s = "cinoki'oki'o + 3";
            Lexer l = new Lexer(s);
            try
            {
                Parser p = new Parser(l);
                if (p.Result != null)
                {
                    Console.WriteLine(l.ToString() + " =>");
                    Console.WriteLine(p.Result.Verbose());
                    try
                    {
                        Console.WriteLine("Answer: " + p.Result.Evaluate());
                    }
                    catch { }
                }
            }
            catch (ParseError pe)
            {
                Console.WriteLine(pe.Message + " : " + pe.token.Value);
            }
        }

        static void TestLatex()
        {
            string j = "";
            j += @"\documentclass{article}\begin{document}";

            foreach (string k in test)
            {
                string i = k.Split('|')[0];
                Lexer l = new Lexer(i);
                Parser p = new Parser(l);
                if(p.Result != null)
                {
                    j += i + " == $";
                    j += p.Result.ToLatex();
                    j+= "$";
                    try
                    {
                        j += " = " + p.Result.Evaluate().ToString();
                    }
                    catch { }
                    j += "\r\n";
                }
            }

            j += @"\end{document}";
            Console.WriteLine(j);
        }

        static void TimeTest()
        {
            string[] t = new string[3];
            t[0] = "vei va'a by. ku'e su'i fe'a vei by. te'abo re vu'u vo pi'ibo .abu pi'ibo cy. ve'o ve'o fe'i re pi'ibo .abu";
            t[1] = "+ + + + + + + + 1";
            t[2] = "1 2 3 + 4 5 6 - 7 8 9 *";

            for(int i = 0; i < 1000; i++)
                foreach (string s in t)
                {
                    Lexer l = new Lexer(s);
                    Parser p = new Parser(l);
                    if (p.Result != null)
                    {
                        Console.WriteLine("Current Token: " + l.Current.Value);
                        Console.WriteLine("Length of production: " + p.Result.Length.ToString());
                        Console.WriteLine(p.Result.Verbose());
                        try
                        {
                            Console.WriteLine("Answer: " + p.Result.Evaluate());
                        }
                        catch { }
                    }
                }
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

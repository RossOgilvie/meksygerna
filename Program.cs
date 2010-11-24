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
            //string input ="";
            string input = "1 + 2 + 3";
            //string input = "va'a 2 su'i 1";
            //string input = "vei pe'o va'a 2 ku'e su'i pe'o fe'a vei 2 te'abo re vu'u vo pi'ibo 1 pi'ibo 1 ve'o ku'e ve'o fe'i re pi'ibo 1";
            //string input = "vei va'a by ku'e su'i fe'a vei by te'abo re vu'u vo pi'ibo .abu pi'ibo cy ve'o ve'o fe'i re pi'ibo .abu";
            
            //for now we just take all the args and make a string out of them
            //in the future, we might support flags etc
            foreach (string s in args)
                input += s + " ";

            //Do the parse
            //Output the result of doing the mex and the polish form of it
            try
            {
                Lexer lex = new Lexer(input);
                Parser par = new Parser(lex);
                Console.WriteLine(par.Result);
                Console.WriteLine(par.Result.OutputPolish());
                Console.WriteLine(par.Result.OutputPolishVerbose());
                Console.WriteLine(par.Result.OutputLatex());
            }
            catch (ParseError pe)
            {
                Console.WriteLine(pe.Message + " ... " + pe.token.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Console.ReadLine();
        }
    }
}

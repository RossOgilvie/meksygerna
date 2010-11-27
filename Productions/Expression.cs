using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar.Productions
{
    /// <summary>
    /// An intermediate type for representing expressions
    /// </summary>
    /// <typeparam name="E">The type of the arguments that this expression takes</typeparam>
    abstract class Expression : NonTerminal
    {
        protected Operator _operator;
        protected List<NonTerminal> _args;

        public Expression()
        {
            _operator = new Operator();
            _args = new List<NonTerminal>();
        }

        public override string ToString()
        {
            return _operator != null ? _operator.ToString() : "Expression not constructed yet";
        }

        public override string ToPolish()
        {
            string a = "";
            a += "(" + _operator.ToPolish() + " ";
            foreach (NonTerminal m in _args)
                a += m.ToPolish() + " ";
            a += ")";
            return a;
        }

        protected string Verbose(string Notation)
        {
            string a = "";
            a += "(" + _operator.Verbose() + " /" + Notation + " ";
            foreach (NonTerminal m in _args)
                a += m.Verbose() + " ";
            a += ")";
            return a;
        }

        public override string ToLatex()
        {
            string result = "";

            switch (_operator.ToString())
            {
                case "+":
                case "su'i":
                    if (_args.Count == 0)
                        return "0";
                    else
                    {
                        result = _args[0].ToLatex();
                        for (int i = 1; i < _args.Count; i++)
                            result += "+ " + _args[i].ToLatex();
                    }
                    return result;

                case "-":
                case "vu'u":
                    if (_args.Count == 0)
                        return "0";
                    else
                    {
                        result = _args[0].ToLatex();
                        for (int i = 1; i < _args.Count; i++)
                            result += "- " + _args[i].ToLatex();
                    }
                    return result;

                case "*":
                case "pi'i":
                    if (_args.Count == 0)
                        return "1";
                    else
                    {
                        result = _args[0].ToLatex();
                        for (int i = 1; i < _args.Count; i++)
                            result += "\\times " + _args[i].ToLatex();
                    }
                    return result;

                case "/":
                case "fe'i":
                    if (_args.Count == 0)
                        return "1";
                    else if (_args.Count == 1)
                        return _args[0].ToLatex();
                    else
                    {
                        result = "\\frac{" + _args[0].ToLatex() + "}{" + _args[1].ToLatex();
                        for (int i = 2; i < _args.Count; i++)
                            result += "\\times " + _args[i].ToLatex();
                        result += "}";
                    }
                    return result;

                case "te'a":
                case "^":
                    if (_args.Count == 0)
                        return "1";
                    else if (_args.Count == 1)
                        return _args[0].ToLatex();
                    else
                        return "{" + _args[0].ToLatex() + "}^{" + _args[1].ToLatex() + "}";

                case "gei":
                    if (_args.Count == 0)
                        return "1";
                    else if (_args.Count == 1)
                        return "10 ^ { " + _args[0].ToLatex() + " } ";
                    else if (_args.Count == 2)
                        return "{" + _args[1].ToLatex() + "} \\times 10^{" + _args[1].ToLatex() + "}";
                    else
                        return "{" + _args[1].ToLatex() + "} \\times {" + _args[2].ToLatex() + "}^{" + _args[1].ToLatex() + "}";

                case "fa'i":
                    if (_args.Count == 0)
                        return "1";
                    else
                        return "\\frac{1}{" + _args[0].ToLatex() + "}";

                case "va'a":
                    if (_args.Count == 0)
                        return "0";
                    else
                        return "{-" + _args[0].ToLatex() + "}";

                case "fe'a":
                    if (_args.Count == 0)
                        return "0";
                    else
                        return "\\sqrt{" + _args[0].ToLatex() + "}";

                default:
                    return "unsupported op: " + _operator.ToString();
            }
        }

        public override double Evaluate()
        {
            double ans = 0; //for n-ary ops
            double x1 = 0;
            double x2 = 0;
            double x3 = 0;

            switch (_operator.ToString())
            {
                case "+":
                case "su'i":
                    ans = 0; //default valuNonTerminal for +
                    foreach (NonTerminal d in _args)
                        ans += d.Evaluate();
                    return ans;
                case "-":
                case "vu'u":
                    ans = 0; //default valuNonTerminal for -
                    if (_args.Count > 0)
                    {
                        ans = _args[0].Evaluate();
                        for (int i = 1; i < _args.Count; i++)
                            ans -= _args[i].Evaluate();
                    }
                    return ans;
                case "*":
                case "pi'i":
                    ans = 1; //default valuNonTerminal for *
                    foreach (NonTerminal d in _args)
                        ans *= d.Evaluate();
                    return ans;
                case "/":
                case "fe'i":
                    ans = 1; //default valuNonTerminal for /
                    if (_args.Count > 0)
                    {
                        ans = _args[0].Evaluate();
                        for (int i = 1; i < _args.Count; i++)
                            ans /= _args[i].Evaluate();
                    }
                    return ans;
                case "fa'i":
                    ans = 1;
                    if (_args.Count > 0)
                        ans = 1 / _args[0].Evaluate();
                    return ans;
                case "gei":
                    x1 = 0;
                    x2 = 1;
                    x3 = 10;
                    if (_args.Count > 0)
                        x1 = _args[0].Evaluate();
                    if (_args.Count > 1)
                        x2 = _args[1].Evaluate();
                    if (_args.Count > 2)
                        x3 = _args[2].Evaluate();
                    return x2 * Math.Pow(x3, x1);
                case "^":
                case "te'a":
                    x1 = 1;
                    x2 = 0;
                    if (_args.Count > 0)
                        x1 = _args[0].Evaluate();
                    if (_args.Count > 1)
                        x2 = _args[1].Evaluate();
                    return Math.Pow(x1, x2);
                case "cu'a":
                    x1 = 0;
                    if (_args.Count > 0)
                        x1 = _args[0].Evaluate();
                    return Math.Abs(x1);
                case "de'o":
                    x1 = 1;
                    x2 = Math.E;
                    if (_args.Count > 0)
                        x1 = _args[0].Evaluate();
                    if (_args.Count > 1)
                        x2 = _args[1].Evaluate();
                    return Math.Log(x1, x2);
                case "fe'a":
                    x1 = 1;
                    x2 = 2;
                    if (_args.Count > 0)
                        x1 = _args[0].Evaluate();
                    if (_args.Count > 1)
                        x2 = _args[1].Evaluate();
                    return Math.Pow(x1, 1 / x2);
                case "ne'o":
                    x1 = 1;
                    if (_args.Count > 0)
                        x1 = _args[0].Evaluate();
                    return factorial(x1);
                case "va'a":
                    x1 = 0;
                    if (_args.Count > 0)
                        x1 = _args[0].Evaluate();
                    return -x1;
                default:
                    throw new ArgumentException("An invalid operator: " + _operator.ToString());
            }
        }

        static double factorial(double x)
        {
            if (x < 0)
                return 0;
            if (x == 0)
                return 1;
            return x * factorial(x - 1);
        }
    }
}

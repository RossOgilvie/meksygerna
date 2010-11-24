using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar
{
    class Expression
    {
        public Expression()
        {
            Args = new List<Expression>();
            PAs = new List<string>();
            Ls = new List<string>();
            Notation = OperatorNotation.Infix;
            ExprType = ExpressionType.Expression;
        }

        public string Op;
        public List<Expression> Args;
        public List<string> PAs;
        public List<string> Ls;
        public OperatorNotation Notation;
        public ExpressionType ExprType;

        public override string ToString()
        {
            //If this is a number, return the value of the number, otherwise return the operator name
            switch (ExprType)
            {
                case ExpressionType.Number:
                    return ((double)this).ToString();
                case ExpressionType.Letter:
                    string result = "";
                    foreach (string s in Ls)
                    {
                        switch (s.Length)
                        {
                            case 2:
                            case 3:
                                result += s[0];
                                break;
                            default:
                                result += s;
                                break;
                        }

                    }
                    return result;
                default:
                    return Op;
            }
        }

        public string OutputPolish()
        {
            string result = "";
            switch (ExprType)
            {
                case ExpressionType.Number:
                    result = ((double)this) + " ";
                    break;
                case ExpressionType.Letter:
                    foreach (string s in Ls)
                    {
                        switch (s.Length)
                        {
                            case 2:
                            case 3:
                                result += s[0];
                                break;
                            default:
                                result += s;
                                break;
                        }

                    }
                    break;
                default:
                    result += "(" + Op + " ";
                    foreach (Expression e in Args)
                        result += e.OutputPolish();
                    result += ")";
                    break;
            }

            return result;
        }

        public string OutputPolishVerbose()
        {
            string result = "";
            switch (ExprType)
            {
                case ExpressionType.Number:
                    result = ((double)this) + " ";
                    break;
                case ExpressionType.Letter:
                    foreach (string s in Ls)
                    {
                        switch (s.Length)
                        {
                            case 2:
                            case 3:
                                result += s[0];
                                break;
                            default:
                                result += s;
                                break;
                        }

                    }
                    break;
                default:
                    string notation = "";
                    switch (Notation)
                    {
                        case OperatorNotation.Infix:
                            notation = "i";
                            break;
                        case OperatorNotation.Polish:
                            notation = "p";
                            break;
                        case OperatorNotation.ReversePolish:
                            notation = "r";
                            break;
                        case OperatorNotation.InfixBO:
                            notation = "b";
                            break;
                    }

                    result += "(" + Op + " " + notation + " ";
                    foreach (Expression e in Args)
                        result += e.OutputPolishVerbose();
                    result += ")";
                    break;
            }

            return result;
        }

        public string OutputLatex()
        {
            string result = "";
            switch (ExprType)
            {
                case ExpressionType.Number:
                    result = ((double)this) + " ";
                    break;
                case ExpressionType.Letter:
                    foreach (string s in Ls)
                    {
                        switch (s.Length)
                        {
                            case 2:
                            case 3:
                                result += s[0];
                                break;
                            default:
                                result += s;
                                break;
                        }

                    }
                    break;
                default:
                    switch (Op)
                    {
                        case "+":
                        case "su'i":
                            if (Args.Count == 0)
                                return "0";
                            else
                            {
                                result = Args[0].OutputLatex();
                                for (int i = 1; i < Args.Count; i++)
                                    result += "+ " + Args[i].OutputLatex();
                            }
                            break;
                        case "-":
                        case "vu'u":
                            if (Args.Count == 0)
                                return "0";
                            else
                            {
                                result = Args[0].OutputLatex();
                                for (int i = 1; i < Args.Count; i++)
                                    result += "- " + Args[i].OutputLatex();
                            }
                            break;
                        case "*":
                        case "pi'i":
                            if (Args.Count == 0)
                                return "1";
                            else
                            {
                                result = Args[0].OutputLatex();
                                for (int i = 1; i < Args.Count; i++)
                                    result += "\\times " + Args[i].OutputLatex();
                            }
                            break;
                        case "/":
                        case "fe'i":
                            if (Args.Count == 0)
                                return "1";
                            else if (Args.Count == 1)
                                return Args[0].OutputLatex();
                            else
                            {
                                result = "\\frac{" + Args[0].OutputLatex() + "}{" + Args[1].OutputLatex();
                                for (int i = 2; i < Args.Count; i++)
                                    result += "\\times " + Args[i].OutputLatex();
                                result += "}";
                            }
                            break;
                        case "te'a":
                        case "^":
                            if (Args.Count == 0)
                                return "1";
                            else if (Args.Count == 1)
                                return Args[0].OutputLatex();
                            else
                            {
                                result = "{" + Args[0].OutputLatex() + "}^{" + Args[1].OutputLatex() + "}";
                            }
                            break;
                        case "fa'i":
                            if (Args.Count == 0)
                                return "1";
                            else
                                return "\\frac{1}{" + Args[0].OutputLatex() + "}";
                        case "va'a":
                            if (Args.Count == 0)
                                return "0";
                            else
                                return "{-" + Args[0].OutputLatex() + "}";
                        case "fe'a":
                            if (Args.Count == 0)
                                return "0";
                            else
                                return "\\sqrt{" + Args[0].OutputLatex() + "}";
                        default:
                            result = "unsupported op: " + Op;
                            break;
                    }
                    break;
            }

            return result;
        }

        //evaluate
        public static implicit operator double(Expression exp)
        {
            //handling for numbers
            if(exp.ExprType == ExpressionType.Number)
            {
                double value = 0;
                //first, turn any words into numerals
                for (int i = 0; i < exp.PAs.Count; i++)
                {
                    switch (exp.PAs[i])
                    {
                        case "no":
                            exp.PAs[i] = "0";
                            break;
                        case "pa":
                            exp.PAs[i] = "1";
                            break;
                        case "re":
                            exp.PAs[i] = "2";
                            break;
                        case "ci":
                            exp.PAs[i] = "3";
                            break;
                        case "vo":
                            exp.PAs[i] = "4";
                            break;
                        case "mu":
                            exp.PAs[i] = "5";
                            break;
                        case "xa":
                            exp.PAs[i] = "6";
                            break;
                        case "ze":
                            exp.PAs[i] = "7";
                            break;
                        case "bi":
                            exp.PAs[i] = "8";
                            break;
                        case "so":
                            exp.PAs[i] = "9";
                            break;
                    }
                }

                //then make the value of the number. don't have fraction or decimal handling yet
                foreach (string p in exp.PAs)
                {
                    double a = Convert.ToDouble(p);
                    value *= 10;
                    value += a;
                }

                return value;
            }

            double ans = 0; //for n-ary ops
            double x1 = 0;
            double x2 = 0;
            double x3 = 0;

            switch (exp.Op)
            {
                case "+":
                case "su'i":
                    ans = 0; //default value for +
                    foreach (double d in exp.Args)
                        ans += d;
                    return ans;
                case "-":
                case "vu'u":
                    ans = 0; //default value for -
                    if (exp.Args.Count > 0)
                    {
                        ans = exp.Args[0];
                        for (int i = 1; i < exp.Args.Count; i++)
                            ans -= exp.Args[i];
                    }
                    return ans;
                case "*":
                case "pi'i":
                    ans = 1; //default value for *
                    foreach (double d in exp.Args)
                        ans *= d;
                    return ans;
                case "/":
                case "fe'i":
                    ans = 1; //default value for /
                    if (exp.Args.Count > 0)
                    {
                        ans = exp.Args[0];
                        for (int i = 1; i < exp.Args.Count; i++)
                            ans /= exp.Args[i];
                    }
                    return ans;
                case "fa'i":
                    ans = 1;
                    if (exp.Args.Count > 0)
                        ans = 1 / exp.Args[0];
                    return ans;
                case "gei":
                    x1 = 0;
                    x2 = 1;
                    x3 = 10;
                    if (exp.Args.Count > 0)
                        x1 = exp.Args[0];
                    if (exp.Args.Count > 1)
                        x2 = exp.Args[1];
                    if (exp.Args.Count > 2)
                        x3 = exp.Args[2];
                    return x2 * Math.Pow(x3, x1);
                case "^":
                case "te'a":
                    x1 = 1;
                    x2 = 0;
                    if (exp.Args.Count > 0)
                        x1 = exp.Args[0];
                    if (exp.Args.Count > 1)
                        x2 = exp.Args[1];
                    return Math.Pow(x1, x2);
                case "cu'a":
                    x1 = 0;
                    if (exp.Args.Count > 0)
                        x1 = exp.Args[0];
                    return Math.Abs(x1);
                case "de'o":
                    x1 = 1;
                    x2 = Math.E;
                    if (exp.Args.Count > 0)
                        x1 = exp.Args[0];
                    if (exp.Args.Count > 1)
                        x2 = exp.Args[1];
                    return Math.Log(x1, x2);
                case "fe'a":
                    x1 = 1;
                    x2 = 2;
                    if (exp.Args.Count > 0)
                        x1 = exp.Args[0];
                    if (exp.Args.Count > 1)
                        x2 = exp.Args[1];
                    return Math.Pow(x1, 1 / x2);
                case "ne'o":
                    x1 = 1;
                    if (exp.Args.Count > 0)
                        x1 = exp.Args[0];
                    return factorial(x1);
                case "va'a":
                    x1 = 0;
                    if (exp.Args.Count > 0)
                        x1 = exp.Args[0];
                    return -x1;
                default:
                    throw new ArgumentException("An invalid operator: " + exp.Op);
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

        public enum OperatorNotation
        {
            Infix, Polish, ReversePolish, InfixBO
        }

        public enum ExpressionType
        {
            Expression, Number, Letter, Operator
        }
    }
}

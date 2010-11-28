using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MexGrammar
{
    public class Lexer
    {
        public Lexer(string input)
        {
            _Stream = new List<Token>();
            string currWord = "";
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (isVowel(c))
                    currWord += c;
                else if (isConsonant(c))
                {
                    //if it's a consonant, start a new word
                    currWord = flushWord(currWord, _Stream);
                    currWord += c;
                }
                else if (isSymbol(c) || isDigit(c))
                {
                    //if it's a symbol or digit, finish the current word and add the symbol/digit
                    currWord = flushWord(currWord, _Stream);
                    flushWord(c + "", _Stream);
                }
                else if (isSpace(c))
                {
                    //if it's a space, then finish off the word
                    currWord = flushWord(currWord, _Stream);
                    //check it wasn't a space at the beginning of the input.
                    if (_Stream.Count > 0)
                    {
                        //if that word was a PA, then add a boi (spaces terminate numbers)
                        if (_Stream[_Stream.Count - 1].Type == Selmaho.PA)
                            _Stream.Add(new Token("boi"));
                        //if that word was an A, then add a bu (spaces change A into A BU)
                        if (_Stream[_Stream.Count - 1].Type == Selmaho.A)
                            _Stream.Add(new Token("bu"));
                        //if that word was an letter (BY or BU), then add a boi (spaces terminate letters)
                        if (_Stream[_Stream.Count - 1].Type == Selmaho.BU || _Stream[_Stream.Count - 1].Type == Selmaho.BY)
                            _Stream.Add(new Token("boi"));
                    }
                }
                else if (isDot(c))
                {
                    //if it's a dot, then finish off the word
                    currWord = flushWord(currWord, _Stream);
                    //check it wasn't a . at the beginning of the input.
                    if (_Stream.Count > 0)
                    {
                        //if that word was a PA, then add a boi (dots terminate numbers)
                        if (_Stream[_Stream.Count - 1].Type == Selmaho.PA)
                            _Stream.Add(new Token("boi"));
                        //if that word was an A, then add a bu (dots change A into A BU)
                        if (_Stream[_Stream.Count - 1].Type == Selmaho.A)
                            _Stream.Add(new Token("bu"));
                    }
                }
            }

            //Put anything remaining into a token.
            currWord = flushWord(currWord, _Stream);

            //Put the end of stream token at the end.
            _Stream.Add(new Token("eof!"));
        }

        string flushWord(string word, List<Token> stream)
        {
            if (word.Length > 0)
            {
                Token nt = new Token(word);
                _Stream.Add(nt);
            }
            return "";
        }

        private List<Token> _Stream;
        private int _Pointer = 0;

        public Token Current { get { return _Stream[_Pointer]; } }
        public Token Peek { get { return _Stream[_Pointer + 1]; } }
        public string Advance()
        {
            string s = Current.Value;
            _Pointer++;
            return s;
        }
        public void Advance(int distance)
        {
            _Pointer += distance;
        }
        public int Position { get { return _Pointer; } }
        public void Seek(int Position) { _Pointer = Position; }
        public int Length { get { return _Stream.Count; } }

        #region Character sorting functions
        private bool isSpace(char c)
        {
            return c == ' ';
        }
        private bool isDigit(char c)
        {
            for (int i = 0; i <= 9; i++)
                if (i.ToString()[0] == c)
                    return true;
            return false;
        }
        private bool isLetter(char c)
        {
            Regex r = new Regex("[a-zA-Z']");
            return r.IsMatch(c.ToString());
        }
        private bool isConsonant(char c)
        {
            Regex r = new Regex("[bcdfgjklmnprstvxzBCDFGJKLMNPRSTVXZ]");
            return r.IsMatch(c.ToString());
        }
        private bool isVowel(char c)
        {
            Regex r = new Regex("[aeiouyAEIOUY']");
            return r.IsMatch(c.ToString());
        }
        private bool isSymbol(char c)
        {
            //plus minus multiply divide brackets
            //Have to escape alot of these characters
            Regex r = new Regex(@"[\+\-\*/\(\)\^]");
            return r.IsMatch(c.ToString());
        }
        private bool isDot(char c)
        {
            return c == '.' ? true: false;
        }
        #endregion
    }

    public class Token
    {
        public Token(string value)
        {
            _Value = value;
            setType(value);
        }

        private string _Value;
        public string Value
        { get { return _Value; } }

        private Selmaho _Type;
        public Selmaho Type
        { get { return _Type; } }

        private void setType(string s)
        {
            s = s.ToLower();
            switch (s)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "^":
                case "su'i":
                case "vu'u":
                case "pi'i":
                case "fe'i":
                case "fa'i":
                case "gei":
                case "te'a":
                case "cu'a":
                case "de'o":
                case "fe'a":
                case "ne'o":
                case "va'a":
                    _Type = Selmaho.Operator;
                    break;
                case "eof!":
                    _Type = Selmaho.EndOfStream;
                    break;
                case "fu'a":
                case "pe'o":
                case "vei":
                case "(":
                    _Type = Selmaho.VEI;
                    break;
                case "ve'o":
                case ")":
                    _Type = Selmaho.VEhO;
                    break;
                case "ku'e":
                    _Type = Selmaho.KUhE;
                    break;
                case "bo":
                    _Type = Selmaho.BO;
                    break;
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "no":
                case "pa":
                case "re":
                case "ci":
                case "vo":
                case "mu":
                case "xa":
                case "ze":
                case "bi":
                case "so":
                    _Type = Selmaho.PA;
                    break;
                case "boi":
                    _Type = Selmaho.BOI;
                    break;
                case "bu":
                    _Type = Selmaho.BU;
                    break;
                case "by":
                case "cy":
                case "dy":
                case "fy":
                case "gy":
                case "jy":
                case "ky":
                case "ly":
                case "my":
                case "ny":
                case "py":
                case "ry":
                case "sy":
                case "ty":
                case "vy":
                case "wy":
                case "xy":
                case "zy":
                case "b":
                case "c":
                case "d":
                case "f":
                case "g":
                case "j":
                case "k":
                case "l":
                case "m":
                case "n":
                case "p":
                case "r":
                case "s":
                case "t":
                case "v":
                case "x":
                case "z":
                    _Type = Selmaho.BY;
                    break;
                case "a":
                case "e":
                case "i":
                case "o":
                case "u":
                case "y'y":
                    _Type = Selmaho.A;
                    break;
                default:
                    _Type = Selmaho.Unknown;
                    break;
            }
        }

        public static implicit operator Selmaho(Token t)
        {
            return t.Type;
        }
        public static implicit operator string(Token t)
        {
            return t.Value;
        }

        public override string ToString()
        {
            return _Value;
        }
    }

    public enum Selmaho
    { A, BO, BOI, BU, BY, KUhE, Operator, PA, VEI, VEhO, Unknown, EndOfStream }
}
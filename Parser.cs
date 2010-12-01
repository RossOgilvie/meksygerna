using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MexGrammar.Productions;

namespace MexGrammar
{
    class Parser
    {
        private Lexer lex;
        public Parser(Lexer l)
        {
            lex = l;
            ProductionStorage ps = new ProductionStorage(l);
            if (!ps.MatchProduction<Mex>(out Result))
                throw new ParseError("Parse failed.", l.Current);
            if (l.Current.Type != Selmaho.EndOfStream)
                throw new ParseError("Parse ended before end of input.", l.Current);
        }

        public Mex Result;
    }

    class ParseError : Exception
    {
        public ParseError(string msg, Token tok):base(msg)
        {
            token = tok;
        }

        public Token token;
    }
}

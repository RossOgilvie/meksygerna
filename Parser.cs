using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MexGrammar
{
    class Parser
    {
        private Lexer lex;
        public Parser(Lexer l)
        {
            lex = l;
            Result = mex();
            if (l.Current.Type != Terminals.EndOfStream)
                throw new ParseError("Parse ended before end of input.", l.Current);
        }

        public Expression Result;

        #region grammar functions
        //mex-1 (operator mex-1 | mex * operator /KUhE#/) *
        private Expression mex()
        {
            //mex-1 [mex-1 ... operator]
            List<Expression> args = new List<Expression>();
            args.Add(mex1());

            //save our position, then try to match the bracket
            int save = lex.Position;

            while (true)
            {
                tryToMatchResult r = tryToMatch(mex1);
                if (r.success)
                    args.Add(r.result);
                else
                    break;
            }

            //if there is an operator, then we have matched the bracket, otherwise not and we have to roll back
            if (lex.Current.Type == Terminals.Operator)
            {
                Expression e = new Expression();
                e.Op = lex.Advance();
                e.Args = args;
                e.Notation = Expression.OperatorNotation.ReversePolish;
                return e;
            }
            else
            {
                lex.Seek(save);
                return args[0];
            }
        }

        //mex-1 = mex-2 [operator mex-2] ...
        private Expression mex1()
        {
            Expression result = mex2();

            //[operator mex-2] ... 
            while (true)
            {
                if (lex.Current.Type == Terminals.Operator)
                {
                    string op = lex.Advance();
                    tryToMatchResult r = tryToMatch(mex2);
                    if (r.success)
                    {
                        // if you make a whole bracket
                        // then turn that into an infix expression
                        //this also encodes the left associativity of infix.
                        Expression e = new Expression();
                        e.Op = op;
                        e.Args = new List<Expression> { result, r.result };
                        e.Notation = Expression.OperatorNotation.Infix;
                        result = e;
                    }
                    else
                    {
                        //Have to wind back to the start of the bracket, the trytomatch only goes back to the op advancement
                        lex.Seek(lex.Position - 1);
                        break;
                    }
                }
                else
                    break;
            }
            return result;
        }

        //mex-2 = mex-3 [operator [stag] BO# mex-3]...
        private Expression mex2()
        {
            Expression result = mex3();

            //[operator BO mex-3] ... 
            while (true)
            {
                if (lex.Current.Type == Terminals.Operator)
                {
                    string op = lex.Advance();
                    if (lex.Current.Type == Terminals.BO)
                    {
                        lex.Advance(); //eat the bo
                        tryToMatchResult r = tryToMatch(mex3);
                        if (r.success)
                        {
                            // if you make a whole bracket
                            // then turn that into an infix expression
                            Expression e = new Expression();
                            e.Op = op;
                            e.Args = new List<Expression> { result, r.result };
                            e.Notation = Expression.OperatorNotation.InfixBO;
                            result = e;
                        }
                        else
                        {
                            //Have to wind back to the satrt of the bracket, the trytomatch only goes back to the op advancement
                            lex.Seek(lex.Position - 2);
                            break;
                        }
                    }
                    else
                    {
                        lex.Seek(lex.Position - 1);
                        break;
                    }
                }
                else
                    break;
            }
            return result;
        }

        //mex-3 = operator mex-1 ... /KUhE/ | mex-4
        private Expression mex3()
        {
            if (lex.Current.Type == Terminals.Operator)
            {
                //operator mex-1 ... /KUhE/
                Expression e = new Expression();
                e.Op = lex.Advance();
                e.Notation = Expression.OperatorNotation.Polish;
                List<Expression> args = new List<Expression>();
                args.Add(mex1());
                while (true)
                {
                    tryToMatchResult r = tryToMatch(mex1);
                    if (r.success)
                        args.Add(r.result);
                    else
                        break;
                }
                e.Args = args;

                if (lex.Current.Type == Terminals.KUhE)
                    lex.Advance();

                return e;
            }
            else
            {
                return mex4();
            }
        }

        /*
         * mex-4 =						
	    >    number /BOI#/		
	        | lerfu-string /BOI#/
	    >   | VEI# mex /VEhO#/
	        | NIhE# selbri /TEhU#/
	        | MOhE# sumti /TEhU#/
	        | gek mex gik mex-2
        	| (LAhE# / NAhE# BO#) mex /LUhU#/
        */
        private Expression mex4()
        {
            if (lex.Current.Type == Terminals.VEI)
            {
                //VEI# mex /VEhO#/

                //eat the VEI
                lex.Advance();
                Expression e2 = new Expression();
                e2 = mex();
                //If there's an ellidible VEhO, eat it
                if (lex.Current == Terminals.VEhO)
                    lex.Advance();
                return e2;
            }
            else
            {
                //number /BOI#/
                tryToMatchResult r = tryToMatch(number);
                if (r.success)
                {
                    //eat the boi if it's there
                    if (lex.Current.Type == Terminals.BOI)
                        lex.Advance();
                    return r.result;
                }

                //lerfuString /BOI/
                tryToMatchResult s = tryToMatch(lerfuString);
                if (s.success)
                {
                    //eat the boi if it's there
                    if (lex.Current.Type == Terminals.BOI)
                        lex.Advance();
                    return r.result;
                }
            }

            throw new ParseError("Error in mex4", lex.Current);
        }

        //number = PA ...
        private Expression number()
        {
            Expression result = new Expression();
            if (lex.Current.Type == Terminals.PA)
            {
                result.PAs.Add(lex.Advance());
                result.isNum = true;
                while (true)
                {
                    if (lex.Current.Type == Terminals.PA)
                    {
                        result.PAs.Add(lex.Advance());
                    }
                    else
                        break;
                }
                return result;
            }
            else
                throw new ParseError("Error in number", lex.Current);
        }

        //lerfuString = (BY | A BU) ...
        private Expression lerfuString()
        {
            Expression e = new Expression();
            e.isLetter = true;

            if (lex.Current.Type == Terminals.BY)
            {
                e.Ls.Add(lex.Advance());
            }
            else if (lex.Current.Type == Terminals.A)
            {
                if (lex.Peek.Type == Terminals.BU)
                {
                    e.Ls.Add(lex.Advance() + lex.Advance());
                }
            }
            else
                throw new ParseError("Error in lerfu string", lex.Current); 

            while(true)
            {
                if (lex.Current.Type == Terminals.BY)
                {
                    e.Ls.Add(lex.Advance());
                }
                else if (lex.Current.Type == Terminals.A)
                {
                    if(lex.Peek.Type == Terminals.BU)
                    {
                        e.Ls.Add(lex.Advance() +lex.Advance());
                    }
                }
                else
                    break; 
            }

            return e;
        }

        #endregion

        private delegate Expression ParseMethod();
        struct tryToMatchResult
        {
            public bool success;
            public Expression result;
        }
        private tryToMatchResult tryToMatch(ParseMethod m)
        {
            int save = lex.Position;
            tryToMatchResult r = new tryToMatchResult();
            try
            {
                r.result = m.Invoke();
                r.success = true;
            }
            catch(Exception e)
            {
                lex.Seek(save);
                r.success = false;
            }
            return r;
        }
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

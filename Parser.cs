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
        //mex = mex-1 (operator mex-1 | mex * operator /KUhE#/) *
        private Expression mex()
        {
            //Get the mex1
            Expression result = new Expression();
            result = mex1();

            //add this to be the first in a potential sequence of arguments
            List<Expression> args = new List<Expression>();
            args.Add(result);

            //save our position, then try to match the bracket repeatedly
            int save = lex.Position;

            //initialise
            tryToMatchResult r;

            while (true)
            {
                //try to match an infix expression
                r = tryToMatch(new List<ParseMethod> { operato, mex1 });
                if (r.success)
                {
                    //we have successfully matched an infix expression
                    Expression e = new Expression();
                    e.Notation = Expression.OperatorNotation.Infix;
                    e.Op = r.result[0].Op;
                    args.Add(r.result[1]);
                    e.Args = args;
                    e.ExprType = Expression.ExpressionType.Expression;

                    //this has made an expression with all the data we got
                    //now reset for the next loop
                    result = e;
                    args.Clear();
                    args.Add(e);

                    // move the save forward to this point
                    save = lex.Position;
                }
                    //if that didn't match, try matching an RP expression
                else
                {
                    //grab as many mex as you can to be arguments.
                    while (true)
                    {
                        r = tryToMatch(mex);
                        if (r.success)
                        {
                            args.Add(r.result[0]);
                        }
                        else
                            break;
                    }

                    //now try to match an operator
                    r = tryToMatch(operato);
                    if (r.success)
                    {
                        //successful have matched a whole RP
                        //turn it into an Expression
                        Expression e = new Expression();
                        e.Notation = Expression.OperatorNotation.ReversePolish;
                        e.Op = r.result[0].Op;
                        e.Args = args;
                        e.ExprType = Expression.ExpressionType.Expression;

                        //eat the optional KUhE
                        if (lex.Current.Type == Terminals.KUhE)
                            lex.Advance();

                        //clean up for next pass of loop
                        result = e;
                        args.Clear();
                        args.Add(e);

                        // move the save forward to this point
                        save = lex.Position;
                    }
                    else
                    {
                        //we have failed to match an infix and an RP, so we are done
                        break;
                    }
                }
            }

            //resore to last place we had a fulle match
            lex.Seek(save);
            return result;
        }

        //mex-1 = mex-2 (operator BO mex-2)*
        private Expression mex1()
        {
            Expression result = mex2();

            //note where we are.
            int save = lex.Position;

            while (true)
            {
                //try and get the operator
                tryToMatchResult r = tryToMatch(operato);
                //if you've got the operator and the BO after it
                if (r.success && lex.Current.Type == Terminals.BO)
                {
                    lex.Advance(); //eat the bo

                    //try and get the mex2 after the operator
                    tryToMatchResult s = tryToMatch(mex2);
                    if (s.success)
                    {
                        // if you make a whole bracket
                        // then turn that into an infix expression
                        Expression e = new Expression();
                        e.Op = r.result[0].Op;
                        e.Args = new List<Expression> { result, s.result[0] };
                        e.Notation = Expression.OperatorNotation.InfixBO;
                        e.ExprType = Expression.ExpressionType.Expression;
                        result = e;
                        save = lex.Position;
                    }
                    else
                        break;
                }
                else
                    break;
            }

            //return the point of the last complete match.
            lex.Seek(save);
            return result;
        }

        //mex-3 = operator mex-1 ... /KUhE/ | mex-4
        private Expression mex2()
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
                return operand();
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
        private Expression operand()
        {
            if (lex.Current.Type == Terminals.VEI)
            {
                //VEI# mex /VEhO#/

                //eat the VEI
                lex.Advance();
                Expression e = mex();
                //If there's an ellidible VEhO, eat it
                if (lex.Current == Terminals.VEhO)
                    lex.Advance();
                return e;
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
                    return r.result[0];
                }

                //lerfuString /BOI/
                tryToMatchResult s = tryToMatch(lerfuString);
                if (s.success)
                {
                    //eat the boi if it's there
                    if (lex.Current.Type == Terminals.BOI)
                        lex.Advance();
                    return r.result[0];
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
                result.ExprType = Expression.ExpressionType.Number;
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
            e.ExprType = Expression.ExpressionType.Letter;

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

        //operator = Operator
        private Expression operato()
        {
            if(lex.Current.Type == Terminals.Operator)
            {
                Expression e = new Expression();
                e.ExprType = Expression.ExpressionType.Operator;
                e.Op = lex.Advance();
                return e;
            }
            else
                throw new ParseError("Expected operator",lex.Current);
        }
        #endregion

        private delegate Expression ParseMethod();
        struct tryToMatchResult
        {
            public bool success;
            public List<Expression> result;
        }
        private tryToMatchResult tryToMatch(ParseMethod m)
        {
            return tryToMatch(new List<ParseMethod> { m });
        }
        private tryToMatchResult tryToMatch(List<ParseMethod> m)
        {
            int save = lex.Position;
            tryToMatchResult r = new tryToMatchResult();
            r.result = new List<Expression>();
            foreach (ParseMethod meth in m)
            {
                try
                {
                    r.result.Add(meth.Invoke());
                    r.success = true;
                }
                catch
                {
                    lex.Seek(save);
                    r.success = false;
                }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MexGrammar.Productions;

namespace MexGrammar
{
    public class ProductionStorage
    {
        Lexer _Lex;

        Dictionary<Type,NonTerminal>[] _Storage;

        public ProductionStorage(Lexer lex)
        {
            _Lex = lex;
            _Storage = new Dictionary<Type,NonTerminal>[lex.Length];
            for (int i = 0; i < lex.Length; i++)
                _Storage[i] = new Dictionary<Type, NonTerminal>();
        }

        /// <summary>
        /// Gets the requested type starting at the requested position. If returns true, it's advanced to the end of the token used to make that production. 
        /// If it returns false, the lexer stays unchanged
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool MatchProduction<E>(out E Result) where E : NonTerminal, new()
        {
            NonTerminal res;
            int StartPos = _Lex.Position;

            //try to find the object of the appropriate type starting at the appropriate place
            if (_Storage[StartPos].TryGetValue(typeof(E), out res))
            {
                //make sure that value isn't a null marker
                if (res != null)
                {
                    Result = (E)res;
                    //advance all the tokens this uses
                    _Lex.Advance(res.Length);
                    return true;
                }
                else
                {
                    Result = null;
                    return false;
                }
            }
            else
            {
                //if it wasn't already in the storage, then we'll need to put it there
                E res2 = new E();

                //try to create the object proper
                if (res2.CreateNonTerminal(_Lex, this))
                {
                    _Storage[StartPos].Add(typeof(E), res2);
                    Result = res2;
                    //Calculate how far to Lexer has advanced to figure out how long this production is
                    Result.Length = _Lex.Position - StartPos;
                    //no need to advance the lexer, CreateNonTerminal has already done this.
                    return true;
                }
                else
                {
                    //we failed to make the production at this posistion
                    //mark it so we don't waste time trying in the future.
                    _Storage[StartPos].Add(typeof(E), null);
                    Result = null;
                    return false;
                }
            }
        }

        public bool MatchProduction(Selmaho Type, out string Value)
        {
            if (_Lex.Current.Type == Type)
            {
                Value = _Lex.Advance();
                return true;
            }
            else
            {
                Value = "";
                return false;
            }
        }

        public bool MatchProduction(Selmaho Type)
        {
            string s;
            return MatchProduction(Type, out s);
        }
    }
}

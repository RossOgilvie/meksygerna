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
        Dictionary<ProdKey,NonTerminal> _Storage;

        public ProductionStorage(Lexer lex)
        {
            _Lex = lex;
            _Storage = new Dictionary<ProdKey, NonTerminal>();
        }

        /// <summary>
        /// Gets the requested type starting at the requested position. If returns true, it's advanced to the end of the token used to make that production. 
        /// If it returns false, the lexer stays unchanged
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool Retrieve<E>(out E Result) where E : NonTerminal, new()
        {
            NonTerminal res;

            //try to find the object of the appropriate type starting at the appropriate place
            if (_Storage.TryGetValue(new ProdKey(_Lex.Position, typeof(E)), out res))
            {
                Result = (E)res;
                //advance all the tokens this uses
                _Lex.Advance(res.Length);
                return true;
            }
            else
            {
                //if it wasn't already in the storage, then we'll need to put it there
                E res2 = new E();
                int pos = _Lex.Position;

                //try to create the object proper
                if (res2.CreateNonTerminal(_Lex, this))
                {
                    _Storage.Add(new ProdKey(pos, typeof(E)), res);
                    Result = res2;
                    //no need to advance the lexer, CreateNonTerminal has already done this.
                    return true;
                }
                else
                {
                    //we failed to make the production at this posistion
                    //mark it so we don't waste time trying in the future.
                    _Storage.Add(new ProdKey(pos, typeof(E)), null);
                    Result = null;
                    return false;
                }
            }
        }

        private struct ProdKey
        {
            public int Position;
            public Type Production;

            public ProdKey(int pos, Type prod)
            {
                Position = pos;
                Production = prod;
            }
        }
    }
}

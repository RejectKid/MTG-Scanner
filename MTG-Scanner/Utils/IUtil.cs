using MTG_Scanner.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MTG_Scanner.Utils
{
    public interface IUtil
    {
        void TraverseTree(string root, List<MagicCard> listOfMagicCards);
        string GetVariableName<T>(Expression<Func<T>> expression);
    }
}

using MTG_Scanner.Models;
using System.Collections.Generic;

namespace MTG_Scanner.Utils
{
    public interface IUtil
    {
        void TraverseTree(string root, List<MagicCard> listOfMagicCards);
    }
}

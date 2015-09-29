using System.Collections.Generic;

namespace MTG_Scanner.Models
{
    public interface ICardDatabase
    {
        List<MagicCard> ListOfAllMagicCards { get; }
    }
}

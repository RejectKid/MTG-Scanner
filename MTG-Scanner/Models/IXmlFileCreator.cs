using System.Collections.Generic;

namespace MTG_Scanner.Models
{
    public interface IXmlFileCreator
    {
        string CreateXmlDb(List<MagicCard> listOfCards);
    }
}

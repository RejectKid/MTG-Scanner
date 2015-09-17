using System.Collections.Generic;

namespace MTG_Scanner.Models
{
    interface IXmlFileCreator
    {
        string CreateXmlDb(List<MagicCard> listOfCards);
    }
}

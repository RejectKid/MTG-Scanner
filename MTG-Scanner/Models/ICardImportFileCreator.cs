using System.Collections.Generic;

namespace MTG_Scanner.Models.Impl
{
    public interface ICardImportFileCreator
    {
        List<MagicCard> ListOfMatchedCards { get; set; }
        string FilePath { get; set; }
        bool CardImportStarted { get; set; }
        void CreateCardListFile();
    }
}
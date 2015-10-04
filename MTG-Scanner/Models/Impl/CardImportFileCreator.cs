using System.Collections.Generic;
using System.IO;

namespace MTG_Scanner.Models.Impl
{
    class CardImportFileCreator : ICardImportFileCreator
    {
        public List<MagicCard> ListOfMatchedCards { get; set; } = new List<MagicCard>();
        public string FilePath { get; set; }
        public bool CardImportStarted { get; set; }

        public void CreateCardListFile()
        {
            using (var fileStream = new StreamWriter(FilePath))
            {
                fileStream.WriteLine("Name, Edition, Quantity, Foil");

                foreach (var distinctCountCard in ListOfMatchedCards)
                {
                    fileStream.WriteLine(SurroundWithQuotes(distinctCountCard.Name) + ", " +
                                         SurroundWithQuotes(distinctCountCard.Set) + ", " +
                                         SurroundWithQuotes(1.ToString()) + ", " +
                                         SurroundWithQuotes(distinctCountCard.IsFoil.ToString()));
                }
                fileStream.Close();
            }
        }

        private static string SurroundWithQuotes(string name)
        {
            return "\"" + name + "\"";
        }
    }
}

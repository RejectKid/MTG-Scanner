using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MTG_Scanner.Models.Impl
{
    class CardImportFileCreator : ICardImportFileCreator
    {
        public List<MagicCard> ListOfMatchedCards { get; set; } = new List<MagicCard>();
        public string FilePath { get; set; }
        public bool CardImportStarted { get; set; }

        public void CreateCardListFile()
        {
            var distinctCountCards = (from cards in ListOfMatchedCards
                                      group cards by cards.Name into c
                                      select new
                                      {
                                          Name = c.Key,
                                          Count = c.Count()
                                      }).ToList();

            using (var fileStream = new StreamWriter(FilePath))
            {
                foreach (var distinctCountCard in distinctCountCards)
                {
                    fileStream.WriteLineAsync(distinctCountCard.Count + " " + distinctCountCard.Name);
                }
                fileStream.Close();
            }
        }
    }
}

namespace MTG_Scanner.Models
{
    public class MagicCard
    {
        public string PathOfCardImage { get; set; }
        public string SetNameShort { get; set; }
        public string CardName { get; set; }
        public ulong PHash { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public string Manacost { get; set; }
        public int ConvertedManaCost { get; set; }

        public MagicCard(string pathOfCardImage)
        {
            PathOfCardImage = pathOfCardImage;
        }

        public MagicCard()
        {
        }
    }
}

namespace MTG_Scanner.Models
{
    public class MagicCard
    {
        public string PathOfCardImage { get; set; }
        public string SetNameShort { get; set; }
        public string CardName { get; set; }
        public ulong PHash { get; set; }

        public MagicCard(string pathOfCardImage)
        {
            PathOfCardImage = pathOfCardImage;
        }
    }
}

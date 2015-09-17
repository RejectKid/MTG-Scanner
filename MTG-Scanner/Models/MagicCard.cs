namespace MTG_Scanner.Models
{
    public class MagicCard
    {
        public string PathOfCardImage { get; set; }
        public string Set { get; set; }
        public string Name { get; set; }
        public ulong PHash { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public string Manacost { get; set; }
        public int ConvertedManaCost { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public int Loyalty { get; set; }
        public string Ability { get; set; }
        public string Flavor { get; set; }
        public int Variation { get; set; }
        public string Artist { get; set; }
        public string Number { get; set; }
        public double Rating { get; set; }
        public string Ruling { get; set; }
        public string Color { get; set; }
        public string GeneratedMana { get; set; }
        public int BackId { get; set; }
        public string WaterMark { get; set; }
        public string PrintNumber { get; set; }
        public bool IsOriginal { get; set; }

        public MagicCard(string pathOfCardImage)
        {
            PathOfCardImage = pathOfCardImage;
        }

        public MagicCard()
        {
        }
    }
}

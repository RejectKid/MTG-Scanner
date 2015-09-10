using System.IO;

namespace MTG_Scanner.Models
{
    public class MagicCard
    {
        public string PathOfCardImage { get; set; }
        public string SetNameShort { get; set; }

        public MagicCard(string pathOfCardImage)
        {
            PathOfCardImage = pathOfCardImage;
        }
    }
}

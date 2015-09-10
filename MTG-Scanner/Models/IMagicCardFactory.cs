namespace MTG_Scanner.Models
{
    public interface IMagicCardFactory
    {
        MagicCard CreateMagicCard(string pathOfCardImage);
    }
}

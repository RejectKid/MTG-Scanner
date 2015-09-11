using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MTG_Scanner.Models
{
    class XmlFileLoader
    {
        public readonly List<XDocument> ListOfxmlDatabase = new List<XDocument>();
        public List<MagicCard> ListOfAllMagicCards { get; set; } = new List<MagicCard>();
        private const string XmlDbPath = @"H:\Compy Sci\MTG-Scanner\MTG-Scanner\Resources\Card Db\StandardDB.xml";

        public XmlFileLoader()
        {
            using (var reader = XmlReader.Create(XmlDbPath))
            {
                ListOfxmlDatabase.Add(XDocument.Load(reader));
            }
            GetCards();

        }

        public List<XElement> GetCards()
        {
            var tmpListOfCards = new List<XElement>();
            foreach (var query in ListOfxmlDatabase.Select(doc => doc.Descendants("card").ToList()))
            {
                foreach (var card in query)
                {
                    var tmpCard = new MagicCard
                    {
                        Id = Convert.ToInt32(card.Element("id")?.Value),
                        CardName = card.Element("name")?.Value,
                        SetNameShort = card.Element("set")?.Value,
                        Type = card.Element("type")?.Value,
                        Rarity = card.Element("rarity")?.Value,
                        Manacost = card.Element("manacost")?.Value,
                        ConvertedManaCost = Convert.ToInt32(card.Element("converted_manacost")?.Value),
                        Power = Convert.ToInt32(card.Element("power")?.Value),
                        Toughness = Convert.ToInt32(card.Element("toughness")?.Value),
                        Loyalty = Convert.ToInt32(card.Element("loyalty")?.Value),
                        Ability = Convert.ToInt32(card.Element("ability")?.Value),
                        Flavor = card.Element("flavor")?.Value,
                        Variation = Convert.ToInt32(card.Element("variation")?.Value),
                        Artist = card.Element("artist")?.Value,
                        Number = Convert.ToInt32(card.Element("number")?.Value),
                        Rating = Convert.ToInt32(card.Element("rating")?.Value),
                        Ruling = card.Element("ruling")?.Value,
                        Color = card.Element("color")?.Value,
                        GeneratedMana = Convert.ToInt32(card.Element("generated_mana")?.Value),
                        BackId = Convert.ToInt32(card.Element("back_id")?.Value),
                        WaterMark = card.Element("watermark")?.Value,
                        PrintNumber = card.Element("print_number")?.Value,
                        IsOriginal = Convert.ToBoolean(card.Element("is_original")?.Value)

                    };
                    ListOfAllMagicCards.Add(tmpCard);
                }
            }
            return tmpListOfCards;
        }
    }
}

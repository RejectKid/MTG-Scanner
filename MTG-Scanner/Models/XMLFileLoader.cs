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
            // ReSharper disable LoopCanBePartlyConvertedToQuery
            foreach (var query in ListOfxmlDatabase.Select(doc => doc.Descendants("card").ToList()))
            // ReSharper restore LoopCanBePartlyConvertedToQuery
            {
                foreach (var tmpCard in query.Select(card => new MagicCard
                {
                    Id = Convert.ToInt32(card.Element("id")?.Value),
                    CardName = card.Element("name")?.Value,
                    SetNameShort = card.Element("set")?.Value,
                    Type = card.Element("type")?.Value,
                    Rarity = card.Element("rarity")?.Value,
                    Manacost = card.Element("manacost")?.Value,
                    ConvertedManaCost = Convert.ToInt32(GetNullableElementValue(card, "converted_manacost")),
                    Power = Convert.ToInt32(GetNullableElementValue(card, "power")),
                    Toughness = Convert.ToInt32(GetNullableElementValue(card, "toughness")),
                    Loyalty = Convert.ToInt32(GetNullableElementValue(card, "loyalty")),
                    Ability = card.Element("ability")?.Value,
                    Flavor = card.Element("flavor")?.Value,
                    Variation = Convert.ToInt32(GetNullableElementValue(card, "variation")),
                    Artist = card.Element("artist")?.Value,
                    Number = Convert.ToInt32(GetNullableElementValue(card, "number")),
                    Rating = Convert.ToInt32(GetNullableElementValue(card, "rating")),
                    Ruling = card.Element("ruling")?.Value,
                    Color = card.Element("color")?.Value,
                    GeneratedMana = Convert.ToInt32(GetNullableElementValue(card, "generated_mana")),
                    BackId = Convert.ToInt32(GetNullableElementValue(card, "back_id")),
                    WaterMark = card.Element("watermark")?.Value,
                    PrintNumber = card.Element("print_number")?.Value,
                    IsOriginal = Convert.ToBoolean(GetNullableElementValue(card, "is_original"))
                }))
                {
                    ListOfAllMagicCards.Add(tmpCard);
                }
            }
            return tmpListOfCards;
        }

        private static string GetNullableElementValue(XContainer card, string elementName)
        {
            return card.Element(elementName)?.Value == "" ? null : card.Element(elementName)?.Value;
        }
    }
}

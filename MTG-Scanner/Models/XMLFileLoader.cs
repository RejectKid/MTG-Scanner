using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            var watch = new Stopwatch();
            watch.Start();
            using (var reader = XmlReader.Create(XmlDbPath))
            {
                ListOfxmlDatabase.Add(XDocument.Load(reader));
            }
            GetCards();
            Debug.WriteLine("XmlFileLoader(): " + watch.ElapsedMilliseconds + "ms");
        }

        public List<XElement> GetCards()
        {
            var tmpListOfCards = new List<XElement>();
            // ReSharper disable LoopCanBePartlyConvertedToQuery
            foreach (var query in ListOfxmlDatabase.Select(doc => doc.Descendants("card").ToList()))
            // ReSharper restore LoopCanBePartlyConvertedToQuery
            {

                foreach (var card in query)
                {
                    Task.Run(() =>
                    {
                        var tmpCard = new MagicCard
                        {
                            Id = Convert.ToInt32(card.Element("id")?.Value),
                            CardName = card.Element("name")?.Value,
                            SetNameShort = card.Element("set")?.Value,
                            Type = card.Element("type")?.Value,
                            Rarity = card.Element("rarity")?.Value,
                            Manacost = card.Element("manacost")?.Value,
                            ConvertedManaCost = Convert.ToInt32(GetNullableElementValue(card, "converted_manacost")),
                            Power = card.Element("power")?.Value,
                            Toughness = card.Element("toughness")?.Value,
                            Loyalty = Convert.ToInt32(GetNullableElementValue(card, "loyalty")),
                            Ability = card.Element("ability")?.Value,
                            Flavor = card.Element("flavor")?.Value,
                            Variation = Convert.ToInt32(GetNullableElementValue(card, "variation")),
                            Artist = card.Element("artist")?.Value,
                            Number = card.Element("number")?.Value,
                            Rating = Convert.ToDouble(GetNullableElementValue(card, "rating")),
                            Ruling = card.Element("ruling")?.Value,
                            Color = card.Element("color")?.Value,
                            GeneratedMana = card.Element("generated_mana")?.Value,
                            BackId = Convert.ToInt32(GetNullableElementValue(card, "back_id")),
                            WaterMark = card.Element("watermark")?.Value,
                            PrintNumber = card.Element("print_number")?.Value,
                            IsOriginal = Convert.ToBoolean(GetNullableElementValue(card, "is_original"))
                        };
                        ListOfAllMagicCards.Add(tmpCard);
                    });
                }
            }
            return tmpListOfCards;
        }

        private static string GetNullableElementValue(XContainer card, string elementName)
        {
            var tmpVal = card.Element(elementName)?.Value;
            return tmpVal == "" ? null : tmpVal;
        }
    }
}

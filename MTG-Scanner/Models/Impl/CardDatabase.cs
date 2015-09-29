using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MTG_Scanner.Models.Impl
{
    class CardDatabase : ICardDatabase
    {
        public readonly List<XDocument> ListOfxmlDatabase = new List<XDocument>();
        //public List<MagicCard> ListOfAllMagicCards { get; set; } = new List<MagicCard>();
        public List<MagicCard> ListOfAllMagicCards { get; private set; }

        private const string XmlDbPath = @"..\..\Resources\Card Db\MagicCardDB_Simple.xml";

        public CardDatabase()
        {
            ListOfAllMagicCards = new List<MagicCard>();
            var watch = new Stopwatch();
            watch.Start();
            using (var reader = XmlReader.Create(XmlDbPath))
            {
                ListOfxmlDatabase.Add(XDocument.Load(reader));
            }
            GetCards();
            Debug.WriteLine("CardDatabase(): " + watch.ElapsedMilliseconds + "ms");
        }

        private void GetCards()
        {
            var allcards = ListOfxmlDatabase.Select(doc => doc.Descendants("card").ToList()).SelectMany(query => query);
            foreach (var tmpCard in allcards.Select(card => new MagicCard
            {
                Id = Convert.ToInt32(card.Element("id")?.Value),
                Name = card.Element("name")?.Value,
                Set = card.Element("set")?.Value,
                PHash = Convert.ToUInt64(GetNullableElementValue(card, "phash"))
            }))
            {
                ListOfAllMagicCards.Add(tmpCard);
            }

            //Task.Run(() =>
            //{
            //    var tmpCard = new MagicCard
            //    {
            //        Id = Convert.ToInt32(card.Element("id")?.Value),
            //        Name = card.Element("name")?.Value,
            //        Set = card.Element("set")?.Value,
            //        Type = card.Element("type")?.Value,
            //        Rarity = card.Element("rarity")?.Value,
            //        Manacost = card.Element("manacost")?.Value,
            //        ConvertedManaCost = Convert.ToInt32(GetNullableElementValue(card, "converted_manacost")),
            //        Power = card.Element("power")?.Value,
            //        Toughness = card.Element("toughness")?.Value,
            //        Loyalty = Convert.ToInt32(GetNullableElementValue(card, "loyalty")),
            //        Ability = card.Element("ability")?.Value,
            //        Flavor = card.Element("flavor")?.Value,
            //        Variation = Convert.ToInt32(GetNullableElementValue(card, "variation")),
            //        Artist = card.Element("artist")?.Value,
            //        Number = card.Element("number")?.Value,
            //        Rating = Convert.ToDouble(GetNullableElementValue(card, "rating")),
            //        Ruling = card.Element("ruling")?.Value,
            //        Color = card.Element("color")?.Value,
            //        GeneratedMana = card.Element("generated_mana")?.Value,
            //        BackId = Convert.ToInt32(GetNullableElementValue(card, "back_id")),
            //        WaterMark = card.Element("watermark")?.Value,
            //        PrintNumber = card.Element("print_number")?.Value,
            //        IsOriginal = Convert.ToBoolean(GetNullableElementValue(card, "is_original"))
            //    };
            //    ListOfAllMagicCards.Add(tmpCard);
            //});
        }

        private static string GetNullableElementValue(XContainer card, string elementName)
        {
            var tmpVal = card.Element(elementName)?.Value;
            return tmpVal == "" ? null : tmpVal;
        }
    }
}

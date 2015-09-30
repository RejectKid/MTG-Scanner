using MoreLinq;
using MTG_Scanner.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace MTG_Scanner.Models.Impl
{
    internal class XmlFileCreator : IXmlFileCreator
    {
        private readonly IUtil _util;
        private XmlDocument _xmlfile;
        private XmlElement _rootNode;
        private const string XmlDbPath = @"..\..\Resources\Card Db\MagicCardDB_Simple.xml";

        public XmlFileCreator(IUtil util)
        {
            _util = util;
            InitializeBaseXmlDoc();
        }

        private void InitializeBaseXmlDoc()
        {
            _xmlfile = new XmlDocument();
            _rootNode = _xmlfile.CreateElement("cards");
            _xmlfile.AppendChild(_rootNode);
        }

        public string CreateXmlDb(List<MagicCard> listOfCards)
        {

            foreach (var distinctSet in listOfCards.DistinctBy(o => o.Set))
            {
                var enumerable = listOfCards.Where(op => op.Set == distinctSet.Set).DistinctBy(o => o.Name);
                foreach (var distinctCard in enumerable)
                {
                    var cardElement = _xmlfile.CreateElement("card");
                    AddChildElementAndValue(cardElement, _util.GetVariableName(() => distinctCard.Name), distinctCard.Name);
                    AddChildElementAndValue(cardElement, _util.GetVariableName(() => distinctCard.Set), distinctCard.Set);
                    var phashesElement = _xmlfile.CreateElement("phashes");
                    foreach (var magicCard in listOfCards.Where(o => o.Name == distinctCard.Name && o.Set == distinctSet.Set))
                    {
                        foreach (var phash in magicCard.PHashes)
                        {
                            AddChildElementAndValue(phashesElement, "phash", phash.ToString());
                        }
                    }
                    cardElement.AppendChild(phashesElement);
                    _rootNode.AppendChild(cardElement);
                }
            }

            SaveXmlFile();
            return Path.GetFullPath(XmlDbPath);
        }

        private void AddChildElementAndValue(XmlNode cardElement, string propertyName, string propertyValue)
        {
            var xmlElement = _xmlfile.CreateElement(string.Empty, propertyName.ToLower(), string.Empty);
            var xmlTextValue = _xmlfile.CreateTextNode(propertyValue);
            xmlElement.AppendChild(xmlTextValue);
            cardElement.AppendChild(xmlElement);
        }

        private void SaveXmlFile()
        {
            _xmlfile.Save(XmlDbPath);
        }
    }
}

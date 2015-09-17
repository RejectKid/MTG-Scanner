using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MTG_Scanner.Utils;

namespace MTG_Scanner.Models
{
    internal class XmlFileCreator : IXmlFileCreator
    {
        private readonly IUtil _util;
        private XmlDocument _xmlfile;
        private XmlElement _rootNode;
        private const string _xmlDbPath = @"..\..\Resources\Card Db\MagicCardDB_Simple.xml";

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
            foreach (var magicCard in listOfCards)
            {
                var cardElement = _xmlfile.CreateElement("card");
                AddChildElementAndValue(cardElement, _util.GetVariableName(() => magicCard.Name), magicCard.Name);
                AddChildElementAndValue(cardElement, _util.GetVariableName(() => magicCard.Set), magicCard.Set);
                AddChildElementAndValue(cardElement, _util.GetVariableName(() => magicCard.PHash), magicCard.PHash.ToString());
                _rootNode.AppendChild(cardElement);
            }
            SaveXmlFile();
            return Path.GetFullPath(_xmlDbPath);
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
            _xmlfile.Save(_xmlDbPath);
        }
    }
}

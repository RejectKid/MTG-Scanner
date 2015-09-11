using MahApps.Metro;
using MTG_Scanner.Models;
using MTG_Scanner.Theme;
using MTG_Scanner.Utils;
using MTG_Scanner.Utils.Impl;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Xml;

namespace MTG_Scanner.VMs
{
    public class MainWindowViewModel
    {
        [DllImport(@"\Extern DLLs\pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_dct_imagehash(string file, ref ulong hash);
        [DllImport(@"\Extern DLLs\pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_hamming_distance(ulong hasha, ulong hashb);

        private readonly IUtil _until;
        private XmlFileLoader _xmlFileLoader;
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<MagicCard> ListOfMagicCards { get; set; } = new List<MagicCard>();
        private const string XmlDbPath = @"H:\Compy Sci\MTG-Scanner\MTG-Scanner\Resources\Card Db\StandardDB.xml";

        public MainWindowViewModel()
        {
            GenerateThemeData();
            _until = KernelUtil.Kernel.Get<IUtil>();
            _xmlFileLoader = new XmlFileLoader();
        }

        private void GenerateThemeData()
        {
            // create accent color menu items for the demo
            AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData
                                            {
                                                Name = a.Name,
                                                ColorBrush = a.Resources["AccentColorBrush"] as Brush
                                            })
                                            .ToList();

            // create metro theme color menu items for the demo
            AppThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData
                                           {
                                               Name = a.Name,
                                               BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                                               ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                                           })
                                           .ToList();

        }

        public void ComputePHashes(string selectedPath)
        {
            //read images in resources
            _until.TraverseTree(selectedPath, ListOfMagicCards);
            ulong hash1 = 0;
            //compute hashes
            //foreach (var magicCard in ListOfMagicCards)
            //{
            ph_dct_imagehash(@"H:\Compy Sci\MTG-Scanner\MTG-Scanner\Resources\Card Images\BNG\Acolyte's Reward.full.jpg", ref hash1);
            //}
            //add to XML
            AddHashDataToXML();
        }

        private void AddHashDataToXML()
        {
            using (var reader = new XmlTextReader(XmlDbPath))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.None:
                            break;
                        case XmlNodeType.Element:
                            if (reader.Name == "card")
                                Debug.WriteLine("thing");
                            break;
                        case XmlNodeType.Attribute:
                            break;
                        case XmlNodeType.Text:
                            break;
                        case XmlNodeType.CDATA:
                            break;
                        case XmlNodeType.EntityReference:
                            break;
                        case XmlNodeType.Entity:
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            break;
                        case XmlNodeType.Comment:
                            break;
                        case XmlNodeType.Document:
                            break;
                        case XmlNodeType.DocumentType:
                            break;
                        case XmlNodeType.DocumentFragment:
                            break;
                        case XmlNodeType.Notation:
                            break;
                        case XmlNodeType.Whitespace:
                            break;
                        case XmlNodeType.SignificantWhitespace:
                            break;
                        case XmlNodeType.EndElement:
                            break;
                        case XmlNodeType.EndEntity:
                            break;
                        case XmlNodeType.XmlDeclaration:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}

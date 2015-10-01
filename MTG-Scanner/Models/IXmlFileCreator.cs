using System.Collections.Generic;
using MahApps.Metro.Controls.Dialogs;

namespace MTG_Scanner.Models
{
    public interface IXmlFileCreator
    {
        string CreateXmlDb(List<MagicCard> listOfCards, ProgressDialogController dialogController);
    }
}

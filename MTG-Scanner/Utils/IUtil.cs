using MTG_Scanner.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Windows.Media;

namespace MTG_Scanner.Utils
{
    public interface IUtil
    {
        void TraverseTree(string root, List<MagicCard> listOfMagicCards);
        string GetVariableName<T>(Expression<Func<T>> expression);
        ulong ComputePHash(MagicCard card);
        ImageSource ConvertBitmapInMemory(Image cameraBitmap);
        MagicCard ComparePHash(MagicCard card);
    }
}

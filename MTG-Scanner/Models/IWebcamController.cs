using System.Drawing;

namespace MTG_Scanner.Models
{
    public interface IWebcamController
    {
        void DetectQuads();
        Bitmap CameraBitmap { get; set; }
        Bitmap FilteredBitmap { get; set; }
        Bitmap CardArtBitmap { get; set; }
    }
}

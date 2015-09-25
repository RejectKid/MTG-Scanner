using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MTG_Scanner.Models
{
    class WebcamController : IWebcamController
    {
        public Bitmap CameraBitmap { get; set; } = new Bitmap(800, 600);

        public Bitmap CardArtBitmap { get; set; } = new Bitmap(400, 400);

        public Bitmap CardBitmap { get; set; } = new Bitmap(400, 400);

        public Bitmap FilteredBitmap { get; set; } = new Bitmap(800, 600);

        public List<MagicCard> DetectedMagicCards { get; set; } = new List<MagicCard>();


        public void DetectQuads()
        {
            // Greyscale
            FilteredBitmap = Grayscale.CommonAlgorithms.BT709.Apply(CameraBitmap);

            // edge filter
            var edgeFilter = new SobelEdgeDetector();
            edgeFilter.ApplyInPlace(FilteredBitmap);

            // Threshhold filter
            var threshholdFilter = new Threshold(190);
            threshholdFilter.ApplyInPlace(FilteredBitmap);

            var bitmapData = FilteredBitmap.LockBits(
                new Rectangle(0, 0, FilteredBitmap.Width, FilteredBitmap.Height),
                ImageLockMode.ReadWrite, FilteredBitmap.PixelFormat);


            var blobCounter = new BlobCounter
            {
                FilterBlobs = true,
                MinHeight = 125,
                MinWidth = 125
            };


            blobCounter.ProcessImage(bitmapData);
            var blobs = blobCounter.GetObjectsInformation();
            FilteredBitmap.UnlockBits(bitmapData);

            var shapeChecker = new SimpleShapeChecker();

            var bm = new Bitmap(FilteredBitmap.Width, FilteredBitmap.Height, PixelFormat.Format24bppRgb);

            var g = Graphics.FromImage(bm);
            g.DrawImage(FilteredBitmap, 0, 0);

            var pen = new Pen(Color.Purple, 5);
            var cardPositions = new List<IntPoint>();


            // Loop through detected shapes
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                var edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;
                var sameCard = false;

                //var what = shapeChecker.IsQuadrilateral(edgePoints, out corners);

                // is triangle or quadrilateral
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    // get sub-type
                    var subType = shapeChecker.CheckPolygonSubType(corners);

                    // Only return 4 corner rectanges
                    if ((subType != PolygonSubType.Parallelogram && subType != PolygonSubType.Rectangle) || corners.Count != 4)
                        continue;

                    // Check if its sideways, if so rearrange the corners so it's veritcal
                    RearrangeCorners(corners);

                    // Prevent it from detecting the same card twice
                    foreach (var point in cardPositions)
                    {
                        var distance = corners[0].DistanceTo(point);
                        if (corners[0].DistanceTo(point) < 40)
                            sameCard = true;
                    }

                    if (sameCard)
                        continue;

                    // Hack to prevent it from detecting smaller sections of the card instead of the whole card
                    var area = GetArea(corners);

                    if (area < 20000)// || area > 35000)
                        continue;


                    cardPositions.Add(corners[0]);

                    g.DrawPolygon(pen, ToPointsArray(corners));

                    // Extract the card bitmap
                    //var transformFilter = new QuadrilateralTransformation(corners, 211, 298);
                    var transformFilter = new QuadrilateralTransformation(corners, 225, 325);
                    CardBitmap = transformFilter.Apply(CameraBitmap);

                    var artCorners = new List<IntPoint>
                    {
                        new IntPoint(14, 35),
                        new IntPoint(193, 35),
                        new IntPoint(193, 168),
                        new IntPoint(14, 168)
                    };

                    // Extract the art bitmap
                    var cartArtFilter = new QuadrilateralTransformation(artCorners, 183, 133);
                    CardArtBitmap = cartArtFilter.Apply(CardBitmap);



                    TmpCard = new MagicCard
                    {
                        Corners = corners,
                        CardBitmap = CardBitmap,
                        CardArtBitmap = CardArtBitmap
                    };

                    //DetectedMagicCards.Add(card);
                }
            }

            pen.Dispose();
            g.Dispose();

            FilteredBitmap = bm;
        }

        public MagicCard TmpCard { get; set; }


        private static void RearrangeCorners(IList<IntPoint> corners)
        {
            var pointDistances = new float[4];

            for (var x = 0; x < corners.Count; x++)
            {
                var point = corners[x];

                pointDistances[x] = point.DistanceTo((x == (corners.Count - 1) ? corners[0] : corners[x + 1]));
            }

            var shortestDist = float.MaxValue;
            var shortestSide = int.MaxValue;

            for (var x = 0; x < corners.Count; x++)
            {
                if (!(pointDistances[x] < shortestDist))
                    continue;

                shortestSide = x;
                shortestDist = pointDistances[x];
            }

            if (shortestSide == 0 || shortestSide == 2)
                return;

            var endPoint = corners[0];
            corners.RemoveAt(0);
            corners.Add(endPoint);
        }

        private static double GetArea(IList<IntPoint> vertices)
        {
            if (vertices.Count < 3)
            {
                return 0;
            }
            var area = GetDeterminant(vertices[vertices.Count - 1].X, vertices[vertices.Count - 1].Y, vertices[0].X, vertices[0].Y);
            for (var i = 1; i < vertices.Count; i++)
            {
                area += GetDeterminant(vertices[i - 1].X, vertices[i - 1].Y, vertices[i].X, vertices[i].Y);
            }
            return area / 2;
        }

        private static double GetDeterminant(double x1, double y1, double x2, double y2)
        {
            return x1 * y2 - x2 * y1;
        }

        // Conver list of AForge.NET's points to array of .NET points
        private static System.Drawing.Point[] ToPointsArray(IReadOnlyList<IntPoint> points)
        {
            var array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }


    }
}

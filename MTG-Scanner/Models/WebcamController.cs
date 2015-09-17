namespace MTG_Scanner.Models
{
    class WebcamController
    {

        //private void detectQuads(Bitmap bitmap)
        //{
        //    // Greyscale
        //    filteredBitmap = Grayscale.CommonAlgorithms.BT709.Apply(bitmap);

        //    // edge filter
        //    SobelEdgeDetector edgeFilter = new SobelEdgeDetector();
        //    edgeFilter.ApplyInPlace(filteredBitmap);

        //    // Threshhold filter
        //    Threshold threshholdFilter = new Threshold(190);
        //    threshholdFilter.ApplyInPlace(filteredBitmap);

        //    BitmapData bitmapData = filteredBitmap.LockBits(
        //        new Rectangle(0, 0, filteredBitmap.Width, filteredBitmap.Height),
        //        ImageLockMode.ReadWrite, filteredBitmap.PixelFormat);


        //    BlobCounter blobCounter = new BlobCounter();

        //    blobCounter.FilterBlobs = true;
        //    blobCounter.MinHeight = 125;
        //    blobCounter.MinWidth = 125;

        //    blobCounter.ProcessImage(bitmapData);
        //    Blob[] blobs = blobCounter.GetObjectsInformation();
        //    filteredBitmap.UnlockBits(bitmapData);

        //    SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

        //    Bitmap bm = new Bitmap(filteredBitmap.Width, filteredBitmap.Height, PixelFormat.Format24bppRgb);

        //    Graphics g = Graphics.FromImage(bm);
        //    g.DrawImage(filteredBitmap, 0, 0);

        //    Pen pen = new Pen(Color.Red, 5);
        //    List<IntPoint> cardPositions = new List<IntPoint>();


        //    // Loop through detected shapes
        //    for (int i = 0, n = blobs.Length; i < n; i++)
        //    {
        //        List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
        //        List<IntPoint> corners;
        //        bool sameCard = false;

        //        // is triangle or quadrilateral
        //        if (shapeChecker.IsConvexPolygon(edgePoints, out corners))
        //        {
        //            // get sub-type
        //            PolygonSubType subType = shapeChecker.CheckPolygonSubType(corners);

        //            // Only return 4 corner rectanges
        //            if ((subType == PolygonSubType.Parallelogram || subType == PolygonSubType.Rectangle) && corners.Count == 4)
        //            {
        //                // Check if its sideways, if so rearrange the corners so it's veritcal
        //                rearrangeCorners(corners);

        //                // Prevent it from detecting the same card twice
        //                foreach (IntPoint point in cardPositions)
        //                {
        //                    if (corners[0].DistanceTo(point) < 40)
        //                        sameCard = true;
        //                }

        //                if (sameCard)
        //                    continue;

        //                // Hack to prevent it from detecting smaller sections of the card instead of the whole card
        //                if (GetArea(corners) < 20000)
        //                    continue;

        //                cardPositions.Add(corners[0]);

        //                g.DrawPolygon(pen, ToPointsArray(corners));

        //                // Extract the card bitmap
        //                QuadrilateralTransformation transformFilter = new QuadrilateralTransformation(corners, 211, 298);
        //                cardBitmap = transformFilter.Apply(cameraBitmap);

        //                List<IntPoint> artCorners = new List<IntPoint>();
        //                artCorners.Add(new IntPoint(14, 35));
        //                artCorners.Add(new IntPoint(193, 35));
        //                artCorners.Add(new IntPoint(193, 168));
        //                artCorners.Add(new IntPoint(14, 168));

        //                // Extract the art bitmap
        //                QuadrilateralTransformation cartArtFilter = new QuadrilateralTransformation(artCorners, 183, 133);
        //                cardArtBitmap = cartArtFilter.Apply(cardBitmap);

        //                MagicCard card = new MagicCard();
        //                card.corners = corners;
        //                card.cardBitmap = cardBitmap;
        //                card.cardArtBitmap = cardArtBitmap;

        //                magicCards.Add(card);
        //            }
        //        }
        //    }

        //    pen.Dispose();
        //    g.Dispose();

        //    filteredBitmap = bm;
        //}
    }
}

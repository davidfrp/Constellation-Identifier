using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace StarDetector
{
    public static class StarReader
    {
        // TODO: Kør kun bitmap.GetPixel(x, y).GetBrightness(); én gang, og lav et brightness-map over billedet. Dette kunne være et todimentionelt array.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image">Billedet hvorfra stjernene vil blive fundet.</param>
        /// <param name="brightnessTolerance">Tolerancen for lysstyrken. Alle lystyrker over eller lig med denne værdi tilhører stjerner.</param>
        /// <returns>Returnerer en liste af alle stjerner, der blev fundet.</returns>
        public static Star[] DetectStars(this Image image, double brightnessTolerance)
        {
            Bitmap bitmap = new Bitmap(image);

            // Oprettelse af en tom liste til stjerner.
            List<Star> detectedStars = new List<Star>();

            // Tjek alle pixels i billedet fra øverste venstre hjørne, med læseretningen...
            for (int y = 0; y < image.Height; y++)
            {
                // ...ned til nederst højre hjørne.
                for (int x = 0; x < image.Width; x++)
                {
                    // Tjek hvorvidt om den givne lysstyrke er over eller lig tolerancen.
                    if (bitmap.GetPixel(x, y).GetBrightness() >= brightnessTolerance)
                    {
                        // Som standard er en stjerne endnu ikke fundet, og en ny stjerne vil senere blive oprettet.
                        bool hasStarBeenFound = false;

                        // Tjek alle stjerner og derefter deres tilhørende stjernepixels.
                        for (int i = 0; i < detectedStars.Count; i++)
                        {
                            // Tjek om den givne pixel er isoleret/enkeltstående, ved at tjekke for omkringliggende stjernepixels.
                            if (detectedStars[i].GetStarPixels().Any(starPixel =>
                                (starPixel.X == x - 1 && starPixel.Y == y) ||     // Tjek hvorvidt om den forrige pixel var en stjernepixel.
                                (starPixel.X == x && starPixel.Y == y - 1) ||     // Tjek hvorvidt om pixlen ovenover var en stjernepixel.
                                (starPixel.X == x + 1 && starPixel.Y == y - 1)))  // Tjek hvorvidt om pixlen diagonalt op til højre, var en stjernepixel. 
                            {
                                // Tilføj den givne pixel ved (x, y) til den fundet stjerne.
                                detectedStars[i].AddStarPixel(new Point(x, y));
                                
                                // Sørg for, at der ikke bliver oprettet en ny stjerne.
                                hasStarBeenFound = true;

                                // Sørg for, at en stjernepixel kun kan tilhøre én stjerne, ved at bryde løkken.
                                break;
                            }
                        }

                        // Givet ingen tilhørende stjerne blev fundet (at den givne pixel er isoleret), tilføjes en ny-instansieret stjerne.
                        if (!hasStarBeenFound)
                            detectedStars.Add(new Star(x, y));
                    }
                }
            }

            return detectedStars.ToArray();
        }
    }
}

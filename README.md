# Constellation-Identifier
Example of the implementation of two homebrewed algorithms for star dectection and constellation identification.

## A. Star detection
For a human being, it is quite easy to cast a glance over the night sky, and see some of the many constellations in the endless void of space. Computers, on the other hand, have certain difficulties associated with what to humans seems like a simple task. Even though a computer is capable of reading an image, it still doesn't know what a star is, nor what different constellations look like. In order to solve the first part of this problem, the most basic approach in order for our naked eye to classify a star as a star, must be considered.

The contrast between the dark sky and its bright shining stars is precisely what can help to identify the difference between stars and all non-stars, for both humans and computers. This contrast can, on a computer, be measured by the brightness of the individual pixels in the image. The brightness of a pixel can be calculated in several ways, but this project is based on a calculation from the System.Drawing namespace for the .NET Framework. Here, the brightness is calculated as being equal to the maximum and minimum of its RGB values, both divided by 255 and then added together. The sum of those two numbers is then divided by two. The brightness is obtained as a value between 0 and 1, with 1 corresponding to a completely white pixel, and 0 corresponding to a completely black pixel. Since the pixels, that makes up the clearly visible parts of the stars, are generally of a high brightness, this is used to categorize all pixels above the tolerance **t**, as a star pixel. t can thus be considered as a brightness threshold.

Star pixels are pixels that make up a given star, it is its pixels it is made of. This is due to the fact that the stars will often consist of several bright pixels which are close together in clusters.

![image](https://user-images.githubusercontent.com/6499570/99393468-ab320e80-28dd-11eb-9105-9dc0ef729ddf.png)

*A magnification of three to five stars shows how significantly more than one star pixel can be present in any individual star.*

To group all star pixels into their respective stars, a home-brewed star detection algorithm is used, which although not based on, has later been shown to resemble the algorithms associated in graph theory with [connected-component labeling](https://en.wikipedia.org/wiki/Connected-component_labeling). The art of CCL is being able to distinguish foreground elements from background elements (most often in a binary image), by labeling pixels with a value indicating in which group they belong to.

The star detection algorithm used, does not label pixels with a value, but instead appends them directly to its corresponding star object's list of star pixels. To be able to find out which pixels are star pixels (which if true, will be added to its corresponding star object), the image, for each pixel from the upper left corner, is read left to right and downwards for each row. At each new pixel (with a brightness above or equal to the tolerance **t**), all already found star pixels are checked for whether they have a star pixel: diagonally up to the right, above, or at the previous pixel, to the current star pixel.

![image](https://user-images.githubusercontent.com/6499570/99395311-84c1a280-28e0-11eb-86d8-a678a92bd887.png)

If a star pixel is located in one of the three surrounding pixels being checked, the current star pixel (cyan) belongs to the star object whose star pixel is the first found surrounding the current star pixel.

If no star pixels are found around the current star pixel (ergo the current star pixel is isolated from all other stars), a new star is created with the current star pixel as belonging to the star. For each iteration with a new star, there is thus yet another element in the sequence of stars. Finally, the sequence of the found stars is returned. To achieve the best possible detection of the stars, the tolerance **t** must be set to the brightness of the faintest/darkest star that's still wanted to be detected as a star.

```csharp
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
```

## B. Constellation identification

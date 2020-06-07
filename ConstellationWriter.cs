using System.Drawing;
using System.Collections.Generic;

namespace StarDetector
{
    public static class ConstellationWriter
    {
        /// <summary>
        /// Opretter en konstellation med et givent navn, ud fra et array af stjerner.
        /// </summary>
        /// <param name="stars">Stjernerne som konstellationen skal bestå af.</param>
        /// <param name="name">Navnet på konstellationen.</param>
        /// <returns></returns>
        public static Constellation CreateConstellation(this Star[] stars, string name)
        {
            // Initialiserer et array så der er plads til stjernerepræsentationer for hver stjerne.
            Constellation.StarRepresentation[] starRepresentations = new Constellation.StarRepresentation[stars.Length];
            for (int i = 0; i < stars.Length; i++)
            {
                List<double> angles = new List<double>();
                List<double> distances = new List<double>();
                for (int j = 0; j < stars.Length; j++)
                {
                    if (j == i)
                        continue;

                    for (int k = 0; k < stars.Length; k++)
                    {
                        if (k == j || k == i || j == i)
                            continue;

                        Point p1 = stars[i].CenterPoint;
                        Point p2 = stars[j].CenterPoint;
                        Point p3 = stars[k].CenterPoint;
                        
                        double angle = ConstellationReader.GetAngularSeperation(p1, p2, p3);
                        double distance = ConstellationReader.GetRelativePointSeperation(p1, p2, p3);

                        // Tilføjer den fundne vinkel.
                        angles.Add(angle);

                        // Tilføjer den fundne afstand.
                        distances.Add(distance);
                    }
                }

                // Samler alle fundne vinkler og afstande til en nyinstantieret repræsentation af en stjerne.
                starRepresentations[i] = new Constellation.StarRepresentation(angles.ToArray(), distances.ToArray());
            }

            // Returnerer en nyinstantieret konstellation med navn og stjernerepræsentationer.
            return new Constellation(name, starRepresentations);
        }
    }
}

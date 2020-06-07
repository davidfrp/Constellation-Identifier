using Depictions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarDetector
{
    public static class ConstellationReader
    {
        public static Star[] FindConstellation(this Star[] stars, Constellation c, double t)
        {
            List<Star> starsInConstellation = new List<Star>();

            int i = 0;
            for (int j = 0; j < stars.Length; j++)
            {
                // Vinkler skal tjekkes på ny for nye stjerne.
                int h = 0;

                for (int k = 0; k < stars.Length; k++)
                {
                    // Sørg for ikke at starte næste løkke, hvis vi allerede ved at den skal springes over.
                    if (k == j)
                        continue;

                    for (int l = 0; l < stars.Length; l++)
                    {
                        // Sørg for at de tre stjerne er forskellige.
                        if (l == k || l == j || k == j)
                            continue;

                        if (starsInConstellation.Contains(stars[j]))
                            continue;

                        Point p1 = stars[j].CenterPoint;
                        Point p2 = stars[k].CenterPoint;
                        Point p3 = stars[l].CenterPoint;

                        double measuredAngle = ConstellationReader.GetAngularSeperation(p1, p2, p3);
                        double measuredDistance = ConstellationReader.GetRelativePointSeperation(p1, p2, p3);

                        // Tjek hvorvidt om den fundne vinkel, ligger inden for den accepterede afvigelse.
                        if ((measuredAngle <= c.StarRepresentations[i].Angles[h] + t &&
                            measuredAngle >= c.StarRepresentations[i].Angles[h] - t)) //||
                            //(measuredAngle <= 360 - c.StarRepresentations[i].Angles[h] + t &&
                            //measuredAngle >= 360 - c.StarRepresentations[i].Angles[h] - t))
                        {
                            // Tjek hvorvidt om den fundne afstand, ligger inden for den accepterede afvigelse.
                            if (measuredDistance <= c.StarRepresentations[i].Distances[h] + 0.025 &&
                                measuredDistance >= c.StarRepresentations[i].Distances[h] - 0.025)
                            {
                                // Sørger for at det er en ny vinkel som skal tjekkes efter.
                                h++;

                                k = 0;
                                l = 0;

                                // Tjek om alle vinkler allerede er blevet tjekket igennem.
                                if (h == c.StarRepresentations[i].Angles.Length)
                                {
                                    starsInConstellation.Add(stars[j]);

                                    // Fortsæt til næste stjernerepræsentation.
                                    i++;

                                    // Start med at skulle tjekke nye vinkler for den næste stjernerepræsentation. 
                                    h = 0;
                                    j = 0;

                                    // Tjek om alle stjernerepræsentationer allerede er blevet tjekket igennem.
                                    if (i == c.StarRepresentations.Length)
                                        return starsInConstellation.ToArray();
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
        
        public static double GetAngularSeperation(Point p1, Point p2, Point p3)
        {
            //double input1 = Math.Atan2(p3.Y - p1.Y, p3.X - p1.X);
            //double input2 = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            //double angleInRadians = input1 - input2;
            //double backupVariable = angleInRadians;

            double angleInRadians = Math.Atan2(p3.Y - p1.Y, p3.X - p1.X) -
                                    Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            if (angleInRadians < 0)
                angleInRadians += 2 * Math.PI;

            // Convert radians into the more human-readable, degrees.
            double angleInDegrees = angleInRadians * 180.0 / Math.PI;

            return angleInDegrees;
        }

        public static double GetRelativePointSeperation(Point p1, Point p2, Point p3)
        {
            double distance1 = GetDistance(p1, p2);
            double distance2 = GetDistance(p1, p3);

            return Math.Min(distance1, distance2) / Math.Max(distance1, distance2);
        }

        private static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
    }
}

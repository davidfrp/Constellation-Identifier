using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace StarDetector
{
    public class Star
    {
        List<Point> _starPixels;
        private Point _centerPoint = new Point(-1, -1);

        /// <summary>
        /// Gennemsnittet for placeringen af alle stjernens stjernepixels.
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                if (_centerPoint.X >= 0 &&
                    _centerPoint.Y >= 0)
                    return _centerPoint;
                
                return _centerPoint = new Point
                {
                    X = (int)_starPixels.Average(p => p.X),
                    Y = (int)_starPixels.Average(p => p.Y)
                };
            }
        }
        
        public Star(Point initialStarPixelPosition)
        {
            _starPixels = new List<Point>() { initialStarPixelPosition };
        }

        public Star(int x, int y)
        {
            _starPixels = new List<Point>() { new Point(x, y) };
        }

        public List<Point> GetStarPixels()
        {
            return _starPixels;
        }

        public void AddStarPixel(Point p)
        {
            _starPixels.Add(p);
        }
    }
}

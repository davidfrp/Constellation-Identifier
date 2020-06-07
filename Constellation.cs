namespace StarDetector
{
    /// <summary>
    /// Represents a star constellation.
    /// </summary>
    public class Constellation
    {
        /// <summary>
        /// Represents a star, including all of its angles to all other 
        /// stars in the constellation.
        /// </summary>
        public class StarRepresentation
        {
            /// <summary>
            /// Angles, from this star representation, to all other 
            /// stars in the constellation.
            /// </summary>
            public double[] Angles { get; private set; }

            // TODO: Comment here.
            public double[] Distances { get; private set; }

            public StarRepresentation(double[] angles, double[] distances)
            {
                this.Angles = angles;
                this.Distances = distances;
            }
        }

        /// <summary>
        /// Name of the constellation, preferably human-readable.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// References all stars in the constellation, including 
        /// their corresponding angles.
        /// </summary>
        public StarRepresentation[] StarRepresentations { get; private set; }

        public Constellation(string name, StarRepresentation[] starRepresentations)
        {
            this.Name = name;
            this.StarRepresentations = starRepresentations;
        }
    }
}

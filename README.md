# Constellation-Identifier
Example of the implementation of two homebrewed algorithms for star dectection and constellation identification.

## A. Star detection
For a human being, it is quite easy to cast a glance over the night sky, and see some of the many constellations in the endless void of space. Computers, on the other hand, have certain difficulties associated with what to humans seems like a simple task. Even though a computer is capable of reading an image, it still doesn't know what a star is, nor what different constellations look like. In order to solve the first part of this problem, the most basic approach in order for our naked eye to classify a star as a star, must be considered.

The contrast between the dark sky and its bright shining stars is precisely what can help to identify the difference between stars and all non-stars, for both humans and computers. This contrast can, on a computer, be measured by the brightness of the individual pixels in the image. The brightness of a pixel can be calculated in several ways, but this project is based on a calculation from the System.Drawing namespace for the .NET Framework. Here, the brightness is calculated as being equal to the maximum and minimum of its RGB values, both divided by 255 and then added together. The sum of those two numbers is then divided by two. The brightness is obtained as a value between 0 and 1, with 1 corresponding to a completely white pixel, and 0 corresponding to a completely black pixel. Since the pixels, that makes up the clearly visible parts of the stars, are generally of a high brightness, this is used to categorize all pixels above the tolerance t, as a star pixel. t can thus be considered as a brightness threshold.

## B. Constellation identification

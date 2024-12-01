using System;
using System.Collections.Generic;

namespace AdventOfCode2022
{
    public class Sensor
    {
        private (int x, int y) coord;
        public (int x, int y) Coord
        {
            get { return coord; }
            set { coord = value; }
        }

        private (int x, int y) beacon;
        public (int x, int y) Beacon
        {
            get { return beacon; }
            set { beacon = value; }
        }

        private int halfWidth;
        public int HalfWidth
        {
            get { return halfWidth; }
            set { halfWidth = value; }
        }

        private (int x, int y)[] corners = new (int x, int y)[4];
                
        public Sensor((int x, int y) coord, (int x, int y) beacon)
        {
            Coord = coord;
            Beacon = beacon;

            halfWidth = ManhattanDistance(coord, beacon);

            corners[0] = (coord.x - halfWidth, coord.y); // LEFT
            corners[1] = (coord.x, coord.y - halfWidth); // UP
            corners[2] = (coord.x + halfWidth, coord.y); // RIGHT
            corners[3] = (coord.x, coord.y + halfWidth); // DOWN
        }

        private static int ManhattanDistance((int x, int y) c1, (int x, int y) c2) => Math.Abs(c1.x - c2.x) + Math.Abs(c1.y - c2.y);
    
        public HashSet<int> IntersectionsWithLine(int y)
        {
            HashSet<int> output = new();

            for (int side = 0; side <= 3; side++)
            {
                (int x, int y) c1 = corners[side];
                (int x, int y) c2 = corners[(side + 1)%4];

                if (y >= Math.Min(c1.y, c2.y) && y <= Math.Max(c1.y, c2.y)) output.Add(IntersectionWithSide(y, c1, c2));
            }

            return output;
        }

        private int IntersectionWithSide(int y, (int x, int y) c1, (int x, int y) c2)
        {
            (int x, int y) leftCorner = c1.x < c2.x ? c1 : c2;
            return leftCorner.x + Math.Abs(y - leftCorner.y); // All rhombuses' sides are equals: slopes = 1
        }
    }
}
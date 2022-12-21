using System;

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
                
        public Sensor((int x, int y) coord, (int x, int y) beacon)
        {
            Coord = coord;
            Beacon = beacon;

            halfWidth = ManhattanDistance(coord, beacon);
        }

        private static int ManhattanDistance((int x, int y) c1, (int x, int y) c2) => Math.Abs(c1.x - c2.x) + Math.Abs(c1.y - c2.y);
    
        public bool IsWithinRhombus((int x, int y) coord) => ManhattanDistance(this.coord, coord) <= halfWidth;
    }
}
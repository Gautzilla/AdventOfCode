using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day2
    {
        public static void Solve(int part)
        { 
            int[][] boxes = File.ReadAllLines(@"Day2\input.txt").Select(line => BoxDimensions(line)).ToArray();

            if (part == 1) Console.WriteLine(boxes.Select(box => NeededPaper(box)).Sum());
            else Console.WriteLine(boxes.Select(box => NeededRibbon(box)).Sum());
        }

        private static int NeededPaper(int[] box)
        {
            int[] surfaces = {box[0]*box[1], box[1]*box[2], box[2]*box[0]};
            return surfaces.Sum()*2 + surfaces.Min();
        }

        private static int NeededRibbon(int[] box)
        {
            return box.OrderBy(side => side).Take(2).Sum() * 2 + box.Aggregate((count, side) => side * count);
        }

        private static int[] BoxDimensions(string line) => line.Split('x').Select(dim => int.Parse(dim)).ToArray();
    }
}
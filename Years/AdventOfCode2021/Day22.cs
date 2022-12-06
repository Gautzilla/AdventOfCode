using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class ReactorRebooter
    {
        public (int Lower, int Upper) X { get; }
        public (int Lower, int Upper) Y { get; }
        public (int Lower, int Upper) Z { get; }
        public int isOn { get; }

        public ReactorRebooter(Match rebootInfo)
        {
            isOn = rebootInfo.Groups["rebootState"].ToString() == "on" ? 1 : -1;
            X = (int.Parse(rebootInfo.Groups["xLower"].ToString()), int.Parse(rebootInfo.Groups["xUpper"].ToString()));
            Y = (int.Parse(rebootInfo.Groups["yLower"].ToString()), int.Parse(rebootInfo.Groups["yUpper"].ToString()));
            Z = (int.Parse(rebootInfo.Groups["zLower"].ToString()), int.Parse(rebootInfo.Groups["zUpper"].ToString()));
        }

        public ReactorRebooter((int isOn, (int Lower, int Upper) X, (int Lower, int Upper) Y, (int Lower, int Upper) Z) rebooter)
        {
            isOn = rebooter.isOn;
            X = rebooter.X;
            Y = rebooter.Y;
            Z = rebooter.Z;
        }
    }
    class Day22
    {
        static public void Solve(int part)
        {
            //string path = @"..\..\Inputs\day22Example.txt";
            string path = @"..\..\Inputs\day22.txt";

            List<string> input = File.ReadAllLines(path).ToList();

            List<ReactorRebooter> rebooters = new List<ReactorRebooter>();

            foreach (string reactorInstruction in input)
            {
                rebooters.Add(ExtractReactorInstruction(reactorInstruction));
            }

           if (part == 1) Console.WriteLine($"{InitializeReactors(rebooters)} are on after initialization procedure.");
           else Console.WriteLine($"{RebootReactors(rebooters)} are on after reboot procedure.");

        }

        static private ReactorRebooter ExtractReactorInstruction(string reactorInstruction)
        {
            Regex instructionRegex = new Regex(@"(?<rebootState>^\w+)\sx=(?<xLower>\W*\d+)..(?<xUpper>\W*\d+),y=(?<yLower>\W*\d+)..(?<yUpper>\W*\d+),z=(?<zLower>\W*\d+)..(?<zUpper>\W*\d+)");

            return new ReactorRebooter(instructionRegex.Match(reactorInstruction));
        }

        static private int InitializeReactors (List<ReactorRebooter> rebooters)
        {
            bool[,,] map = new bool[101, 101, 101];

            int onReactors = 0;

            foreach (ReactorRebooter rebooter in rebooters)
            {
                for (int z = Math.Max(0, rebooter.Z.Lower + 50); z <= Math.Min(100, rebooter.Z.Upper + 50); z++)
                {
                    for (int y = Math.Max(0, rebooter.Y.Lower + 50); y <= Math.Min(100, rebooter.Y.Upper + 50); y++)
                    {
                        for (int x = Math.Max(0, rebooter.X.Lower + 50); x <= Math.Min(100, rebooter.X.Upper + 50); x++)
                        {
                            if (rebooter.isOn == 1 && !map[x, y, z] || rebooter.isOn == -1 && map[x, y, z]) onReactors += rebooter.isOn;

                            map[x, y, z] = rebooter.isOn == 1;
                        }
                    }
                }
            }

            return onReactors;
        }

        static private long RebootReactors(List<ReactorRebooter> rebooters)
        {
            List<ReactorRebooter> boxes = new List<ReactorRebooter>(); // Intersection of 2 cuboids is a cuboid
            List <ReactorRebooter> boxesToAdd = new List<ReactorRebooter>();

            foreach (ReactorRebooter rebooter in rebooters)
            {

                if (rebooter.isOn == 1) boxesToAdd.Add(rebooter); 

                foreach (ReactorRebooter box in boxes)
                {
                    ReactorRebooter overlap = Overlap(rebooter, box); 
                    if (overlap != null) boxesToAdd.Add(overlap);
                }
                boxes.AddRange(boxesToAdd);
                boxesToAdd = new List<ReactorRebooter>();
            }

            return CountReactors(boxes);
        }

        static private ReactorRebooter Overlap(ReactorRebooter rebooterA, ReactorRebooter rebooterB)
        {
            (int Lower, int Upper) xInter = (Math.Max(rebooterA.X.Lower, rebooterB.X.Lower), Math.Min(rebooterA.X.Upper, rebooterB.X.Upper));
            (int Lower, int Upper) yInter = (Math.Max(rebooterA.Y.Lower, rebooterB.Y.Lower), Math.Min(rebooterA.Y.Upper, rebooterB.Y.Upper));
            (int Lower, int Upper) zInter = (Math.Max(rebooterA.Z.Lower, rebooterB.Z.Lower), Math.Min(rebooterA.Z.Upper, rebooterB.Z.Upper));

            if (xInter.Lower > xInter.Upper || yInter.Lower > yInter.Upper || zInter.Lower > zInter.Upper) return null;
            else return new ReactorRebooter((-rebooterB.isOn, xInter, yInter, zInter)); // If rebooter activates reactors : intersection counts as desactivate, and vice versa
        }

        static private long CountReactors(List<ReactorRebooter> rebooters)
        {
            long reactors = 0;
            foreach (ReactorRebooter rebooter in rebooters)
            {
                reactors += (long)rebooter.isOn * (rebooter.X.Upper - rebooter.X.Lower + 1) * (rebooter.Y.Upper - rebooter.Y.Lower + 1) * (rebooter.Z.Upper - rebooter.Z.Lower + 1);
            }

            return reactors;
        }
    }
}


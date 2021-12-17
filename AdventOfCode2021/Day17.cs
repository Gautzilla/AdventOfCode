using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day17
    {
        static public void Solve(int part)
        {
            //string filePath = @"..\..\Inputs\day17Example.txt";
            string filePath = @"..\..\Inputs\day17.txt";

            string input = File.ReadAllText(filePath);

            string xCoord = new string(input.SkipWhile(c => c != 'x').Skip(2).TakeWhile(c => c != ',').ToArray());
            int xMin = int.Parse(xCoord.Split('.').Where(s => s != "").First());
            int xMax = int.Parse(xCoord.Split('.').Where(s => s != "").Last());

            string yCoord = new string(input.SkipWhile(c => c != 'y').Skip(2).ToArray());
            int yMin = int.Parse(yCoord.Split('.').Where(s => s != "").First());
            int yMax = int.Parse(yCoord.Split('.').Where(s => s != "").Last());

            int minXVel = (int)Math.Round((-1 + Math.Sqrt(1 + 8 * xMin)) / 2); // max X distance = XVelocity * (XVelocity + 1)/2 : must be >= xMin
            int maxXVel = (int)(-1 + Math.Sqrt(1 + 8 * xMax)) / 2;  // For reaching highest Y position
            
            List<int[]> initVelsThatHit = new List<int[]>();
            List<int> highestYPos = new List<int>();

            if (part == 2) maxXVel = xMax; // Probe shot directly to the last column of the target after 1st step

            for (int initXVel = minXVel; initXVel <= maxXVel; initXVel++)
            {
                int initYVel = yMin; // Probe shot directly to the bottom of the target after 1st step

                while (true) // yVel
                {
                    int n = 0;
                    int xVel = initXVel;
                    int yVel = initYVel;
                    bool insideTarget = false;

                    int xPos = 0;
                    int yPos = 0;

                    bool targetFound = false;

                    int highestY = 0;

                    while (true) // steps n
                    {
                        xPos += xVel;
                        xVel -= xVel > 0 ? 1 : 0;
                        yPos = (n + 1) * yVel - n * (n + 1) / 2; 

                        highestY = Math.Max(yPos, highestY);

                        if (xPos >= xMin && xPos <= xMax)
                        {
                            insideTarget = yPos >= yMin && yPos <= yMax;

                            if (insideTarget)
                            {
                                if (!targetFound)
                                {
                                    initVelsThatHit.Add(new int[] { initXVel, initYVel });
                                    highestYPos.Add(highestY);
                                }
                                targetFound = true;
                            }
                        } if (yPos < yMin) break; // fallen too low
                        n++;
                    }
                    if (initYVel++ > 200) break;
                    }
            }

            int[] maxYVelInitVels = initVelsThatHit.First(v => v[1] == initVelsThatHit.Max(y => y[1]));

            Console.WriteLine($"Highest probe that hits the target: ({maxYVelInitVels[0]}, {maxYVelInitVels[1]}). Max Y reached : {highestYPos.Max(a => a)}. Count of probes that hit the target : {initVelsThatHit.Count()}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day19
    {
        static public void Solve(int part)
        {
            //string filePath = @"..\..\Inputs\day19Example.txt";
            string filePath = @"..\..\Inputs\day19.txt";

            string[] input = File.ReadAllLines(filePath).Where(l => l != "").ToArray();

            List<List<int[]>> scanners = ParseScanners(input);

            List<Tuple<int, int, int, int[]>> arrangeScannersInstructions = new List<Tuple<int, int, int, int[]>>();

            List<int> pairedScanners = new List<int>();

            pairedScanners.Add(0);

            Stack<int> scannersToPair = new Stack<int>();
            scannersToPair.Push(0);

            while (scannersToPair.Count() > 0)
            {
                int i = scannersToPair.Pop();

                for (int j = 0; j < scanners.Count(); j ++)
                {
                    if (i == j || pairedScanners.Contains(j)) continue;

                    Tuple<int, int, int[]> commonBeacons = PairScanners(scanners[i], scanners[j]); // Nb of common beacons, rotation from 2 -> 1, translation from 2 -> 1

                    if (commonBeacons.Item1 > 11)
                    {
                        arrangeScannersInstructions.Add(new Tuple<int, int, int, int[]>(i, j, commonBeacons.Item2, commonBeacons.Item3));
                        pairedScanners.Add(j);
                        scannersToPair.Push(j);
                    }
                }                
            }

            if (part == 1)
            {
                List<int[]> beacons = SortBeacons(scanners, arrangeScannersInstructions);
                Console.WriteLine($"There are {beacons.Count()} beacons.");
            } else
            {
                List<int[]> scannerPositions = ScannersDistance(scanners, arrangeScannersInstructions);
                int maxDistance = MaximumDistance(scannerPositions);
                Console.WriteLine($"The maximum Manhattan Distance between scanners is {maxDistance}");
            }
        }

        static private List<List<int[]>> ParseScanners(string[] input)
        {
            List<List<int[]>> scanners = new List<List<int[]>>();
            int activeScanner = -1;

            foreach (string line in input)
            {
                if (line.Contains("scanner"))
                {
                    activeScanner++;
                    scanners.Add(new List<int[]>());
                    continue;
                } else
                {
                    scanners[activeScanner].Add(line.Split(',').Select(n => int.Parse(n)).ToArray());
                }
            }

            return scanners;
        }

        static private Tuple<int, int, int[]> PairScanners(List<int[]> scanner1, List<int[]> scanner2)
        {
            List<int[]> rotatedScanner2;
            List<int[]> translatedScanner2;

            int commonBeacons = 0;

            for (int rotation = 0; rotation < 24; rotation++)
            {
                rotatedScanner2 = RotateScanner(scanner2, rotation);

                for (int targetBeacon = 0; targetBeacon < scanner1.Count(); targetBeacon++)
                {
                    for (int translationBeacon = 0; translationBeacon < rotatedScanner2.Count(); translationBeacon++)
                    {
                        int[] translation = { scanner1[targetBeacon][0] - rotatedScanner2[translationBeacon][0], scanner1[targetBeacon][1] - rotatedScanner2[translationBeacon][1], scanner1[targetBeacon][2] - rotatedScanner2[translationBeacon][2] };
                        translatedScanner2 = TranslateScanner(rotatedScanner2, translation);

                        commonBeacons = CommonBeacons(scanner1, translatedScanner2);

                        if (commonBeacons > 11) return new Tuple<int, int, int[]>(commonBeacons, rotation, translation);
                    }
                }
            }

            return new Tuple<int, int, int[]>(commonBeacons, 0, new int[] { 0 });
        }

        static private List<int[]> RotateScanner (List<int[]> scanner, int rotation)
        {
            double ang = (rotation%4) * Math.PI / 2;
            List<int[]> newScannerCoord = new List<int[]>();

            foreach (int[] beacon in scanner)
            {
                int x = 0;
                int y = 0;
                int z = 0;
                int[] newBeacon = new int[3];

                switch (rotation % 6)
                {
                    case 0:
                        x = beacon[0];
                        y = beacon[1];
                        z = beacon[2];
                    break;
                    case 1:
                        x = beacon[1];
                        y = -beacon[0];
                        z = beacon[2];
                        break;
                    case 2:
                        x = -beacon[0];
                        y = -beacon[1];
                        z = beacon[2];
                        break;
                    case 3:
                        x = -beacon[1];
                        y = beacon[0];
                        z = beacon[2];
                        break;
                    case 4:
                        x = beacon[2];
                        y = beacon[1];
                        z = -beacon[0];
                        break;
                    case 5:
                        x = -beacon[2];
                        y = beacon[1];
                        z = beacon[0];
                        break;
                    default:
                        break;
                }

                newBeacon = new int[] { x, y, z};

                switch (rotation / 6 % 4)
                {
                    case 0:
                        x = newBeacon[0];
                        y = newBeacon[1];
                        z = newBeacon[2];
                        break;
                    case 1:
                        x = newBeacon[0];
                        y = newBeacon[2];
                        z = -newBeacon[1];
                        break;
                    case 2:
                        x = newBeacon[0];
                        y = -newBeacon[1];
                        z = -newBeacon[2];
                        break;
                    case 3:
                        x = newBeacon[0];
                        y = -newBeacon[2];
                        z = newBeacon[1];
                        break;
                    default:
                        break;
                }
                newScannerCoord.Add(new int[] { x, y, z });
            }
            return newScannerCoord;
        }

        static private List<int[]> TranslateScanner(List<int[]> scanner, int[] translation)
        {
            scanner = scanner.Select(beacon => beacon.Select((coord, c) => coord += translation[c]).ToArray()).ToList();

            return scanner;
        }

        static private int CommonBeacons(List<int[]> scanner1, List<int[]> scanner2)
        {
            return scanner1.Where(beacon1 => scanner2.Any(beacon2 => beacon1[0] == beacon2[0] && beacon1[1] == beacon2[1] && beacon1[2] == beacon2[2])).Count();
        }

        static private void DisplayScanner(List<int[]> scanner, string name)
        {
            Console.WriteLine($"Scanner {name}: ");
            foreach (int[] beacon in scanner) Console.WriteLine($"{beacon[0]},{beacon[1]},{beacon[2]}");
            Console.WriteLine("\n");
        }

        static private List<int[]> SortBeacons (List<List<int[]>> scanners, List<Tuple<int, int, int, int[]>> instructions)
        {
            Dictionary<int, List<int[]>> movedScanners = new Dictionary<int, List<int[]>>();

            for (int i = 0; i < scanners.Count(); i++) movedScanners.Add(i, scanners[i]);

            for (int i = instructions.Count() - 1; i >= 0; i--)
            {
                int destinationScanner = instructions[i].Item1;
                int movingScanner = instructions[i].Item2;
                int rotation = instructions[i].Item3;
                int[] translation = instructions[i].Item4;

                movedScanners[movingScanner] = TranslateScanner(RotateScanner(movedScanners[movingScanner], rotation), translation); // Aligns moving on destination
                foreach (int[] beacon in movedScanners[movingScanner].Where(beacon1 => !movedScanners[destinationScanner].Any(beacon2 => beacon1[0] == beacon2[0] && beacon1[1] == beacon2[1] && beacon1[2] == beacon2[2]))) movedScanners[destinationScanner].Add(beacon); // Adds beacons from moving to destination
            }
            return movedScanners[0];
        }

        static private List<int[]> ScannersDistance(List<List<int[]>> scanners, List<Tuple<int, int, int, int[]>> instructions)
        {
            Dictionary<int,int[]> scannerPositions = new Dictionary<int, int[]>();
            Dictionary<int, List<int>> pairedScanners = new Dictionary<int, List<int>>();

            for (int i = 0; i < scanners.Count(); i++ ) pairedScanners.Add(i, new List<int>() { i });

            for (int i = instructions.Count() - 1; i >= 0; i--)
            {
                foreach (int scanner in pairedScanners[instructions[i].Item2])
                {
                    if (scannerPositions.ContainsKey(scanner))
                    {
                        scannerPositions[scanner] = RotateScanner(new List<int[]> { scannerPositions[scanner] }, instructions[i].Item3).First();
                        scannerPositions[scanner] = TranslateScanner(new List<int[]> { scannerPositions[scanner] }, instructions[i].Item4).First();
                    }
                    else
                    {
                        scannerPositions.Add(scanner, instructions[i].Item4);
                    }
                    pairedScanners[instructions[i].Item1].Add(scanner);
                }
            }
            return (scannerPositions.Values.ToList());
        }

        static private int MaximumDistance (List<int[]> positions)
        {
            int maxDistance = int.MinValue;

            for (int i = 0; i < positions.Count(); i++)
            {
                for (int j = i; j < positions.Count(); j++)
                {
                    int distance = 0;
                    for (int k = 0; k < 3; k++) distance += Math.Abs(positions[j][k] - positions[i][k]);
                    maxDistance = Math.Max(maxDistance, distance);
                }
            }
            return maxDistance;
        }
    }
}

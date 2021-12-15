using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day15
    {
        private class Node
        {
            public int x { get; }
            public int y { get; }
            public int risk { get; }
            public int cumulateRisk { get; set; }
            public Node(int[] coord, int risk)
            {
                x = coord[0];
                y = coord[1];
                this.risk = risk > 9 ? risk % 9 : risk;
                this.cumulateRisk =  x == 0 && y == 0 ? 0 : int.MaxValue;
            }
        }

        public static void Solve(int part)
        {
            string filePath = @"..\..\Inputs\day15Example.txt";
            //string filePath = @"..\..\Inputs\day15.txt";

            List<string> input = File.ReadAllLines(filePath).ToArray().ToList();

            List<Node> cavern = new List<Node>();

            for (int y = 0; y < input.Count(); y++) for (int x = 0; x < input[0].Length; x++) cavern.Add(new Node(new int[] { x, y }, int.Parse(input[y][x].ToString())));

            cavern = RepeatCavern(cavern, 4);

            int shortestPath = ShortestPath(cavern, new int[] { 0, 0 }, new int[] { cavern.Max(n => n.x), cavern.Max(n => n.y)});


            Console.WriteLine(shortestPath);
        }

        private static void VisualizeCavern (List<Node> cavern, bool risk)
        {
            for (int y = 0; y <= cavern.Max(n => n.y); y++)
            {
                for (int x = 0; x <= cavern.Max(n => n.x); x++)
                {
                    if (risk) Console.Write($"{cavern.First(n => n.x == x && n.y == y).risk}");
                    else Console.Write($"{cavern.First(n => n.x == x && n.y == y).cumulateRisk} ");
                }
                Console.Write("\n");
            }
        }

        private static int ShortestPath(List<Node> cavern, int[] startCoord, int[] endCoord)
        {
            List<Node> nodesToEvaluate = new List<Node>();
            List<Node> evaluatedNodes = new List<Node>();

            nodesToEvaluate.Add(cavern.First(n => n.x == startCoord[0] && n.y == startCoord[1]));

            while (nodesToEvaluate.Count() > 0)
            {
                Node activeNode = nodesToEvaluate.First();
                evaluatedNodes.Add(activeNode);

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if ( (Math.Abs(i) + Math.Abs(j) == 1) && cavern.Any(n => n.x == activeNode.x +i && n.y == activeNode.y + j))
                        {
                            Node nodeToEvaluate = cavern.First(n => n.x == activeNode.x + i && n.y == activeNode.y + j);

                            if (!evaluatedNodes.Contains(nodeToEvaluate) && !nodesToEvaluate.Contains(nodeToEvaluate)) nodesToEvaluate.Add(nodeToEvaluate);

                            if (activeNode.cumulateRisk + nodeToEvaluate.risk < nodeToEvaluate.cumulateRisk) nodeToEvaluate.cumulateRisk = activeNode.cumulateRisk + nodeToEvaluate.risk ;
                        }
                    }
                }

                nodesToEvaluate.RemoveAll(n => n == activeNode);
            }

            return cavern.First(n => n.x == endCoord[0] && n.y == endCoord[1]).cumulateRisk;
        }

        private static List<Node> RepeatCavern (List<Node> cavern, int repeatTimes)
        {
            List<Node> newCavern = new List<Node>(cavern);

            for (int x = 0; x <= repeatTimes; x++)
            {

                for (int y = 0; y <= repeatTimes; y++)
                {
                    foreach (Node node in cavern)
                    {
                        newCavern.Add(new Node(new int[] { node.x + (cavern.Max(n => n.x) + 1) * x, node.y + (cavern.Max(n => n.y) + 1) * y }, node.risk + x + y));
                    }
                }
            }

            return newCavern;
        }
    }
}

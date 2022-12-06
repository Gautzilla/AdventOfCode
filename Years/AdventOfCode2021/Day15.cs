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
            public bool visited { get; set; }
            public Node(int[] coord, int risk)
            {
                x = coord[0];
                y = coord[1];
                this.risk = risk > 9 ? risk % 9 : risk;
                this.cumulateRisk =  x == 0 && y == 0 ? 0 : int.MaxValue;
                visited = x == 0 && y == 0;
            }
        }

        public static void Solve(int part)
        {
            //string filePath = @"..\..\Inputs\day15Example.txt";
            string filePath = @"..\..\Inputs\day15.txt";

            List<string> input = File.ReadAllLines(filePath).ToArray().ToList();

            Node[,] cavern = new Node[input[0].Length, input.Count()];

            for (int y = 0; y < input.Count(); y++) for (int x = 0; x < input[0].Length; x++) cavern[x,y] = new Node(new int[] { x, y }, int.Parse(input[y][x].ToString()));

            int repeat = part == 1 ? 1 : 5;

            cavern = RepeatCavern(cavern, repeat);

            int shortestPath = ShortestPath(cavern, new int[] { 0, 0 }, new int[] { cavern.GetLength(0) - 1, cavern.GetLength(1) - 1 });

            Console.WriteLine(shortestPath);
        }

        private static void VisualizeCavern (Node[,] cavern, bool risk)
        {
            for (int y = 0; y < cavern.GetLength(1); y++)
            {
                for (int x = 0; x < cavern.GetLength(0); x++)
                {
                    if (risk) Console.Write($"{cavern[x,y].risk}");
                    else Console.Write($"[{cavern[x,y].cumulateRisk}] ");
                }
                Console.Write("\n");
            }
        }

        private static int ShortestPath(Node[,] cavern, int[] startCoord, int[] endCoord)
        {
            List<Node> nodesToEvaluate = new List<Node>();

            nodesToEvaluate.Add(cavern[startCoord[0], startCoord[1]]);

            while (nodesToEvaluate.Count() > 0)
            {
                Node activeNode = nodesToEvaluate.First();

                foreach (Node neighbour in GetNeighbours(cavern, activeNode))
                {
                    if (!neighbour.visited)
                    {
                        nodesToEvaluate.Add(neighbour);
                        neighbour.visited = true;
                    }

                    if (activeNode.cumulateRisk + neighbour.risk < neighbour.cumulateRisk)
                    {
                        neighbour.cumulateRisk = activeNode.cumulateRisk + neighbour.risk;

                        foreach (Node newNeighbour in GetNeighbours(cavern, neighbour)) if (newNeighbour != activeNode) newNeighbour.visited = false; // Ameliorations can be made here to gain efficiency
                    }
                }
                nodesToEvaluate.RemoveAt(0);
            }
            return cavern[endCoord[0],endCoord[1]].cumulateRisk;
        }

        private static Node[,] RepeatCavern (Node[,] cavern, int repeatTimes)
        {
            Node[,] newCavern = new Node[ cavern.GetLength(0) * repeatTimes , cavern.GetLength(1) * repeatTimes];

            for (int x = 0; x < repeatTimes; x++)
            {
                for (int y = 0; y < repeatTimes; y++)
                {
                    foreach (Node node in cavern)
                    {
                        newCavern[node.x + cavern.GetLength(0) * x , node.y + cavern.GetLength(1) * y] = new Node(new int[] { node.x + cavern.GetLength(0) * x, node.y + cavern.GetLength(1) * y }, node.risk + x + y);
                    }
                }
            }

            return newCavern;
        }

        private static List<Node> GetNeighbours(Node[,] cavern, Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((Math.Abs(i) + Math.Abs(j) == 1) && node.x + i >= 0 && node.x + i < cavern.GetLength(0) && node.y + j >= 0 && node.y + j < cavern.GetLength(1))
                    {
                        neighbours.Add(cavern[node.x + i , node.y + j]);
                    }
                }
            }
            return neighbours;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public class Node
    {
        private char _height;
        private HashSet<Node> _path;
        private (int x, int y) _coords;
        private HashSet<Node> _neighbours;
        public HashSet<Node> Neighbours
        {
            get { return _neighbours; }
            set { _neighbours = value; }
        }        

        public Node(char height, (int, int) coords)
        {
            _height = height;
            _coords = coords;
            _path = new();
            _neighbours = new();
        }
        public char Height
        {
            get { return _height; }
            set { _height = value; }
        }        
        public HashSet<Node> Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public (int x, int y) Coords
        {
            get { return _coords; }
            set { _coords = value; }
        }
        
        public void AddNeighbour(Node node)
        {
            if (_height == 'E')
            {
                if (node.Height >= 'y') _neighbours.Add(node);
            }
            else if (node.Height == 'S')
            {
                if (_height <= 'b') _neighbours.Add(node);
            }
            else
            {
                if (node.Height >= _height - 1) _neighbours.Add(node);
            }
        }
    }

    public static class Day12
    {
        private static Node[,] _map = new Node[0,0];
        private static (int x, int y) _end = (0,0);
        private static readonly (int x, int y)[] _directions = {(-1,0), (0,-1), (1,0), (0,1)};
        private static Queue<Node> _toVisit = new();
        private static HashSet<Node> _visited = new();
        private static int _shortestPath = int.MaxValue;
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day12\input.txt");
            _map = new Node[input.First().Length, input.Length];

            ParseInput(input);

            _toVisit.Enqueue(_map[_end.x, _end.y]);

            while (_shortestPath == int.MaxValue && _toVisit.Count > 0) BFS(part);

            Console.WriteLine(_shortestPath);
        }

        private static void ParseInput(string[] input)
        {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    _map[x,y] = new Node(input[y][x], (x,y));
                    if (input[y][x] == 'E') _end = (x,y);
                }
            }

            for (int y = 0; y < _map.GetLength(1); y++)
            {
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    foreach ((int x, int y) neighbour in _directions.Select(d => (d.x + x, d.y + y)).Where(n => IsInsideMap(n)))
                    {
                        _map[x,y].AddNeighbour(_map[neighbour.x, neighbour.y]);
                    }
                }
            }
        }

        private static bool IsInsideMap((int x, int y) coord) => coord.x >= 0 && coord.y >= 0 && coord.x < _map.GetLength(0) && coord.y < _map.GetLength(1);
    
        private static void BFS(int part)
        {
            Node actual = _toVisit.Dequeue();

            if (actual.Height == (part == 1 ? 'S' : 'a'))  _shortestPath = actual.Path.Count;         

            _visited.Add(actual);

            foreach (Node neighbour in actual.Neighbours.Where(n => !_visited.Contains(n) && !_toVisit.Contains(n)))
            {
                _toVisit.Enqueue(neighbour);
                neighbour.Path = new(actual.Path);
                neighbour.Path.Add(actual);
            }
        }
    }
}
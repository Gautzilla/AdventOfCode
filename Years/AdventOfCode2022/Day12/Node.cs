using System;
using System.Collections.Generic;

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
}

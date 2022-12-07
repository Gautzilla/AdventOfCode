using System;
using System.Linq;
using System.IO;

namespace AdventOfCode2022
{
    public static class Day7
    {
        private static Folder _root = new (@"/");
        private static Folder _head = _root;
        private static bool _isListingContent = false;
        private static readonly int _totalSpace = 70000000;
        private static readonly int _neededSpace = 30000000;
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day7\input.txt");

            foreach (string instruction in input)
            {
                ManageInstruction(instruction);
            }

            if (part == 1) 
            Console.WriteLine(_root.AllChildrenFolders().Where(cF => cF.TotalSize() < 100000).Sum(cF => cF.TotalSize()));
            else
            {
                int usedSpace = _root.TotalSize();
                Console.WriteLine(_root.AllChildrenFolders().OrderBy(cF => cF.TotalSize()).First(cF => _totalSpace - usedSpace + cF.TotalSize() >= _neededSpace).TotalSize());
            }
        }

        private static void ManageInstruction(string instruction)
        {
            string[] parts = instruction.Split(" ").ToArray();

            if (parts[0] == "$")
            {
                _isListingContent = false;

                if (parts[1] == "cd")
                {
                    ChangeDirectory(parts[2]);
                    return;
                }

                if (parts[1] == "ls")
                {
                    _isListingContent = true;
                    return;
                }

            } else if (_isListingContent)
            {
                _head.AddContent(parts);
            }
        }

        private static void ChangeDirectory(string directory)
        {
            if (directory == "..")
            {
                _head = _head.ParentFolder ?? _root;
                return;
            }

            if (directory == "/")
            {
                _head = _root;
                return;
            }
            _head = _head.ChildrenFolders.Single(folder => folder.Name == directory);
        }
    }
}
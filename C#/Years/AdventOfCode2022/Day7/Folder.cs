using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2022
{
    public class Folder
    {
        private HashSet<Folder> _childrenFolders;
        private HashSet<DataFile> _files;        
        private Folder? _parentFolder;
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }        

        public HashSet<Folder> ChildrenFolders
        {
            get { return _childrenFolders; }
            set { _childrenFolders = value; }
        }        
        
        public Folder? ParentFolder
        {
            get { return _parentFolder; }
            set { _parentFolder = value; }
        }

        public HashSet<DataFile> Files
        {
            get { return _files; }
            set { _files = value; }
        }
        
        public Folder(string name, Folder? parentFolder)
        {
            _parentFolder = parentFolder;
            _name = name;
            _childrenFolders = new();
            _files = new();
        }

        public Folder(string name)
        {
            _name = name;
            _childrenFolders = new();
            _files = new();
        }

        public void AddContent (string[] content)
        {
            if (int.TryParse(content[0], out int size))
            {
                this.Files.Add(new DataFile(size, content[1]));
                return;
            }
            this._childrenFolders.Add(new Folder(content[1], this));
        }

        public int TotalSize()
        {
            return _files.Sum(f => f.Size) + _childrenFolders.Sum(f => f.TotalSize());
        }

        public List<Folder> AllChildrenFolders ()
        {
            if (_childrenFolders.Count == 0) return new List<Folder>() {this};
            else return _childrenFolders.SelectMany(cF => cF.AllChildrenFolders()).Append(this).ToList();
        }
    }
}

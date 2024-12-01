using System;

namespace AdventOfCode2022
{
    public class DataFile
    {
        private int _size;   
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public DataFile(int size, string name)
        {
            _size = size;
            _name = name;
        }        
    }
}
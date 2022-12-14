using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2022
{
    public class Packet
    {
        private List<Packet> content;
        public List<Packet> Content
        {
            get { return content; }
            set { content = value; }
        }

        private int value;
        public int Value
        {
            get { return value; }
            set { this.value = value; isNumber = true; }
        }

        private bool isNumber;
        public bool IsNumber
        {
            get { return isNumber; }
            set { isNumber = value; }
        }
        
        private Packet? parent;
        public Packet? Parent
        {
            get { return parent; }
            set { parent = value; }
        }         
        
        public Packet(string packet, Packet? Parent)
        {
            content = new();
            isNumber = false;

            if (int.TryParse(packet, out int value)) this.Value = value;
            else ParseContent(packet);
        }

        private void ParseContent(string packet)
        {
            int innerIndex = 0;
            string part = string.Empty;

            foreach (char c in packet.Skip(1).Take(packet.Length-2)) // Skips opening and closing brackets
            {
                if (c == '[') innerIndex++;
                if (c == ']') innerIndex--;
                
                if (c == ',' && innerIndex == 0)
                {
                    content.Add(new Packet(part, this));
                    part = string.Empty;
                    continue;
                }
                part += c;
            }
            if (part != string.Empty) content.Add(new Packet(part, this));
        }
    }
}
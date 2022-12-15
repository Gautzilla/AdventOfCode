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
        private string display;
        public string Display
        {
            get { return display; }
            set { display = value; }
        }

        public Packet(string packet)
        {
            content = new();
            isNumber = false;
            display = packet;

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
                    content.Add(new Packet(part));
                    part = string.Empty;
                    continue;
                }
                part += c;
            }
            if (part != string.Empty) content.Add(new Packet(part));
        }
        public bool IsGreaterThan(Packet other)
        {
            for (int packet = 0; packet < Math.Max(this.content.Count, other.Content.Count); packet++)
            {
                // Packet runs out of items
                if (packet == this.Content.Count) return false; 
                if (packet == other.Content.Count) return true;

                Packet thisItem = this.Content[packet];
                Packet otherItem = other.Content[packet];

                // Both items are numbers
                if (thisItem.IsNumber && otherItem.IsNumber)
                {
                    if (thisItem.Value == otherItem.Value) continue;                    
                    return thisItem.Value > otherItem.Value;
                }

                // Both items are lists
                if (!thisItem.IsNumber && !otherItem.IsNumber)
                {
                    if (thisItem.IsEqualTo(otherItem)) continue;
                    return thisItem.IsGreaterThan(otherItem);
                }

                // Only one is a number
                Packet item1 = new("[]");
                Packet item2 = new("[]");
                if (thisItem.IsNumber)
                {
                    item1 = new Packet($"[{thisItem.Value}]");
                    item2 = otherItem;
                }
                else 
                {
                    item1 = thisItem;
                    item2 = new Packet($"[{otherItem.Value}]");
                }

                if (item1.IsEqualTo(item2)) continue;
                return item1 .IsGreaterThan(item2);
            }

            return true;
        }

        public bool IsEqualTo(Packet other) => this.Display == other.Display;
    }
}
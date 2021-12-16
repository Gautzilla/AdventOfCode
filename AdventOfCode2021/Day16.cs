using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day16
    {
        static public void Solve(int part)
        {
            // string path = @"..\..\Inputs\day16Example.txt";
            string path = @"..\..\Inputs\day16.txt";

            Console.WriteLine(HexToBinary("D2FE28"));

        }

        static private string HexToBinary(string hexString)
        {
            string binaryString = "";
            foreach (char c in hexString)
            {
                binaryString += (Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
            }
            return binaryString;
        }
    }
}

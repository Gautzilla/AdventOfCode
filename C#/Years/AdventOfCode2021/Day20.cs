using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day20
    {
        static public void Solve(int part)
        {

            //string filePath = @"..\..\Inputs\day20Example.txt";
            string filePath = @"..\..\Inputs\day20.txt";

            string[] input = File.ReadAllLines(filePath).Where(l => l != "").ToArray();

            char[] enhancementAlgorithm = input.First().ToCharArray();
            char[,] image = ParseImage(input.Skip(1).ToArray());

            int enhancementSteps = part == 1 ? 2 : 50;

            image = EnhanceImage(image, enhancementAlgorithm, enhancementSteps);

            Console.WriteLine($"There are {CountLitPixels(image)} lit pixels.");
                        
        }

        static private char[,] ParseImage (string[] input)
        {
            char[,] image = new char[input.Length, input[0].Length];

            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    image[x, y] = input[y][x];
                }
            }

            return image;
        }

        static private void DisplayImage(char[,] image)
        {
            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    Console.Write(image[x,y].ToString());
                }
                Console.Write("\n");
            }
        }

        static private char[,] EnhanceImage (char[,] image, char[] enhancementAlgorithm, int enhancementSteps)
        {
            for (int step = 0; step < enhancementSteps; step++)
            {
                char[,] enhancedImage = new char[image.GetLength(0) + 2, image.GetLength(1) + 2];

                for (int x = 0; x < enhancedImage.GetLength(0); x++)
                {
                    for (int y = 0; y < enhancedImage.GetLength(1); y++)
                    {
                        string enhancementCoordinate = "";

                        for (int j = -1; j <= 1; j++)
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                if ( (x+i <= 0) || (x+i >= enhancedImage.GetLength(0)-1) || (y+j <= 0) || (y+j >= enhancedImage.GetLength(1)-1) )
                                {
                                    enhancementCoordinate += (enhancementAlgorithm[0] == '#' && step % 2 != 0) ? "1" : "0"; // If enhancementAlgorithm[0] == '#' ; every pixel outside the image is lit on odd steps.
                                } else
                                {
                                    enhancementCoordinate += image[x + i - 1, y + j - 1] == '#' ? "1" : "0";
                                }
                            }
                        }

                        enhancedImage[x, y] = enhancementAlgorithm[Convert.ToInt32(enhancementCoordinate, 2)];
                    }
                }
                image = enhancedImage;      
            }
            return image;
        }

        static private int CountLitPixels (char[,] image)
        {
            int litPixels = 0;

            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    if (image[x, y] == '#') litPixels++;
                }
            }

            return litPixels;
        }
    }
}
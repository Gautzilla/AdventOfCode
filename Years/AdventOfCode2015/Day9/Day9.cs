using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day9
    {
        private class Location
        {
            public string Name { get; private set; }
            public Dictionary<Location, int> Distances { get; private set; }

            public Location(string name)
            {
                this.Name = name;
                Distances = new();
            }
        }

        private static HashSet<Location> _locations = new();

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day9\input.txt");

            foreach (string distance in input) ParseDistance(distance);
            
            bool lookingForMinDistance = part == 1;

            if (lookingForMinDistance) Console.WriteLine(_locations.Min(location => DFS(location, new HashSet<Location>(_locations), 0, lookingForMinDistance)));
            else Console.WriteLine(_locations.Max(location => DFS(location, new HashSet<Location>(_locations), 0, lookingForMinDistance)));
        }

        private static void ParseDistance(string distanceLine)
        {
            string[] elements = distanceLine.Split(' ');

            Location firstLocation = GetLocation(elements[0]);
            Location secondLocation = GetLocation(elements[2]);
            int distance = int.Parse(elements.Last());

            firstLocation.Distances.Add(secondLocation, distance);
            secondLocation.Distances.Add(firstLocation, distance);
        }

        private static Location GetLocation(string name)
        {
            if (_locations.Any(location => location.Name == name)) return _locations.Single(location => location.Name == name);

            Location output = new Location(name);
            _locations.Add(output);
            return output;
        }

        private static int DFS(Location currentLocation, HashSet<Location> locationsToVisit, int totalDistance, bool lookingForMinDistance)
        {
            locationsToVisit.Remove(currentLocation);

            if (locationsToVisit.Count == 0) return totalDistance;

            int comparisonDistance = lookingForMinDistance ? int.MaxValue : int.MinValue;

            foreach (Location location in locationsToVisit)
            {
                int distance = currentLocation.Distances[location];
                HashSet<Location> remainingLocations = new HashSet<Location>(locationsToVisit);
                
                int nextPathDistance = DFS(location, remainingLocations, totalDistance + distance, lookingForMinDistance);

                comparisonDistance = lookingForMinDistance ? Math.Min(comparisonDistance, nextPathDistance) : Math.Max(comparisonDistance, nextPathDistance);
            }

            return comparisonDistance;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day19
    {
        enum Resources 
        {
            ore,
            clay,
            obsidian
        }

        enum Robot
        {
            ore,
            clay,
            obsidian,
            geode
        }

        class Blueprint
        {
            private Dictionary<Robot, List<(Resources resources, int amount)>> robotCosts;
            public Dictionary<Robot, List<(Resources resources, int amount)>> RobotCosts
            {
                get { return robotCosts; }
                set { robotCosts = value; }
            }

            public Blueprint()
            {
                robotCosts = new();
            }

            public void AddEntry(Robot robot, List<(Resources resources, int amount)> cost)
            {
                RobotCosts.Add(robot, cost);
            }
        }

        private static HashSet<Blueprint> _blueprints = new();

        private static readonly Dictionary<string, Resources> _resources = new Dictionary<string, Resources>()
        {
            {"ore", Resources.ore},
            {"clay", Resources.clay},
            {"obsidian", Resources.obsidian}
        };

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day19\input.txt");
            foreach (var blueprint in input) _blueprints.Add(ParseBlueprint(blueprint));
        
            foreach (var bp in _blueprints)
            {
                Console.WriteLine(String.Join(" ", bp.RobotCosts.Select(rC => $"Each {rC.Key} robot costs {String.Join(" and ", rC.Value.Select(v => $"{v.amount} {v.resources}"))}")));
            }
        }

        private static Blueprint ParseBlueprint(string input)
        {
            Blueprint blueprint = new Blueprint();

            foreach (var entry in Regex.Matches(input, @"Each \w+ robot costs (\d+ \w+( and )?)+.").Select(m => m.Value))
            {
                Robot robot = Regex.Match(entry, @"Each (?<robotType>\w+) robot").Groups["robotType"].Value switch 
            {
                "ore" => Robot.ore,
                "clay" => Robot.clay,
                "obsidian" => Robot.obsidian,
                "geode" => Robot.geode,
                _ => Robot.ore
            };

            List<(Resources, int)> cost = Regex.Matches(entry, @"(?<amount>\d+) (?<type>\w+)").Select(m => (_resources[m.Groups["type"].Value], int.Parse(m.Groups["amount"].Value))).ToList();
        
            blueprint.AddEntry(robot, cost); 
            }     

            return blueprint;                  
        }
    }
}
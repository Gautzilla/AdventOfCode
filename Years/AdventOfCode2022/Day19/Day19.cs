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
            obsidian,
            geode
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

        class State
        {
            private int spentMinutes;
            public int SpentMinutes
            {
                get { return spentMinutes; }
                set { spentMinutes = value; }
            }

            private Dictionary<Resources, int> stock;
            public Dictionary<Resources, int> Stock
            {
                get { return stock; }
                set { stock = value; }
            }

            private Dictionary<Robot, int> robots;
            public Dictionary<Robot, int> Robots
            {
                get { return robots; }
                set { robots = value; }
            }

            public State? PreviousState { get; set; }
            
            public State()
            {
                spentMinutes = 1;
                robots = new Dictionary<Robot, int>()
                {
                    {Robot.ore, 1},
                    {Robot.clay, 0},
                    {Robot.obsidian, 0},
                    {Robot.geode, 0}
                };
                stock = new Dictionary<Resources, int>()
                {
                    {Resources.ore, 0},
                    {Resources.clay, 0},
                    {Resources.obsidian, 0},
                    {Resources.geode, 0}
                };
            }

            public State(int remainingMinutes, Dictionary<Resources, int> stock, Dictionary<Robot, int> robots, State previousState)
            {
                this.spentMinutes = remainingMinutes;
                this.stock = stock;
                this.robots = robots;
                this.PreviousState = previousState;
            }

            public void PrintState()
            {
                Console.WriteLine("TIME: " + spentMinutes);
                foreach (var robot in robots) Console.WriteLine($"{robot.Value} {robot.Key} robot.");
                Console.WriteLine();
                foreach (var resources in stock) Console.WriteLine($"{resources.Value} {resources.Key}.");
                Console.WriteLine("--------");
            }
        }

        private static HashSet<Blueprint> _blueprints = new();

        private static readonly Dictionary<string, Resources> _resources = new Dictionary<string, Resources>()
        {
            {"ore", Resources.ore},
            {"clay", Resources.clay},
            {"obsidian", Resources.obsidian}
        };

        private static readonly Dictionary<Robot, Resources> _production = new Dictionary<Robot, Resources>()
        {
            {Robot.ore, Resources.ore},
            {Robot.clay, Resources.clay},
            {Robot.obsidian, Resources.obsidian},
            {Robot.geode, Resources.geode},
        };

        private static int _maxGeode = int.MinValue;

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day19\input.txt");
            foreach (var blueprint in input) _blueprints.Add(ParseBlueprint(blueprint));
        
            foreach (var bp in _blueprints)
            {
                Console.WriteLine(String.Join(" ", bp.RobotCosts.Select(rC => $"Each {rC.Key} robot costs {String.Join(" and ", rC.Value.Select(v => $"{v.amount} {v.resources}"))}")));
                Console.WriteLine(DFS(new State(), bp, null));
            }
            Console.WriteLine(_maxGeode);
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

        private static int DFS (State state, Blueprint blueprint, Robot? robotToBuild)
        {            
            if (state.Stock[Resources.geode] == 7)
            {
                while (state.PreviousState != null)
                {
                    state.PrintState();
                    state = state.PreviousState;
                }
                 Console.ReadKey();
            }

            if (state.SpentMinutes >= 24) 
            {
                _maxGeode = Math.Max(_maxGeode, state.Stock[Resources.geode]);
                return state.Stock[Resources.geode];
            }

            foreach (var robot in state.Robots) state.Stock[_production[robot.Key]] += robot.Value; 

            if (robotToBuild != null)
            {
                state.Robots[robotToBuild.Value]++;

                foreach (var cost in blueprint.RobotCosts[robotToBuild.Value]) state.Stock[cost.resources] -= cost.amount;
            }        
                     

            int maxGeode = int.MinValue;

            foreach (var robotCost in blueprint.RobotCosts)
            {
                // if state contains no robot for producing the required ressources
                if (robotCost.Value.Any(v => state.Robots.Single(r => _production[r.Key] == v.resources).Value == 0)) continue;

                // if state already contains enough of the ressources that robot produces
                if (IsUnnecessary(state, robotCost.Key, blueprint)) continue;

                int minutesBeforeBuilding = MinutesBeforeBuilding(state.Stock, state.Robots, robotCost.Value);
                if (minutesBeforeBuilding + state.SpentMinutes > 24) continue;

                maxGeode = Math.Max(maxGeode, DFS(GenerateRessources(state, minutesBeforeBuilding), blueprint, robotCost.Key));
            }

            return maxGeode > int.MinValue ? maxGeode : DFS(GenerateRessources(state, 24 - state.SpentMinutes), blueprint, null);
            }

        private static State BuildRobot (State previousState, Robot newRobot, List<(Resources resources, int amount)> robotCost, int minutesBeforeBuilding)
        {
            State nextState = GenerateRessources(previousState, minutesBeforeBuilding);

            nextState.Robots[newRobot]++;

            foreach (var cost in robotCost) nextState.Stock[cost.resources] -= cost.amount;

            return nextState;
        }

        private static State GenerateRessources (State previousState, int numberOfMinutes)
        {
            int nextSpentMinutes = previousState.SpentMinutes + numberOfMinutes;
            var stock = new Dictionary<Resources, int> (previousState.Stock);
            var robots = new Dictionary<Robot, int> (previousState.Robots);

            foreach (var robot in previousState.Robots) stock[_production[robot.Key]] += robot.Value * (numberOfMinutes-1);

            return new State(nextSpentMinutes, stock, robots, previousState);
        }

        private static bool IsUnnecessary (State state, Robot robot, Blueprint blueprint)
        {
            if (robot == Robot.geode) return false;

            int mostExpensiveRobot = blueprint.RobotCosts.SelectMany(rC => rC.Value).Where(cost => cost.resources == _production[robot]).Max(cost => cost.amount);
            return state.Robots[robot] >= mostExpensiveRobot || state.Stock[_production[robot]] >= mostExpensiveRobot;
        }

        private static int MinutesBeforeBuilding (Dictionary<Resources, int> stock, Dictionary<Robot, int> robots, List<(Resources resources, int amount)> cost)
        {
            int time = int.MinValue;
            foreach (var item in cost)
            {
                int needed = item.amount;
                Robot robot = _production.Single(r => r.Value == item.resources).Key;
                int timeBeforeBuilding = 1 + (int)Math.Ceiling(Math.Max(0f, needed - stock[item.resources])/robots[robot]);
                
                time = Math.Max(time, timeBeforeBuilding);
            }
            return time;
        }
    }
}
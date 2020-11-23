using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day11
    {
        public static void Solve()
        {
            var instructions = FileReader.ParseDataFromFile<long>("Data/Day11.txt" ,',');

            var numberOfPaintedPanels = GetNumberOfPanelsRobotPaint(instructions);
            Console.WriteLine(numberOfPaintedPanels);

            GetSpaceShipVinNumber(instructions);
        }

        private static void GetSpaceShipVinNumber(List<long> instructions)
        {
            var copyInstructions = instructions
                .Select((val, i) => new { Value = val, Index = i })
                .ToDictionary(entry => (long)entry.Index, entry => entry.Value);

            var panelsPainted = new Dictionary<Panel, long>();
            var dx = 0;
            var dy = -1;
            var robotPosition = new Panel { X = 0, Y = 0 };
            panelsPainted.Add(robotPosition, 1);
            var intcodeComputer = new BigIntcodeComputer
            {
                Instructions = copyInstructions,
                Inputs = new List<long>(),
                Outputs = new List<long>(),
                RelativeBase = 0
            };
            while (true)
            {
                intcodeComputer.Outputs = new List<long>();
                var panelColor = panelsPainted.GetOrDefault(robotPosition);
                intcodeComputer.Inputs.Add(panelColor);
                var isEnd = intcodeComputer.ProcessInstruction(true, 2);

                if (intcodeComputer.Outputs.Count < 2 || isEnd) break;

                var outputs = intcodeComputer.Outputs;
                panelColor = outputs[0];
                panelsPainted.AddOrUpdateExisting(robotPosition, panelColor);
                var direction = outputs[1];
                if (direction == 0)
                    (dx, dy) = TurnLeft(dx, dy);
                else
                    (dx, dy) = TurnRight(dx, dy);
                robotPosition.X += dx;
                robotPosition.Y += dy;
            }

            var locationsPainted = panelsPainted.Keys.ToList();
            var minY = locationsPainted.Min(p => p.Y);
            var minX = locationsPainted.Min(p => p.X);
            var m = locationsPainted.Max(p => p.Y) - minY + 1;
            var n = locationsPainted.Max(p => p.X) - minX + 1;
            var plate = new char[m][];
            for (var i = 0; i < m; i++)
            {
                plate[i] = new char[n];
            }
            foreach (var p in locationsPainted)
            {
                plate[p.Y - minY][p.X - minX] = panelsPainted[p] == 0 ? ' ' : '#';
            }
            for (var i = 0; i < m; i++)
            {
                if (i == 0 || i == 3 || i == 4)
                    Console.WriteLine(string.Join("", plate[i]).Substring(1));
                else
                    Console.WriteLine(string.Join("", plate[i]));
            }
        }

        private static int GetNumberOfPanelsRobotPaint(List<long> instructions)
        {
            var copyInstructions = instructions
                .Select((val, i) => new {Value = val, Index = i})
                .ToDictionary(entry => (long) entry.Index, entry => entry.Value);

            var panelsPainted = new Dictionary<Panel, long>();
            var dx = 0;
            var dy = -1;

            var robotPosition = new Panel {X = 0, Y = 0};
            var intcodeComputer = new BigIntcodeComputer
            {
                Instructions = copyInstructions,
                Inputs = new List<long>(),
                Outputs = new List<long>(),
                RelativeBase = 0
            };
            while (true)
            {
                intcodeComputer.Outputs = new List<long>();
                var panelColor = panelsPainted.GetOrDefault(robotPosition);
                intcodeComputer.Inputs.Add(panelColor);
                var isEnd = intcodeComputer.ProcessInstruction(true, 2);

                if (intcodeComputer.Outputs.Count < 2 || isEnd) break;

                var outputs = intcodeComputer.Outputs;
                panelColor = outputs[0];
                panelsPainted.AddOrUpdateExisting(robotPosition, panelColor);
                var direction = outputs[1];
                if (direction == 0) 
                    (dx, dy) = TurnLeft(dx, dy);
                else
                    (dx, dy) = TurnRight(dx, dy);
                robotPosition.X += dx;
                robotPosition.Y += dy;
            }

            return panelsPainted.Count;
        }

        private static (int, int) TurnLeft(int dx, int dy)
        {
            return (dy, -dx);
        }

        private static (int, int) TurnRight(int dx, int dy)
        { 
            return (-dy, dx);
        }

        private struct Panel
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}

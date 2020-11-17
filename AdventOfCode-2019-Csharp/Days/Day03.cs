using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day03
    {
        private readonly struct WirePoint
        {
            private int X { get; }
            private int Y { get; }
            public WirePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int ManhattanDistance(WirePoint p) => Math.Abs(X - p.X) + Math.Abs(Y - p.Y);
        }

        public static void Solve()
        {
            var wires = FileReader.ParseDataFromFile<string>("Data/Day03.txt", '\n');

            var firstWire = RecordWireCoordinates(wires[0].Split(',').ToList());
            var secondWire = RecordWireCoordinates(wires[1].Split(',').ToList());

            var crossPoints = firstWire
                .Join(secondWire, p1 => p1, p2 => p2, (p1, p2) => p1)
                .ToHashSet();

            var closestPoint = crossPoints.Min(point => point.ManhattanDistance(new WirePoint(0, 0)));
            Console.WriteLine(closestPoint);

            var distanceFromFirstWire = RecordWireCoordinatesWithTimeStamp(wires[0].Split(',').ToList(), crossPoints);
            var distanceFromSecondWire = RecordWireCoordinatesWithTimeStamp(wires[1].Split(',').ToList(), crossPoints);
            var minDistanceFromWiresToIntersection = distanceFromFirstWire.Zip(distanceFromSecondWire).Min(group => group.First + group.Second);
            Console.WriteLine(minDistanceFromWiresToIntersection);
        }

        private static HashSet<WirePoint> RecordWireCoordinates(List<string> wireInstructions)
        {
            var coordinates = new HashSet<WirePoint>();
            var x = 0;
            var y = 0;
            foreach (var instruction in wireInstructions)
            {
                var direction = instruction[0];
                var steps = int.Parse(instruction.Substring(1));
                for (var s = 0; s < steps; s++)
                {
                    (x, y) = direction switch
                    {
                        'R' => (x + 1, y),
                        'L' => (x - 1, y),
                        'U' => (x, y - 1),
                        'D' => (x, y + 1),
                        _ => throw new Exception($"Invalid Direction {direction}")
                    };
                    coordinates.Add(new WirePoint(x, y));
                }
            }

            return coordinates;
        }

        private static List<int> RecordWireCoordinatesWithTimeStamp(List<string> wireInstructions, HashSet<WirePoint> crossPoints)
        {

            var distances = crossPoints.Select(point =>
            {
                var x = 0;
                var y = 0;
                var time = 0;
                foreach (var instruction in wireInstructions)
                {
                    var direction = instruction[0];
                    var steps = int.Parse(instruction.Substring(1));
                    for (var s = 0; s < steps; s++)
                    {
                        (x, y) = direction switch
                        {
                            'R' => (x + 1, y),
                            'L' => (x - 1, y),
                            'U' => (x, y - 1),
                            'D' => (x, y + 1),
                            _ => throw new Exception($"Invalid Direction {direction}")
                        };
                        time++;
                        if (point.Equals(new WirePoint(x, y))) return time;
                    }
                }

                return time;
            }).ToList();
            

            return distances;
        }
    }
}

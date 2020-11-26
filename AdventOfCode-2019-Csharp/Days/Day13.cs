using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day13
    {
        public static void Solve()
        {
            var instructions = FileReader.ParseDataFromFile<long>("Data/Day13.txt", ',');

            var copyInstructions = instructions
                .Select((val, i) => new { Value = val, Index = i })
                .ToDictionary(entry => (long)entry.Index, entry => entry.Value);

            var tiles = GenerateArcadeMap(copyInstructions);

            var blockTilesCount = tiles.Count(tile => tile.Id == 2);
            Console.WriteLine(blockTilesCount);
           
            BuildAndPrintMap(tiles);
            var score = GenerateAndPlayArcadeMap(instructions);
            Console.WriteLine(score);
        }

        private static long GenerateAndPlayArcadeMap(List<long> instructions)
        {
            var copyInstructions = instructions
                .Select((val, i) => new { Value = val, Index = i })
                .ToDictionary(entry => (long)entry.Index, entry => entry.Value);


            var tiles = new Dictionary<Tile, int>();
            var intcodeComputer = new BigIntcodeComputer
            {
                Instructions = copyInstructions,
                Inputs = new List<long>(),
                Outputs = new List<long>(),
                RelativeBase = 0
            };
            copyInstructions[0] = 2;

            int? ball = null;
            int? joystick = null;
            var blocksBroke = 0;
            long score = 0;

            while (true)
            {
                intcodeComputer.Inputs = new List<long>();
                if (ball != null && joystick != null)
                {
                    if (ball < joystick)
                        intcodeComputer.Inputs.Add(-1);
                    else if (ball > joystick)
                        intcodeComputer.Inputs.Add(1);
                    else
                        intcodeComputer.Inputs.Add(0);
                }

                intcodeComputer.Outputs = new List<long>();
                var isEnd = intcodeComputer.ProcessInstruction(true, 3);

                if (isEnd) 
                    break;

                var outputs = intcodeComputer.Outputs;
                var x = outputs[0];
                var y = outputs[1];
                var id = outputs[2];

                if (x == -1 && y == 0)
                {
                    score = id;
                    Console.WriteLine($"Blocks broke: {blocksBroke}");
                    Console.WriteLine($"Current Score {id}");
                    BuildAndPrintMap(tiles.Select(entry => new Tile { X = entry.Key.X, Y = entry.Key.Y, Id = entry.Value }));
                }
                else
                {
                    switch (id)
                    {
                        case 3:
                            joystick = (int) x;
                            break;
                        case 4:
                            ball = (int) x;
                            //
                            break;
                    }

                    tiles.AddOrUpdateExisting(new Tile
                    {
                        X = x,
                        Y = y,
                        Id = (int) id
                    }, (int) id);
                }
                
            }

            return score;
        }

        private static List<Tile> GenerateArcadeMap(Dictionary<long, long> instructions)
        {
            var tiles = new List<Tile>();
            var intcodeComputer = new BigIntcodeComputer
            {
                Instructions = instructions,
                Inputs = new List<long>(),
                Outputs = new List<long>(),
                RelativeBase = 0
            };

            while (true)
            {
                intcodeComputer.Outputs = new List<long>();
                var isEnd = intcodeComputer.ProcessInstruction(true, 3);

                if (intcodeComputer.Outputs.Count < 3 || isEnd) break;

                var outputs = intcodeComputer.Outputs;
                tiles.Add(new Tile
                {
                    X = outputs[0],
                    Y = outputs[1],
                    Id = (int) outputs[2]
                });
            }

            return tiles;
        }

        private static void BuildAndPrintMap(IEnumerable<Tile> tiles)
        {
            var tilesList = tiles.ToList();
            var minX = tilesList.Min(tile => tile.X);
            var maxX = tilesList.Max(tile => tile.X);
            var minY = tilesList.Min(tile => tile.Y);
            var maxY = tilesList.Max(tile => tile.Y);
            var m = maxY - minY + 1;
            var n = maxX - minX + 1;
            var map = new char[m][];
            for (var i = 0; i < m; i++)
            {
                map[i] = new char[n];
            }

            foreach (var tile in tilesList)
            {
                map[tile.Y - minY][tile.X - minX] = tile.Id switch
                {
                    1 => '|',
                    2 => '#',
                    3 => '_',
                    4 => 'O',
                    _ => ' '
                };
            }

            foreach (var row in map)
            {
                Console.WriteLine(string.Join("", row));
            }
        }

        private class Tile : IEquatable<Tile>
        {
            public long X { get; set; }
            public long Y { get; set; }
            public int Id { get; set; }

            public bool Equals(Tile other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Tile) obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }
    }
}

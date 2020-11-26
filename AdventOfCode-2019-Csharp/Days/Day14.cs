using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day14
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Data/Day14.txt", '\n');

            var graph = GetRequirementsFromData(lines);
            var oreNeeded = GetOresNeededForFuel(graph);
            Console.WriteLine();
        }

        private static int GetOresNeededForFuel(Dictionary<Chemical, List<Chemical>> graph)
        {
            var chemicalNeeded = new Dictionary<string, int> { ["FUEL"] = 1 };
            var queue = new Queue<Chemical>();
            queue.Enqueue(new Chemical {Id = "FUEL", ProducedValue = 1});
            while (queue.Any())
            {
                var chemical = queue.Dequeue();
                if (!graph.ContainsKey(chemical)) continue;
                foreach (var needed in graph[chemical])
                {
                    if (!chemicalNeeded.ContainsKey(needed.Id))
                        chemicalNeeded.Add(needed.Id, 0);
                    chemicalNeeded[needed.Id] += chemical.ProducedValue * needed.Requirement;
                }
            }

            return chemicalNeeded["ORE"];
        }

        private static Dictionary<Chemical, List<Chemical>> GetRequirementsFromData(List<string> lines)
        {
            var graph = new Dictionary<Chemical, List<Chemical>>();
            foreach (var line in lines)
            {
                var data = line.Split("=>");
                var chemicalData = data[1].Trim().Split();
                var chemical = new Chemical
                {
                    Id = chemicalData[1],
                    ProducedValue = int.Parse(chemicalData[0])
                };
                var chemicalRequirements = data[0].Trim().Split(',')
                    .Select(cr => cr.Trim().Split())
                    .Select(cr => new Chemical
                    {
                        Id = cr[1],
                        Requirement = int.Parse(cr[0])
                    }).ToList();
                graph.Add(chemical, chemicalRequirements);
            }

            return graph;
        }

        private class Chemical : IEquatable<Chemical>
        {
            public string Id { get; set; }
            public int Requirement { get; set; }
            public int ProducedValue { get; set; }

            public bool Equals(Chemical other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Chemical) obj);
            }

            public override int GetHashCode()
            {
                return (Id != null ? Id.GetHashCode() : 0);
            }
        }
    }
}

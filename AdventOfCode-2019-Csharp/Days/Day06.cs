using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day06
    {
        public static void Solve()
        {
            var orbitsData = FileReader.ParseDataFromFile<string>("Data/Day06.txt", '\n');
            var directedAdjacencyList = orbitsData.Select(data => data.Split(')'))
                .ToLookup(data => data[0], data => data[1]);

            var indirectAndDirectCount = Dfs(directedAdjacencyList, "COM", new HashSet<string>(), 1);
            Console.WriteLine(indirectAndDirectCount);

            var reverseAdjacencyList = orbitsData.Select(data => data.Split(')'))
                .ToLookup(data => data[1], data =>  data[0]);
            var adjacencyList = directedAdjacencyList.Union(reverseAdjacencyList)
                .ToLookup(group => group.Key, group => group.ToList())
                .ToDictionary(item => item.Key, item => item.SelectMany(x => x));


            var closestDistanceToSanta = Bfs(adjacencyList, "YOU", "SAN");
            Console.WriteLine(closestDistanceToSanta);
        }


        private static int Dfs(ILookup<string, string> directedAdjacencyList, string station, HashSet<string> seen, int level)
        {
            var edgesCount = 0;
            foreach (var orbit in directedAdjacencyList[station])
            {
                if (seen.Contains(orbit)) continue;
                seen.Add(orbit);
                edgesCount += level + Dfs(directedAdjacencyList, orbit, seen, level + 1);
            }

            return edgesCount;
        }

        private static int Bfs(IDictionary<string, IEnumerable<string>> adjacencyList, string source, string destination)
        {
            var seen = new HashSet<string>();
            var queue = new Queue<(string Station, int Level)>();
            queue.Enqueue((source, 0));
            seen.Add(source);
            while (queue.Any())
            {
                var current = queue.Dequeue();
                if (current.Station == destination) return current.Level - 2;
                foreach (var orbit in adjacencyList[current.Station])
                {
                    if (seen.Contains(orbit)) continue;
                    queue.Enqueue((orbit, current.Level + 1));
                    seen.Add(orbit);
                }
            }

            return -1;
        }
    }
}

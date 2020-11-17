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

            var indirectCount = GetIndirectOrbitsCount(directedAdjacencyList, "COM", new HashSet<string>());
        }

        private static int GetIndirectOrbitsCount(ILookup<string, string> directedAdjacencyList, string station, HashSet<string> seen)
        {
            if (!directedAdjacencyList.Contains(station)) return 1;
            
            var indirectCount = 0;
            foreach (var orbit in directedAdjacencyList[station])
            {
                if (!directedAdjacencyList.Contains(station) || seen.Contains(station)) continue;
                indirectCount += GetIndirectOrbitsCount(directedAdjacencyList, orbit, seen);
            }

            return indirectCount;
        }
    }
}

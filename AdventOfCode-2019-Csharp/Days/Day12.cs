using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day12
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Data/Day12.txt", '\n');
            var moons = lines.Select(line =>
            {
                var regex = new Regex("-?\\d+", RegexOptions.Compiled);
                var matches = regex.Matches(line).Select(x => x.Value).ToList();
                return new Moon
                {
                    X = int.Parse(matches[0]),
                    Y = int.Parse(matches[1]),
                    Z = int.Parse(matches[2])
                };
            });

            var foo = Rankify(new List<int> {1, 1, 2, 3});
            foo = Rankify(new List<int> {3, 2, 1, 4});
            foo = Rankify(new List<int> {1, 2, 3, 3});
            foo = Rankify(new List<int> { 1, 2, 2, 3 });
        }

        private static List<double> Rankify(List<int> array)
        {
            var n = array.Count;
            var ranks = new List<double>();
            for (var i = 0; i < n; i++)
            {
                var r = 1;
                var s = 1;
                for (var j = 0; j < n; j++)
                {
                    if (j != i && array[j] > array[i]) r += 1;
                    if (j != i && array[j] == array[i]) s += 1;
                }
                ranks.Add(r + 0.5 * (s - 1));
            }

            return ranks;
        }


        private class Moon
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }
    }
}

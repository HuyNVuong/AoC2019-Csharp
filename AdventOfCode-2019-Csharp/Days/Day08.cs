using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day08
    {
        public static void Solve()
        {
            var imageData = FileReader.ParseDataFromFile<string>("Data/Day08.txt", '\n').FirstOrDefault();

            var layers = BuildImageLayers(imageData);

            static int CountNFromLayer(List<List<int>> layer, int n) => layer.SelectMany(x => x).Count(x => x == n);

            var layerWithLeastZeroesWithCount = layers
                .OrderBy(layer => CountNFromLayer(layer, 0))
                .Select(layer => new {OneCount = CountNFromLayer(layer, 1), TwoCount = CountNFromLayer(layer, 2)})
                .First();
            Console.WriteLine(layerWithLeastZeroesWithCount.OneCount * layerWithLeastZeroesWithCount.TwoCount);

            var decodedImage = DecodeImage(layers, 6, 25);
            decodedImage.ForEach(row => Console.WriteLine(string.Join("", row)));
        }
            
        private static List<List<List<int>>> BuildImageLayers(string data)
        {
            var layers = new List<List<List<int>>>();
            var layer = new List<List<int>>();
            var row = new List<int>();
            foreach (var c in data)
            {
                row.Add(c - '0');
                if (row.Count != 25) continue;
                layer.Add(row);
                row = new List<int>();
                if (layer.Count != 6) continue;
                layers.Add(layer);
                layer = new List<List<int>>();
            }

            return layers;
        }

        private static List<List<char>> DecodeImage(List<List<List<int>>> layers, int m, int n)
        {
            var decodedData = new List<List<char>>();
            for (var i = 0; i < m; i++)
            {
                var row = new List<char>();
                for (var j = 0; j < n; j++)
                {
                    foreach (var layer in layers)
                    {
                        if (layer[i][j] == 2) continue;
                        row.Add(layer[i][j] == 1 ? '#' : ' ');
                        break;
                    }
                }
                decodedData.Add(row);
            }

            return decodedData;
        }
    }
}

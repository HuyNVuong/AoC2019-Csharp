using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode_2019_Csharp.Helper
{
    public static class FileReader
    {
        public static List<T> ParseDataFromFile<T>(string fileLocation, char delimiter)
        {
            var lines = File.ReadAllText(fileLocation)
                .Split(delimiter)
                .Select(line => (T) Convert.ChangeType(line.Trim(new char[]{'\r', '\n'}), typeof(T)))
                .ToList();

            return lines;
        }
    }
}

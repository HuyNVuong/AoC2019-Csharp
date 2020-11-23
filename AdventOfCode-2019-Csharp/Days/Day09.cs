using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day09
    {
        public static void Solve()
        {
            var instructions = FileReader.ParseDataFromFile<long>("Data/Day09.txt", ',');
            var bigMemoryInstruction = instructions
                .Select((code, index) => new {Index = index, Code = code})
                .ToDictionary(item => (long) item.Index, item => item.Code);

            var keycode = GetBoostKeycodeFromTest(bigMemoryInstruction);
            Console.WriteLine(keycode);

            var sensoryCode = GetBoostSensorMode(bigMemoryInstruction);
            Console.WriteLine(sensoryCode);
        }

        private static long GetBoostKeycodeFromTest(Dictionary<long, long> instructions)
        {
            var intcodeComputer = new BigIntcodeComputer
            {
                Instructions = instructions.ToDictionary(entry => entry.Key, entry => entry.Value),
                RelativeBase = 0,
                Inputs = new List<long> {1},
                Outputs = new List<long>()
            };

            intcodeComputer.ProcessInstruction();

            return intcodeComputer.Outputs.Last();
        }

        private static long GetBoostSensorMode(Dictionary<long, long> instructions)
        {
            var intcodeComputer = new BigIntcodeComputer
            {
                Instructions = instructions.ToDictionary(entry => entry.Key, entry => entry.Value),
                RelativeBase = 0,
                Inputs = new List<long> { 2 },
                Outputs = new List<long>()
            };

            intcodeComputer.ProcessInstruction();

            return intcodeComputer.Outputs.Last();
        }
    }
}

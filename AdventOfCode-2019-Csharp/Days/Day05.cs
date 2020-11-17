using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day05
    {
        public static void Solve()
        {
            var intcodeInstructions = FileReader.ParseDataFromFile<int>("Data/Day05.txt", ',');
            RunIntcodeInstruction(intcodeInstructions);
            RunIntcodeInstructionWithId(intcodeInstructions, 5);
        }

        private static void RunIntcodeInstruction(List<int> intcodeInstructions)
        {
            var intcodeComputer = new IntcodeComputer
            {
                Instructions = intcodeInstructions.Select(x => x).ToList(),
                Inputs = new List<int> {1},
                Outputs = new List<int>()
            };
            intcodeComputer.ProcessInstruction();
            var outputs = intcodeComputer.Outputs;
            Console.WriteLine(outputs.Last());
        }

        private static void RunIntcodeInstructionWithId(List<int> intcodeInstructions, int id)
        {
            var intcodeComputer = new IntcodeComputer
            {
                Instructions = intcodeInstructions.Select(x => x).ToList(),
                Inputs = new List<int> { id },
                Outputs = new List<int>()
            };
            intcodeComputer.ProcessInstruction();
            var outputs = intcodeComputer.Outputs;
            Console.WriteLine(outputs.Last());
        }
    }
}

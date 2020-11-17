using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day02
    {
        public static void Solve()
        {
            var intCodeInstructions = FileReader.ParseDataFromFile<int>("Data/Day02.txt", ',');

            var intcodeResult = ExecuteIntcodeComputerFromInstruction(intCodeInstructions);
            Console.WriteLine(intcodeResult);

            var (noun, verb) = TrySetIntcodeForValues(intCodeInstructions, 19690720);
            Console.WriteLine(noun * 100 + verb);
        }

        private static int ExecuteIntcodeComputerFromInstruction(List<int> instruction)
        {
            var intCodeComputer = new IntcodeComputer
            {
                Instructions = instruction.Select(x => x).ToList()
            };
            intCodeComputer.Set(new Dictionary<int, int>
            {
                [1] = 12,
                [2] = 2
            });
            intCodeComputer.ProcessInstruction();

            return intCodeComputer.Instructions[0];
        }

        private static (int, int) TrySetIntcodeForValues(List<int> instruction, int expectedValue)
        {
            
            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    var intCodeComputer = new IntcodeComputer
                    {
                        Instructions = instruction.Select(x => x).ToList()
                    };
                    intCodeComputer.Set(new Dictionary<int, int>
                    {
                        [1] = noun,
                        [2] = verb
                    });
                    intCodeComputer.ProcessInstruction();
                    if (intCodeComputer.Instructions[0] == expectedValue) return (noun, verb);
                }
            }
            
            throw new Exception($"Can't find Noun Verb for values {expectedValue}");
        }
    }
}

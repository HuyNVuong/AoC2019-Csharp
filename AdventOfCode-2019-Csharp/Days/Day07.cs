using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    class Day07
    {
        public static void Solve()
        {
            var intcodeInstructions = FileReader.ParseDataFromFile<int>("Data/Day07.txt", ',');
            var settings = GetAllSettings(new[] {0, 1, 2, 3, 4});

            var highestSignal = GetHighestSignalSentFromThrusters(settings, intcodeInstructions);
            Console.WriteLine(highestSignal);

            var feedBackSetting = GetAllSettings(new[] {5, 6, 7, 8, 9});
            var highestSignalFromFeedbackLoop = GetHighestSignalFromFeedbackLoopThrusters(feedBackSetting, intcodeInstructions);
            Console.WriteLine(highestSignalFromFeedbackLoop);
        }

        private static List<List<int>> GetAllSettings(int[] candidates)
        {
            var allSettings = new List<List<int>>();
            Utilities.Permutations.ForAllPermutation(candidates, (permutation =>
            {
                allSettings.Add(permutation.ToList());
                return false;
            }));

            return allSettings;
        }

        private static int GetHighestSignalSentFromThrusters(List<List<int>> settings, List<int> instructions)
        {
            var signal = 0;
            foreach (var setting in settings)
            {
                var amplifiers = setting.Select(s => new IntcodeComputer
                {
                    Instructions = instructions.Select(x => x).ToList(),
                    Inputs = new List<int> {s},
                    Outputs = new List<int>()
                }).ToList();
                var secondInput = 0;
                foreach (var amplifier in amplifiers)
                {
                    amplifier.Inputs.Add(secondInput);
                    amplifier.ProcessInstruction();
                    secondInput = amplifier.Outputs.LastOrDefault();
                }

                signal = Math.Max(signal, secondInput);
            }

            return signal;
        }

        private static int GetHighestSignalFromFeedbackLoopThrusters(List<List<int>> settings, List<int> instructions)
        {
            var signal = 0;
            foreach (var setting in settings)
            {
                var amplifiers = setting.Select(s => new IntcodeComputer
                {
                    Instructions = instructions.Select(x => x).ToList(),
                    Inputs = new List<int> { s },
                    Outputs = new List<int>(),
                    StartingPosition = 0
                }).ToList();
                var secondInput = 0;
                var amplifiersQueue = new Queue<(IntcodeComputer Amplifier, int Index)>();
                amplifiersQueue.Enqueue((amplifiers[0], 0));
                var amplifierPointer = 0;
                var amplifierFinished = 0;
                while (amplifierFinished < amplifiers.Count)
                {
                    amplifiers[amplifierPointer].Inputs.Add(secondInput);
                    amplifiers[amplifierPointer].ProcessInstruction(true);
                    if (amplifiers[amplifierPointer].Outputs.Any())
                    {
                        secondInput = amplifiers[amplifierPointer].Outputs.Last();
                        amplifiers[amplifierPointer].Outputs = new List<int>();
                    }
                    else
                    {
                        amplifierFinished++;
                    }

                    amplifierPointer = (amplifierPointer + 1) % amplifiers.Count;
                }

                signal = Math.Max(signal, secondInput);
            }

            return signal;
        }

    }

}


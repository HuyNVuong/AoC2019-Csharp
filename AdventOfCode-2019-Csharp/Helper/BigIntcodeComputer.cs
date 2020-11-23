using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2019_Csharp.Helper
{
    public class BigIntcodeComputer
    {
        private const int Halts = 99;
        private const int Adds = 1;
        private const int Multiplies = 2;
        private const int SaveInput = 3;
        private const int Output = 4;
        private const int JumpIfTrue = 5;
        private const int JumpIfFalse = 6;
        private const int LessThan = 7;
        private const int IfEquals = 8;
        private const int AdjustBase = 9;

        public Dictionary<long, long> Instructions { get; set; }
        public List<long> Inputs { get; set; }
        public List<long> Outputs { get; set; }
        public long? StartingPosition { get; set; }
        public long? RelativeBase { get; set; }

        public void Set(IDictionary<int, int> dictionary)
        {
            foreach (var (i, value) in dictionary)
            {
                Instructions[i] = value;
            }
        }

        public bool ProcessInstruction(bool returnIfOutputFound = false, int numberOfOutputsExpected = 1)
        {
            var i = StartingPosition ?? 0;
            while (true)
            {
                if (returnIfOutputFound && Outputs.Any() && Outputs.Count == numberOfOutputsExpected)
                {
                    StartingPosition = i;
                    return false;
                }

                var (opcode, thirdParameterMode, secondParameterMode, firstParameterMode) = GetOpcodeAndMode(Instructions[i]);
                long nextInstruction;
                switch (opcode)
                {
                    case Adds:
                        nextInstruction = AddsInstruction(i, secondParameterMode, firstParameterMode, thirdParameterMode);
                        break;
                    case Multiplies:
                        nextInstruction = MultipliesInstruction(i, secondParameterMode, firstParameterMode, thirdParameterMode);
                        break;
                    case SaveInput:
                        nextInstruction = SaveInputInstruction(i, firstParameterMode);
                        break;
                    case Output:
                        nextInstruction = OutputInstruction(i, firstParameterMode);
                        break;
                    case JumpIfTrue:
                        nextInstruction = JumpIfTrueInstruction(i, firstParameterMode, secondParameterMode);
                        break;
                    case JumpIfFalse:
                        nextInstruction = JumpIfFalseInstruction(i, firstParameterMode, secondParameterMode);
                        break;
                    case LessThan:
                        nextInstruction = LessThanInstruction(i, secondParameterMode, firstParameterMode, thirdParameterMode);
                        break;
                    case IfEquals:
                        nextInstruction = IfEqualsInstruction(i, secondParameterMode, firstParameterMode, thirdParameterMode);
                        break;
                    case AdjustBase:
                        nextInstruction = AdjustRelativeBaseInstruction(i, firstParameterMode);
                        break;
                    case Halts:
                        return true;
                    default:
                        throw new Exception($"Invalid Op code {opcode}");
                }

                i = nextInstruction;
            }
        }

        private long AddsInstruction(long i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            var result = Instructions.GetOrDefault(a) + Instructions.GetOrDefault(b);
            Instructions.AddOrUpdateExisting(c, result);

            return i + 4;
        }

        private long MultipliesInstruction(long i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            var result = Instructions.GetOrDefault(a) * Instructions.GetOrDefault(b);
            Instructions.AddOrUpdateExisting(c, result);

            return i + 4;
        }

        private long SaveInputInstruction(long i, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeA);
            Instructions.AddOrUpdateExisting(a, Inputs.First());
            Inputs.RemoveAt(0);

            return i + 2;
        }

        private long OutputInstruction(long i, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeA);
            Outputs.Add(Instructions[a]);

            return i + 2;
        }

        private long JumpIfTrueInstruction(long i, int modeA, int modeB)
        {
            var a = GetPositionValue(i + 1, modeA);
            var b = GetPositionValue(i + 2, modeB);

            return Instructions.GetOrDefault(a) != 0 ? Instructions.GetOrDefault(b) : i + 3;
        }

        private long JumpIfFalseInstruction(long i, int modeA, int modeB)
        {
            var a = GetPositionValue(i + 1, modeA);
            var b = GetPositionValue(i + 2, modeB);

            return Instructions.GetOrDefault(a) == 0 ? Instructions.GetOrDefault(b) : i + 3;
        }

        private long LessThanInstruction(long i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            Instructions.AddOrUpdateExisting(c, Instructions.GetOrDefault(a) < Instructions.GetOrDefault(b) ? 1 : 0);

            return i + 4;
        }

        private long IfEqualsInstruction(long i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            Instructions.AddOrUpdateExisting(c, Instructions.GetOrDefault(a) == Instructions.GetOrDefault(b) ? 1 : 0);

            return i + 4;
        }

        private long AdjustRelativeBaseInstruction(long i, int mode)
        {
            var a = GetPositionValue(i + 1, mode);
            RelativeBase += Instructions.GetOrDefault(a);

            return i + 2;
        }

        private (int, int, int, int) GetOpcodeAndMode(long code)
        {
            var opcode = (int)code % 100;
            var a = (int) code / 10000;
            var b = (int) code / 1000 % 10;
            var c = (int) code / 100 % 10;

            return (opcode, a, b, c);
        }



        private long GetPositionValue(long i, int mode)
        {
            return mode switch
            {
                0 => Instructions.GetOrDefault(i),
                1 => i,
                2 => (RelativeBase ?? 0) + Instructions[i],
                _ => throw new Exception($"Invalid mode: {mode} value")
            };
        }
    }
}
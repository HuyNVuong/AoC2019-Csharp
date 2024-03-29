﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2019_Csharp.Helper
{
    public class IntcodeComputer
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

        public List<int> Instructions { get; set; }
        public List<int> Inputs { get; set; }
        public List<int> Outputs { get; set; }
        public int? StartingPosition { get; set; }
        public int? RelativeBase { get; set; }

        public void Set(IDictionary<int, int> dictionary)
        {
            foreach (var (i, value) in dictionary)
            {
                Instructions[i] = value;
            }
        }

        public bool ProcessInstruction(bool returnIfOutputFound = false)
        {
            var i = StartingPosition ?? 0;
            while (true)
            {
                if (returnIfOutputFound && Outputs.Any())
                {
                    StartingPosition = i;
                    return true;
                }

                var (opcode, thirdParameterMode, secondParameterMode, firstParameterMode) = GetOpcodeAndMode(Instructions[i]);
                int nextInstruction;
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

        private int AddsInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            var result = Instructions[a] + Instructions[b];
            Instructions[c] = result;

            return i + 4;
        }

        private int MultipliesInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            var result = Instructions[a] * Instructions[b];
            Instructions[c] = result;

            return i + 4;
        }

        private int SaveInputInstruction(int i, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeA);
            Instructions[a] = Inputs.First();
            Inputs.RemoveAt(0);

            return i + 2;
        }

        private int OutputInstruction(int i, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeA);
            Outputs.Add(Instructions[a]);

            return i + 2;
        }

        private int JumpIfTrueInstruction(int i, int modeA, int modeB)
        {
            var a = GetPositionValue(i + 1, modeA);
            var b = GetPositionValue(i + 2, modeB);

            return Instructions[a] != 0 ? Instructions[b] : i + 3;
        }

        private int JumpIfFalseInstruction(int i, int modeA, int modeB)
        {
            var a = GetPositionValue(i + 1, modeA);
            var b = GetPositionValue(i + 2, modeB);

            return Instructions[a] == 0 ? Instructions[b] : i + 3;
        }

        private int LessThanInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            Instructions[c] = Instructions[a] < Instructions[b] ? 1 : 0;

            return i + 4;
        }

        private int IfEqualsInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = GetPositionValue(i + 1, modeC);
            var b = GetPositionValue(i + 2, modeB);
            var c = GetPositionValue(i + 3, modeA);
            Instructions[c] = Instructions[a] == Instructions[b] ? 1 : 0;

            return i + 4;
        }

        private (int, int, int, int) GetOpcodeAndMode(int code)
        {
            var opcode = code % 100;
            var a = code / 10000;
            var b = code / 1000 % 10;
            var c = code / 100 % 10;

            return (opcode, a, b, c);
        }

        private int AdjustRelativeBaseInstruction(int i, int mode)
        {
            var a = GetPositionValue(i + 1, mode);
            RelativeBase ??= RelativeBase + Instructions[a];

            return i + 2;
        }

        private int GetPositionValue(int i, int mode)
        {
            return mode switch
            {
                0 => Instructions[i],
                1 => i,
                2 => RelativeBase ?? 0 + Instructions[i],
                _ => throw new Exception($"Invalid mode: {mode} value")
            };
        }
    }
}

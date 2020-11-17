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

        public List<int> Instructions { get; set; }
        public List<int> Inputs { get; set; }
        public List<int> Outputs { get; set; }


        public void Set(IDictionary<int, int> dictionary)
        {
            foreach (var (i, value) in dictionary)
            {
                Instructions[i] = value;
            }
        }

        public bool ProcessInstruction()
        {
            var i = 0;
            while (i != Halts)
            {
                var (opcode, _, modeB, modeC) = GetOpcodeAndMode(Instructions[i]);
                int nextInstruction;
                switch (opcode)
                {
                    case Adds:
                        nextInstruction = AddsInstruction(i, modeB, modeC);
                        break;
                    case Multiplies:
                        nextInstruction = MultipliesInstruction(i, modeB, modeC);
                        break;
                    case SaveInput:
                        nextInstruction = SaveInputInstruction(i);
                        break;
                    case Output:
                        nextInstruction = OutputInstruction(i, modeB);
                        break;
                    case JumpIfTrue:
                        nextInstruction = JumpIfTrueInstruction(i, modeC, modeB);
                        break;
                    case JumpIfFalse:
                        nextInstruction = JumpIfFalseInstruction(i, modeC, modeB);
                        break;
                    case LessThan:
                        nextInstruction = LessThanInstruction(i, modeB, modeC);
                        break;
                    case IfEquals:
                        nextInstruction = IfEqualsInstruction(i, modeB, modeC);
                        break;
                    case Halts:
                        return true;
                    default:
                        return false;
                }

                i = nextInstruction;
            }

            return true;
        }

        private int AddsInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = modeC == 0 ? Instructions[i + 1] : i + 1;
            var b = modeB == 0 ? Instructions[i + 2] : i + 2;
            var c = modeA == 0 ? Instructions[i + 3] : i + 3;
            var result = Instructions[a] + Instructions[b];
            Instructions[c] = result;

            return i + 4;
        }

        private int MultipliesInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = modeC == 0 ? Instructions[i + 1] : i + 1;
            var b = modeB == 0 ? Instructions[i + 2] : i + 2;
            var c = modeA == 0 ? Instructions[i + 3] : i + 3;
            var result = Instructions[a] * Instructions[b];
            Instructions[c] = result;

            return i + 4;
        }

        private int SaveInputInstruction(int i, int modeA = 0)
        {
            var a = modeA == 0 ? Instructions[i + 1] : i + 1;
            Instructions[a] = Inputs.First();

            return i + 2;
        }

        private int OutputInstruction(int i, int modeA = 0)
        {
            var a = modeA == 0 ? Instructions[i + 1] : i + 1;
            Outputs.Add(Instructions[a]);

            return i + 2;
        }

        private int JumpIfTrueInstruction(int i, int modeA, int modeB)
        {
            var a = modeA == 0 ? Instructions[i + 1] : i + 1;
            var b = modeB == 0 ? Instructions[i + 2] : i + 2;

            return Instructions[a] != 0 ? Instructions[b] : i + 3;
        }

        private int JumpIfFalseInstruction(int i, int modeA, int modeB)
        {
            var a = modeA == 0 ? Instructions[i + 1] : i + 1;
            var b = modeB == 0 ? Instructions[i + 2] : i + 2;

            return Instructions[a] == 0 ? Instructions[b] : i + 3;
        }

        private int LessThanInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = modeC == 0 ? Instructions[i + 1] : i + 1;
            var b = modeB == 0 ? Instructions[i + 2] : i + 2;
            var c = modeA == 0 ? Instructions[i + 3] : i + 3;
            Instructions[c] = Instructions[a] < Instructions[b] ? 1 : 0;

            return i + 4;
        }

        private int IfEqualsInstruction(int i, int modeB, int modeC, int modeA = 0)
        {
            var a = modeC == 0 ? Instructions[i + 1] : i + 1;
            var b = modeB == 0 ? Instructions[i + 2] : i + 2;
            var c = modeA == 0 ? Instructions[i + 3] : i + 3;
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
    }
}

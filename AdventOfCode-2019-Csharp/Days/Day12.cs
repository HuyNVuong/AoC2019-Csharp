using System;
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
                    Z = int.Parse(matches[2]),
                    Velocity = new Velocity()
                };
            }).ToList();

            var totalEnergies = MoveMoons(moons.Select(x => x.Clone()).ToList(), 1000);
            Console.WriteLine(totalEnergies);

            var steps = GetStepsUntilRepeat(moons.Select(x => x.Clone()).ToList());
            Console.WriteLine(steps);
        }

        private static int MoveMoons(List<Moon> moons, int? steps = null)
        {
            var timeSteps = 0;
            while (steps == null || timeSteps < steps)
            {
                var moonsXVelocity = moons.Select(m => m.X).Rankify().GetGravitationalPull().ToList();
                var moonsYVelocity = moons.Select(m => m.Y).Rankify().GetGravitationalPull().ToList();
                var moonsZVelocity = moons.Select(m => m.Z).Rankify().GetGravitationalPull().ToList();

                for (var i = 0; i < moons.Count; i++)
                {
                    moons[i].Velocity.Dx += moonsXVelocity[i];
                    moons[i].Velocity.Dy += moonsYVelocity[i];
                    moons[i].Velocity.Dz += moonsZVelocity[i];
                    moons[i].Move();
                }

                timeSteps++;
            }

            var totalEnergy = moons.Sum(moon =>
            {
                var pos = Math.Abs(moon.X) + Math.Abs(moon.Y) + Math.Abs(moon.Z);
                var kin = Math.Abs(moon.Velocity.Dx) + Math.Abs(moon.Velocity.Dy) + Math.Abs(moon.Velocity.Dz);

                return pos * kin;
            });

            return totalEnergy;
        }

        private static long GetStepsUntilRepeat(List<Moon> moons)
        {
            var timeSteps = 0;
            var initialState = moons.Select(x => x.Clone()).ToList();
            long? stepsToRepeatX = null;
            long? stepsToRepeatY = null;
            long? stepsToRepeatZ = null;
            do
            {
                var moonsXVelocity = moons.Select(m => m.X).Rankify().GetGravitationalPull().ToList();
                var moonsYVelocity = moons.Select(m => m.Y).Rankify().GetGravitationalPull().ToList();
                var moonsZVelocity = moons.Select(m => m.Z).Rankify().GetGravitationalPull().ToList();

                for (var i = 0; i < moons.Count; i++)
                {
                    moons[i].Velocity.Dx += moonsXVelocity[i];
                    moons[i].Velocity.Dy += moonsYVelocity[i];
                    moons[i].Velocity.Dz += moonsZVelocity[i];
                    moons[i].Move();
                }

                timeSteps++;

                if (stepsToRepeatX == null && moons.Zip(initialState).All(item => item.First != null && item.Second != null
                    && item.First.X == item.Second.X && item.First.Velocity.Dx == item.Second.Velocity.Dx))
                    stepsToRepeatX = timeSteps;
                if (stepsToRepeatY == null && moons.Zip(initialState).All(item => item.First != null && item.Second != null
                    && item.First.Y == item.Second.Y && item.First.Velocity.Dy == item.Second.Velocity.Dy))
                    stepsToRepeatY = timeSteps;
                if (stepsToRepeatZ == null && moons.Zip(initialState).All(item => item.First != null && item.Second != null 
                    && item.First.Z == item.Second.Z && item.First.Velocity.Dz == item.Second.Velocity.Dz))
                    stepsToRepeatZ = timeSteps;
            } while (stepsToRepeatX == null || stepsToRepeatY == null || stepsToRepeatZ == null);
            
            return Utilities.Lcm(stepsToRepeatX.Value, stepsToRepeatY.Value, stepsToRepeatZ.Value);
        }

        private static IEnumerable<double> Rankify(this IEnumerable<int> array)
        {
            var list = array.ToList();
            var n = list.Count;
            var ranks = new List<double>();
            for (var i = 0; i < n; i++)
            {
                var r = 1;
                var s = 1;
                for (var j = 0; j < n; j++)
                {
                    if (j != i && list[j] > list[i]) r += 1;
                    if (j != i && list[j] == list[i]) s += 1;
                }
                ranks.Add(r + 0.5 * (s - 1));
            }

            return ranks;
        }

        private static IEnumerable<int> GetGravitationalPull(this IEnumerable<double> rank)
        {
            var list = rank.ToList();
            var n = list.Count;
            var strengths = new List<int>();
            for (var i = 0; i < n; i++)
            {
                var strength = 0;
                for (var j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    if (list[i] < list[j]) strength--;
                    if (list[i] > list[j]) strength++;
                }
                strengths.Add(strength);
            }

            return strengths;
        }

        private class Moon : ICloneable<Moon>
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public Velocity Velocity { get; set; }

            public void Move()
            {
                X += Velocity.Dx;
                Y += Velocity.Dy;
                Z += Velocity.Dz;
            }

            public Moon Clone()
            {
                return new Moon
                {
                    X = X,
                    Y = Y,
                    Z = Z,
                    Velocity = new Velocity
                    {
                        Dx = Velocity.Dx,
                        Dy = Velocity.Dy,
                        Dz = Velocity.Dz
                    }
                };
            }
        }

        private class Velocity
        {
            public int Dx { get; set; }
            public int Dy { get; set; }
            public int Dz { get; set; }
        }
    }
}

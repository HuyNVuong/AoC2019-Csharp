using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day01
    {
        public static void Solve()
        {
            var fuels = FileReader.ParseDataFromFile<int>("Data/Day01.txt", '\n');

            var fuelRequirement = GetFuelRequirementForModule(fuels);
            Console.WriteLine(fuelRequirement);

            var fuelRequirementWithModuleFuel = GetFuelRequirementForModuleAndFuel(fuels);
            Console.WriteLine(fuelRequirementWithModuleFuel);
        }

        private static int GetFuelRequirementForModule(List<int> fuels)
        {
            var fuelRequirement = fuels.Sum(fuel => fuel / 3 - 2);

            return fuelRequirement;
        }

        private static int GetFuelRequirementForModuleAndFuel(List<int> fuels)
        {
            var fuelRequirement = fuels.Sum(fuel =>
            {
                var requirement = 0;
                while (fuel > 0)
                {
                    fuel = fuel / 3 - 2;
                    requirement += fuel < 0 ? 0 : fuel;
                }
                return requirement;
            });

            return fuelRequirement;
        }
    }
}

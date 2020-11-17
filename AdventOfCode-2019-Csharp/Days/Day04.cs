using System;
using System.Linq;

namespace AdventOfCode_2019_Csharp.Days
{
    public static class Day04
    {
        public static void Solve()
        {
            var passwordCombinations = FindNumberOfPasswordCombinationsInRange(134564, 585159);
            Console.WriteLine(passwordCombinations);

            var strictPasswordCombinations = FindStrictNumberOfPasswordCombinationsInRange(134564, 585159);
            Console.WriteLine(strictPasswordCombinations);
        }

        private static int FindNumberOfPasswordCombinationsInRange(int low, int high)
        {
            var combinations = 0;
            for (var i = low; i < high; i++)
            {
                var number = i.ToString();
                if (HasSameAdjacent(number) && IsIncreasingOrder(number)) combinations++;
            }

            return combinations;
        }

        private static int FindStrictNumberOfPasswordCombinationsInRange(int low, int high)
        {
            var combinations = 0;
            for (var i = low; i < high; i++)
            {
                var number = i.ToString();
                if (HasSameStrictAdjacent(number) && IsIncreasingOrder(number)) combinations++;
            }

            return combinations;
        }

        private static bool HasSameAdjacent(string number)
        {
            for (var i = 1; i < number.Length; i++)
            {
                if (number[i - 1] == number[i]) 
                    return true;
            }

            return false;
        }

        private static bool HasSameStrictAdjacent(string number)
        {
            var numberCount = number.GroupBy(c => c)
                .ToDictionary(grp => grp.Key, grp => grp.Count());
            for (var i = 1; i < number.Length; i++)
            {
                if (number[i - 1] == number[i] && numberCount[number[i - 1]] == 2)
                    return true;
            }

            return false;
        }

        private static bool IsIncreasingOrder(string number)
        {
            for (var i = 1; i < number.Length; i++)
            {
                if (number[i - 1] > number[i]) 
                    return false;
            }

            return true;
        }
    }
}

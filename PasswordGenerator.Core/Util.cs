using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordGenerator.Core
{
    public static class Util
    {
        public static bool ContainsElementFrom<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return first.Intersect(second).Count() > 0;
        }

        public static int GetMinRelativelyPrimeNumber(int number)
        {
            int res = -1;

            for (int i = 2; i<number; i++)
            {
                int[] multipliers = GetDividers(i);

                if (multipliers.All(n => number % n != 0))
                {
                    res = i;
                    break;
                }                    
            }
            return res;
        }

        private static int[] GetDividers(int n)
        {
            var dividers = new List<int>();

            for (int i = 2; i<=n/2; i++)
            {
                if (n % i == 0)
                {
                    dividers.Add(i); 
                }
            }
            if (n>1) dividers.Add(n);

            return dividers.ToArray();
        }        
    }
}

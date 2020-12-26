using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Helpers
{
    public static class RegexHelper
    {
        public static bool Match(
            string input,
            string pattern
        )
        {
            return GetMatch(input, pattern, 0).Success;
        }

        public static int Matches(
            string input,
            string pattern
        )
        {
            return Regex.Matches(input, pattern).Count;
        }
        
        public static bool Match<T1>(
            string input, 
            string pattern,
            out T1 value1,
            bool display = false
        )
        {
            Match m = GetMatch(input, pattern, 1, display);
            if (m.Success)
            {
                value1 = (T1) Convert.ChangeType(m.Groups[1].Value, typeof(T1));
            }
            else
            {
                value1 = default;
            }
            return m.Success;
        }

        public static bool Match<T1, T2>(
            string input, 
            string pattern,
            out T1 value1,
            out T2 value2,
            bool display = false
        )
        {
            Match m = GetMatch(input, pattern, 2, display);
            if (m.Success)
            {
                value1 = (T1) Convert.ChangeType(m.Groups[1].Value, typeof(T1));
                value2 = (T2) Convert.ChangeType(m.Groups[2].Value, typeof(T2));
            }
            else
            {
                value1 = default;
                value2 = default;
            }
            return m.Success;
        }

        public static bool Match<T1, T2, T3>(
            string input, 
            string pattern,
            out T1 value1,
            out T2 value2,
            out T3 value3,
            bool display = false
        )
        {
            Match m = GetMatch(input, pattern, 3, display);
            if (m.Success)
            {
                value1 = (T1) Convert.ChangeType(m.Groups[1].Value, typeof(T1));
                value2 = (T2) Convert.ChangeType(m.Groups[2].Value, typeof(T2));
                value3 = (T3) Convert.ChangeType(m.Groups[3].Value, typeof(T3));
            }
            else
            {
                value1 = default;
                value2 = default;
                value3 = default;
            }
            return m.Success;
        }

        public static bool Match<T1, T2, T3, T4>(
            string input,
            string pattern,
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            bool display = false
        )
        {
            Match m = GetMatch(input, pattern, 4, display);
            if (m.Success)
            {
                value1 = (T1) Convert.ChangeType(m.Groups[1].Value, typeof(T1));
                value2 = (T2) Convert.ChangeType(m.Groups[2].Value, typeof(T2));
                value3 = (T3) Convert.ChangeType(m.Groups[3].Value, typeof(T3));
                value4 = (T4) Convert.ChangeType(m.Groups[4].Value, typeof(T4));
            }
            else
            {
                value1 = default;
                value2 = default;
                value3 = default;
                value4 = default;
            }

            return m.Success;
        }

        public static bool Match<T1, T2, T3, T4, T5>(
            string input,
            string pattern,
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            out T5 value5,
            bool display = false
        )
        {
            Match m = GetMatch(input, pattern, 5, display);
            if (m.Success)
            {
                value1 = (T1) Convert.ChangeType(m.Groups[1].Value, typeof(T1));
                value2 = (T2) Convert.ChangeType(m.Groups[2].Value, typeof(T2));
                value3 = (T3) Convert.ChangeType(m.Groups[3].Value, typeof(T3));
                value4 = (T4) Convert.ChangeType(m.Groups[4].Value, typeof(T4));
                value5 = (T5) Convert.ChangeType(m.Groups[5].Value, typeof(T5));
            }
            else
            {
                value1 = default;
                value2 = default;
                value3 = default;
                value4 = default;
                value5 = default;
            }
            return m.Success;
        }

        public static Match GetMatch(string input, string pattern, int count, bool display = false)
        {
            Match m = Regex.Match(input, pattern);
            if (!m.Success)
            {
                if (display)
                {
                    Console.WriteLine($"Pattern {pattern} did not match input {input}");
                }
                return m;
            }
            else if (m.Groups.Count != count+1)
            {
                throw new ArgumentException($"Pattern {pattern} did not match {count} groups in {input}");
            }
            return m;
        }
    }
}
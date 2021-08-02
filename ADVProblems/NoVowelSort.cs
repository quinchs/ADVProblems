using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADVProblems
{
    public static class NoVowelSort
    {
        public static readonly char[] Vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

        public static IOrderedEnumerable<string> Sort(IEnumerable<string> input)
        {
            return input.OrderBy(x =>
            {
                string s = x;
                foreach (var vowel in Vowels)
                    s = s.Replace($"{vowel}", string.Empty);
                return s;
            });
        }

        public static void SolveAndDisplay()
        {
            var input = new string[]
            {
                "once", "upon", "abc", "time", "there", "were", "some", "words"
            };

            var result = Sort(input);

            Console.ForegroundColor = ConsoleColor.Green;
            LogUtils.CreateTitle($"No vowel Sort");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Input:");

            Console.WriteLine("[\n" + string.Join("\u001b[37m,\n", input.Select(x => $"  \u001b[38;5;215m\"{x}\"")) + "\u001b[37m\n]");

            Console.WriteLine("\nResult\n");

            Console.WriteLine("[\n" + string.Join("\u001b[37m,\n", result.Select(x => $"  \u001b[38;5;215m\"{x}\"")) + "\u001b[37m\n]\n\n");
        }
    }
}

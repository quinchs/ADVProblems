
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADVProblems
{
    public static class BookClub
    {
        // since their can be the same book (key) in the collection, we have to use a tuple array

        public static IOrderedEnumerable<(string book, IEnumerable<string> friends)> Solve(IEnumerable<(string book, string friend)> input)
        {
            List<(string book, List<string> friends)> result = new List<(string book, List<string> friends)>();

            foreach(var pair in input)
            {
                // find a book that this friend may have not read

                var books = input.Where(x => x.friend != pair.friend && !input.Any(y => y.book == x.book && y.friend == pair.friend)).Select(x => x.book).Distinct().ToArray();

                foreach(var book in books)
                {
                    var resultItem = result.FirstOrDefault(x => x.book == book);

                    if (resultItem != default((string book, List<string> friends)))
                    {
                        if(!resultItem.friends.Contains(pair.friend))
                            resultItem.friends.Add(pair.friend);
                    }
                    else
                    {
                        result.Add((book, new() { pair.friend }));
                    }
                }
            }

            return result.Select(x => (x.book, (IEnumerable<string>)x.friends)).OrderBy(x => x.book);
        }

        public static void SolveAndDisplay()
        {
            (string book, string friend)[] arr = new[]
            {
                ("Pride and Prejudice", "Jenny"),
                ("A Tale of Two Cities", "Mark"),
                ("Magician", "Jenny"),
                ("The Lord of the Rings", "Pavel"),
                ("Magician", "Pavel")
            };

            var result = Solve(arr);

            LogUtils.CreateTitle("Book sort");

            Console.WriteLine("Input:");
            Console.WriteLine("{");

            Console.WriteLine(string.Join(",\n", arr.Select(x => $"  {{ \u001b[38;5;45m\"{x.book}\": \u001b[38;5;215m\"{x.friend}\" \u001b[37m}}")));

            Console.WriteLine("}\n\nResult\n\n{");

            Console.WriteLine(string.Join(",\n", result.Select(x => $"  {{ \u001b[38;5;45m\"{x.book}\": \u001b[37m[ {string.Join(", ", x.friends.Select(y => $"\u001b[38;5;215m\"{y}\""))} \u001b[37m] }}")) + "\n}\n\n");

        }
    }
}

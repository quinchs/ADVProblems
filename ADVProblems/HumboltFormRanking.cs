using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADVProblems
{
    public class Thread
    {
        public string Title { get; set; }

        public List<string> Tags { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public ulong Upvotes { get; set; }
    }

    public static class HumboltFormRanking
    {
        public static IOrderedEnumerable<(string author, ulong totalUpvotes, string formRanking)> AuthorRankings(IEnumerable<Thread> threads)
        {
            List<(string author, ulong totalUpvotes, string formRanking)> results = new();

            foreach(var thread in threads)
            {
                foreach(var post in thread.Posts)
                {
                    var ranking = results.FirstOrDefault(x => x.author == post.Author);

                    if(ranking != default((string author, ulong totalUpvotes, string formRanking)))
                    {
                        ranking.totalUpvotes += post.Upvotes;
                        ranking.formRanking = GetRanking(ranking.totalUpvotes);
                    }
                    else
                    {
                        results.Add((post.Author, post.Upvotes, GetRanking(post.Upvotes)));
                    }
                }
            }

            return (from result in results
                    orderby result.totalUpvotes descending, result.author ascending
                    select result);
        }

        private static string GetRanking(ulong upvotes)
        {
            if(upvotes == 0)
            {
                return "Insignificantly Evil";
            }
            
            if(upvotes < 20)
            {
                return "Cautiosly Evil";
            }

            if(upvotes < 100)
            {
                return "Justifiably Evil";
            }

            if(upvotes < 500) 
            {
                return "Wickedly Evil";
            }

            return "Diabolically Evil";
        }

        public static void SolveAndDisplay()
        {
            var arr = new Thread[]
            {
                new Thread()
                {
                    Title = "Invade Manhatten, anyone?",
                    Tags = new() { "world-domination", "hangout" },
                    Posts = new()
                    {
                        new Post()
                        {
                            Author = "Mr. Sinister",
                            Content = "I'm thinking 9 pm?",
                            Upvotes = 2,
                        },
                        new Post()
                        {
                            Author = "Mystique",
                            Content = "Sounds fun!",
                            Upvotes = 0,
                        },
                        new Post()
                        {
                            Author = "Magneto",
                            Content = "I'm in!",
                            Upvotes = 0,
                        }
                    }
                }
            };

            var rankings = AuthorRankings(arr).ToArray();

            Console.ForegroundColor = ConsoleColor.Green;
            LogUtils.CreateTitle($"Form Ranking");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Input:\n");

            Console.WriteLine($"[\n{string.Join("\u001b[37m,\n", arr.Select(thread => $"  \u001b[37m{{\n    {LogUtils.JsonKeyValue("title", thread.Title)},\n    {LogUtils.JsonKeyValue("tags", thread.Tags, 6)},\n    {LogUtils.ObjectArray("posts", thread.Posts, 6)}\n  \u001b[37m}}"))}\n]");

            Console.WriteLine("\nOutput:\n");

            Console.WriteLine($"[\n  {string.Join($"\u001b[37m,\n{" ".PadLeft(2)}", rankings.Select(x => $"\u001b[37m(\u001b[38;5;45m\"{x.author}\"\u001b[37m, \u001b[38;5;48m{x.totalUpvotes}\u001b[37m, \u001b[38;5;45m\"{x.formRanking}\"\u001b[37m)"))}\n]");
        }
    }
}

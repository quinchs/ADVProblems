using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ADVProblems
{
    public static class MarkovTextGeneration
    {
        public static readonly string FileDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "MarkovData";

        public static string Generate(string startToken, CancellationToken token = default, params string[] files)
        {
            var random = new Random();
            string currentToken = startToken;

            List<string> generatedTokens = new() { currentToken };

            List<string> tokens = new();

            foreach(var file in files)
            {
                if (File.Exists(FileDirectory + Path.DirectorySeparatorChar + file))
                    tokens.AddRange(File.ReadAllText(FileDirectory + Path.DirectorySeparatorChar + file).Split(' '));
            }

            while (true)
            {
                if (token.IsCancellationRequested)
                    throw new OperationCanceledException();

                var possibleNextTokens = tokens.Select((x, i) => x == currentToken ? i : -1).Where(x => x != -1).ToArray();

                if (possibleNextTokens.Length == 0)
                    break; // 2. there is no next possible token

                var nextTokenIndex = random.Next(possibleNextTokens.Length);

                var indx = possibleNextTokens[nextTokenIndex];

                if (tokens.Count == indx + 1)
                    break;

                currentToken = tokens[indx + 1];
                generatedTokens.Add(currentToken);

                if (currentToken == "." || generatedTokens.Count >= 200)
                    break; // 1. the previous token was a full stop | 3. the produced sentence is 200 tokens in length
            }

            return Regex.Replace(string.Join(" ", generatedTokens), @"\s(\W)", (m) => m.Groups[1].Value);
        }

        public static void SolveAndDisplay()
        {
            LogUtils.CreateTitle("Markov Generator");

            Console.WriteLine("Generating...");

            for (int i = 0; i != 4; i++)
                Console.WriteLine(Generate("There", files: "single.txt"));

            Console.WriteLine($"\n=-=-=-=-=-=-=-\n");

            for (int i = 0; i != 4; i++)
                Console.WriteLine(Generate("the", files: "jab.txt"));

            Console.WriteLine($"\n=-=-=-=-=-=-=-\n");

            for (int i = 0; i != 4; i++)
                Console.WriteLine(Generate("It", files: new[] { "dracula.txt", "pandp.txt" }));

            Console.WriteLine($"\n=-=-=-=-=-=-=-\n");

            for (int i = 0; i != 4; i++)
                Console.WriteLine(Generate("Once", files: new[] { "dracula.txt", "pandp.txt", "jb.txt", "totc.txt" }));

            Console.WriteLine($"\n=-=-=-=-=-=-=-\n");

            for (int i = 0; i != 4; i++)
                Console.WriteLine(Generate("cat", files: new[] { "single.txt", "textwraps.txt" }));

            Console.WriteLine($"\n=-=-=-=-=-=-=-\n");

            Console.WriteLine("Complete");
        }
    }
}

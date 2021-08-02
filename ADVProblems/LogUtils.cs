using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADVProblems
{
    public static class LogUtils
    {
        public static event EventHandler OnResise;

        private static int Width;
        private static int Height;


        static LogUtils()
        {
            Width = Console.BufferWidth;
            Height = Console.BufferHeight;
            _ = Task.Run(() =>
            {
                while (!Process.GetCurrentProcess().HasExited)
                {
                    if(Console.BufferHeight != Height || Console.BufferWidth != Width)
                    {
                        Width = Console.BufferWidth;
                        Height = Console.BufferHeight;

                        OnResise?.Invoke(null, null);
                    }
                }
            });
        }

        public static void CreateTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\u001b[4m{$"-= {title} =-".PadLeft(Console.BufferWidth / 2 + (title.Length + 6) / 2).PadRight(Console.BufferWidth)}\u001b[0m\n");
            Console.ForegroundColor = ConsoleColor.White;

            var pos = Console.GetCursorPosition();

            pos.Top -= 2;

            OnResise += (o, s) =>
            {
                var curPos = Console.GetCursorPosition();
                Console.SetCursorPosition(0, pos.Top);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\u001b[4m{$"-= {title} =-".PadLeft(Console.BufferWidth / 2 + (title.Length + 6) / 2).PadRight(Console.BufferWidth)}\u001b[0m\n");
                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(0, curPos.Top);
            };

        }



        public static string JsonKeyValue(string key, string value)
        {
            return $"\u001b[38;5;45m\"{key}\"\u001b[37m: \u001b[38;5;215m\"{value}\"\u001b[37m";
        }

        public static string JsonKeyValue(string key, IEnumerable<string> value, int padding)
        {
            return $"\u001b[38;5;45m\"{key}\"\u001b[37m: {Array(value, padding)}\u001b[37m";
        }

        public static string Array(IEnumerable<string> value, int padding)
        {
            return $"\u001b[37m[\n{" ".PadLeft(padding)}{string.Join($",\n{" ".PadLeft(padding)}", value.Select(x => $"\u001b[38;5;215m\"{x}\"\u001b[37m"))}\n{" ".PadLeft(padding - 2)}]";
        }

        public static string ObjectArray<TElement>(string name, IEnumerable<TElement> value, int padding) 
        {
            return $"\u001b[38;5;45m\"{name}\"\u001b[37m: {ObjectArray(value, padding)}";
        }

        public static string ObjectArray<TElement>(IEnumerable<TElement> value, int padding)
        {
            List<string> objects = new();

            foreach (var item in value)
                objects.Add(Object(item, padding));

            return $"[\n{string.Join(",\n", objects)}\n{" ".PadLeft(padding - 2)}]";
        }

        public static string Object<TElement>(TElement value, int padding)
        {
            var type = typeof(TElement);

            List<string> props = new();

            foreach(var prop in type.GetProperties())
            {
                props.Add($"{" ".PadLeft(padding + 2)}" + JsonKeyValue(prop.Name.ToLower(), prop.GetValue(value).ToString()));
            }

            return $"{" ".PadLeft(padding)}{{\n{string.Join(",\n", props)}\n{" ".PadLeft(padding)}}}";
        }

        public static string Indent(string val, int padding)
        {
            return $"{val.Replace("\n", $"\n{" ".PadLeft(padding)}")}";
        }


    }
}

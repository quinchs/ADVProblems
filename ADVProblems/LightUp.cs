using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADVProblems
{
    public class Tile
    {
        public bool IsLamp { get; set; } = false;

        public bool IsBlack { get; set; } = false;

        public int RequiredLights { get; set; } = -1;

        public int X { get; set; }

        public int Y { get; set; }

        public Board Board { get; set; }

        public Tile[] NorthCells => Column.Where(x => x.Y < Y).OrderByDescending(x => x.Y).ToArray();
        public Tile[] SouthCells => Column.Where(x => x.Y > Y).OrderBy(x => x.Y).ToArray();
        public Tile[] EastCells => Row.Where(x => x.X > X).OrderBy(x => x.X).ToArray();
        public Tile[] WestCells => Row.Where(x => x.X < X).OrderByDescending(x => x.X).ToArray();

        public Tile[] Row
        {
            get
            {
                Tile[] arr = new Tile[Board.Height];
                for (int i = 0; i < Board.Height; i++)
                {
                    arr[i] = Board.Tiles[i, Y];
                }

                return arr;
            }
        }

        public Tile[] Column
        {
            get
            {
                Tile[] arr = new Tile[Board.Width];
                for (int i = 0; i < Board.Width; i++)
                {
                    arr[i] = Board.Tiles[X, i];
                }

                return arr;
            }
        }

        public bool IsValidNumberedSquare()
        {
            if (this.IsLamp)
                return false;

            if (this.RequiredLights == -1)
                return false;

            int lampCount = 0;

            if (EastCells.Any(x => x.IsLamp) || WestCells.Any(x => x.IsLamp) || NorthCells.Any(x => x.IsLamp) || SouthCells.Any(x => x.IsLamp))
            {
                if (EastCells.FirstOrDefault()?.IsLamp ?? false)
                    lampCount++;
                if (WestCells.FirstOrDefault()?.IsLamp ?? false)
                    lampCount++;
                if (NorthCells.FirstOrDefault()?.IsLamp ?? false)
                    lampCount++;
                if (SouthCells.FirstOrDefault()?.IsLamp ?? false)
                    lampCount++;


                return lampCount <= this.RequiredLights;
            }
            else return true;
        }

        public bool IsLitUp()
        {
            if (this.IsLamp)
                return true;

            if (this.IsBlack)
                return false;

            if (EastCells.Any(x => x.IsLamp) || WestCells.Any(x => x.IsLamp) || NorthCells.Any(x => x.IsLamp) || SouthCells.Any(x => x.IsLamp))
            {
                if (EastCells.Any(x => x.IsLamp))
                {
                    var blackTile = EastCells.FirstOrDefault(x => x.IsBlack);

                    if (blackTile == null)
                        return true;
                    else if(blackTile.X > EastCells.FirstOrDefault(x => x.IsLamp).X)
                        return true;
                }

                if (WestCells.Any(x => x.IsLamp))
                {
                    var blackTile = WestCells.FirstOrDefault(x => x.IsBlack);

                    if (blackTile == null)
                        return true;
                    else if (blackTile.X < WestCells.FirstOrDefault(x => x.IsLamp).X)
                        return true;
                }

                if (NorthCells.Any(x => x.IsLamp))
                {
                    var blackTile = NorthCells.FirstOrDefault(x => x.IsBlack);

                    if (blackTile == null)
                        return true;
                    else if (blackTile.Y < NorthCells.FirstOrDefault(x => x.IsLamp).Y)
                        return true;
                }

                if (SouthCells.Any(x => x.IsLamp))
                {
                    var blackTile = SouthCells.FirstOrDefault(x => x.IsBlack);

                    if (blackTile == null)
                        return true;
                    else if (blackTile.Y > SouthCells.FirstOrDefault(x => x.IsLamp).Y)
                        return true;
                }

                return false;
            }

            return false;
        }

        public bool IsLegalLamp()
        {
            var rowEast = Row.Where(x => x.X > X).OrderBy(x => x.X);
            var rowWest = Row.Where(x => x.X < X).OrderByDescending(x => x.X);
            var columnNorth = Column.Where(x => x.Y < Y).OrderByDescending(x => x.Y);
            var columnSouth = Column.Where(x => x.Y > Y).OrderBy(x => x.Y);

            foreach(var item in rowEast)
            {
                if (item.IsBlack)
                    break;

                if (item.IsLamp)
                    return false;
            }

            foreach (var item in rowWest)
            {
                if (item.IsBlack)
                    break;

                if (item.IsLamp)
                    return false;
            }
            foreach (var item in columnNorth)
            {
                if (item.IsBlack)
                    break;

                if (item.IsLamp)
                    return false;
            }
            foreach (var item in columnSouth)
            {
                if (item.IsBlack)
                    break;

                if (item.IsLamp)
                    return false;
            }

            return true;
        }

        public static Tile Lamp(Board b, int x, int y) => new Tile(b, x, y, true);
        public static Tile Black(Board b, int x, int y) => new Tile(b, x, y,true, -1);
        public static Tile White(Board b, int x, int y) => new Tile(b, x, y, false, -1);

        public static Tile BlackNumber(Board b, int x, int y, int num) => new Tile(b, x, y, true, num);

        public Tile(Board board, int x, int y, bool isBlack, int requiredLights)
        {
            this.Board = board;
            this.X = x;
            this.Y = y;

            this.IsBlack = isBlack;
            this.RequiredLights = requiredLights;
        }

        public Tile(Board board, int x, int y, bool isLamp)
        {
            this.Board = board;
            this.X = x;
            this.Y = y;

            this.IsLamp = true;
        }
    }

    public class Board
    {
        public readonly int Width;

        public readonly int Height;

        public readonly Tile[,] Tiles;

        public Tile[] Lamps
        {
            get
            {
                List<Tile> lamps = new();

                for (int y = 0; y != Tiles.GetLength(1); y++)
                {
                    for (int x = 0; x != Tiles.GetLength(0); x++)
                    {
                        if (Tiles[x, y].IsLamp)
                            lamps.Add(Tiles[x, y]);
                    }
                }

                return lamps.ToArray();
            }
        }

        public Tile[] NumberedSquares
        {
            get
            {
                List<Tile> s = new();

                for (int y = 0; y != Tiles.GetLength(1); y++)
                {
                    for (int x = 0; x != Tiles.GetLength(0); x++)
                    {
                        if (Tiles[x, y].RequiredLights >= 0)
                            s.Add(Tiles[x, y]);
                    }
                }

                return s.ToArray();
            }
        }

        public Tile[] BlackSquares
        {
            get
            {
                List<Tile> s = new();

                for (int y = 0; y != Tiles.GetLength(1); y++)
                {
                    for (int x = 0; x != Tiles.GetLength(0); x++)
                    {
                        if (Tiles[x, y].RequiredLights == -1 && Tiles[x, y].IsBlack)
                            s.Add(Tiles[x, y]);
                    }
                }

                return s.ToArray();
            }
        }

        public Tile[] WhiteSquares
        {
            get
            {
                List<Tile> white = new();

                for (int y = 0; y != Tiles.GetLength(1); y++)
                {
                    for (int x = 0; x != Tiles.GetLength(0); x++)
                    {
                        if (!Tiles[x, y].IsBlack && !Tiles[x, y].IsLamp)
                            white.Add(Tiles[x, y]);
                    }
                }

                return white.ToArray();
            }
        }

        /// <summary>
        ///     Creates a new board based on the provided tile arrangement.
        ///     Board strings are defined as:<br/>
        ///     X = Solid black square<br/>
        ///     0-9 = Black square with number<br/>
        ///     L = Lamp<br/>
        ///     . = White space<br/><br/>
        ///     Heres an example board:<br/>
        ///     ...1.0.<br/>
        ///     X......<br/>
        ///     ..X.X..<br/>
        ///     X...L.X<br/>
        ///     ..X.3..<br/>
        ///     .L....X<br/>
        ///     L3L2...<br/>
        /// </summary>
        /// <param name="boardString"></param>
        public Board(string boardString, int w, int h)
        {
            Tiles = new Tile[w, h];

            this.Width = w;
            this.Height = h;

            for(int y = 0; y != h; y++)
            {
                for(int x = 0; x != w; x++)
                {
                    Tiles[x, y] = boardString.ElementAt((h * y) + x) switch
                    {
                        '.' => Tile.White(this, x, y),
                        'X' => Tile.Black(this, x, y),
                        'L' => Tile.Lamp(this, x, y),
                        char d => Tile.BlackNumber(this, x, y,int.Parse($"{d}"))
                    };
                }
            }
        }
    }

    public enum BoardState
    {
        Happy,
        Unhappy,
        Solved
    }

    public static class LightUp
    {
        public static BoardState GetBoardState(Board board)
        {
            // check the lamp rules
            foreach (var lamp in board.Lamps)
            {
                if (!lamp.IsLegalLamp())
                    return BoardState.Unhappy;
            }

            // check the black numbered cells
            foreach(var numberedCell in board.NumberedSquares)
            {
                if(!numberedCell.IsValidNumberedSquare())
                    return BoardState.Unhappy;
            }

            if (board.WhiteSquares.All(x => x.IsLitUp()))
                return BoardState.Solved;

            return BoardState.Happy;
        }

        public static void SolveAndDisplay()
        {
            Console.ResetColor();

            LogUtils.CreateTitle("LightUp");

            var board1 = new Board("...1.0." +
                                   "X......" +
                                   "..X.X.." +
                                   "X...L.X" +
                                   "..X.3.." +
                                   ".L....X" +
                                   "L3L2...", 7,7);

            var board2 = new Board("..L1.0." +
                                   "X...L.." +
                                   "L.X.X.L" +
                                   "X...L.X" +
                                   "..XL3L." +
                                   ".L....X" +
                                   "L3L2L..", 7,7);

            var board3 = new Board("L..1L0." +
                                   "X.L...." +
                                   "L.X.X.L" +
                                   "X...L.X" +
                                   "..XL3L." +
                                   ".L....X" +
                                   "L3L2L..", 7,7);

            var board4 = new Board("L1.L." +
                                   "..L3L" +
                                   "..X1." +
                                   ".1..." +
                                   ".....",5,5);

            var board1State = GetBoardState(board1);
            var board2State = GetBoardState(board2);
            var board3State = GetBoardState(board3);
            var board4State = GetBoardState(board4);

            Console.WriteLine($"Board One: {board1State}");
            WriteBoard(board1);
            Console.WriteLine($"\n\nBoard Two: {board2State}");
            WriteBoard(board2);

            Console.WriteLine($"\n\nBoard Three: {board3State}");
            WriteBoard(board3);

            Console.WriteLine($"\n\nBoard Four: {board4State}");
            WriteBoard(board4);
        }

        public static void WriteBoard(Board board)
        {
            ConsoleColor[,] color = new ConsoleColor[board.Width, board.Height];

            // draw the outline
            var pos = Console.GetCursorPosition();
            Console.WriteLine("+-".PadRight(board.Width + 1, '-') + "+");
            for (int i = 0; i != board.Height; i++)
            {
                Console.WriteLine("|".PadRight(board.Width + 1) + "|");
            }
            Console.WriteLine("+-".PadRight(board.Width + 1, '-') + "+");


            Console.Write("\u001b[38;5;232m\u001b[48;5;227m");

            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    var tile = board.Tiles[x, y];

                    if (tile.IsLitUp())
                    {
                        Console.SetCursorPosition(pos.Left + 1 + x, pos.Top + 1 + y);
                        Console.Write($"\u001b[38;5;232m\u001b[48;5;227m ");
                    }

                    if (tile.IsLamp)
                    {
                        Console.SetCursorPosition(pos.Left + 1 + x, pos.Top + 1 + y);
                        Console.Write($"\u001b[38;5;232mL");
                    }

                    if (!tile.IsBlack && !tile.IsLitUp())
                    {
                        Console.SetCursorPosition(pos.Left + 1 + x, pos.Top + 1 + y);
                        Console.Write($"\u001b[38;5;232m\u001b[48;5;15m ");
                    }

                    if (tile.RequiredLights != -1)
                    {
                        Console.SetCursorPosition(pos.Left + 1 + x, pos.Top + 1 + y);
                        Console.Write($"\u001b[38;5;15m\u001b[48;5;232m{tile.RequiredLights}");
                    }
                }
            }
            Console.ResetColor();
            Console.Write("\n");
        }
    }
}

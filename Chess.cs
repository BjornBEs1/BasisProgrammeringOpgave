/*
 * File: Chess.cs
 * File Created: 11 Sep 2026
 * Author: BjornBEs
 * -----
 * Last Modified: 11 Sep 2026
 * Modified By: BjornBEs
 * -----
 */

using System.Text;

namespace BasisProgrammeringOpgave
{
    [Flags]
    enum PieceInfo : uint
    {
        TeamWhite = 0b000_000_0,
        TeamBlack = 0b000_000_1,
        PieceTypeEmpty = 0b000_000_0,
        PieceTypePawn = 0b000_001_0,
        PieceTypeKnight = 0b000_010_0,
        PieceTypeBishop = 0b000_011_0,
        PieceTypeRook = 0b000_100_0,
        PieceTypeQueen = 0b000_101_0,
        PieceTypeKing = 0b000_110_0,
        PieceTypeMask = 0b000_111_0,
    }
    enum Command
    {
        None,
        Move,
        Print,
        Retry,
        Quit,
        Restart,
    }
    public static class Chess
    {
        /// <summary>
        /// Is the user playing aginst an AI or another user
        /// </summary>
        static bool aginstAi = false;

        /// <summary>
        /// Use text based input (true) or use keybindings (false)
        /// </summary>
        static bool textBasedInput = true;

        /// <summary>
        /// The borad
        /// </summary>
        static char[,] borad;

        /// <summary>
        /// a bitfield for piece info bc we can't use objects... fuck!!!!!
        /// </summary>
        static PieceInfo[,] boradInfo;

        static bool firstTime;

        /// <summary>
        /// Starts the chess game
        /// </summary>
        public static void StartChessGame()
        {
            // First start the output encoding to use UTF-8 so we can you unicode charaters
            Console.OutputEncoding = Encoding.UTF8;

            // initializing the board and pieces
            borad = new char[8, 8]
            {
                { '♖', '♘', '♗', '♕', '♔', '♗', '♘', '♖' },
                { '♙', '♙', '♙', '♙', '♙', '♙', '♙', '♙' },
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { '♙', '♙', '♙', '♙', '♙', '♙', '♙', '♙' },
                { '♖', '♘', '♗', '♕', '♔', '♗', '♘', '♖' },
            };
            boradInfo = new PieceInfo[8, 8]
            {
                { PieceInfo.PieceTypeRook, PieceInfo.PieceTypeKnight, PieceInfo.PieceTypeBishop, PieceInfo.PieceTypeQueen, PieceInfo.PieceTypeKing, PieceInfo.PieceTypeBishop, PieceInfo.PieceTypeKnight, PieceInfo.PieceTypeRook },
                { PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn },
                { PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty },
                { PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty },
                { PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty },
                { PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty, PieceInfo.PieceTypeEmpty },
                { PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn, PieceInfo.PieceTypePawn },
                { PieceInfo.PieceTypeRook, PieceInfo.PieceTypeKnight, PieceInfo.PieceTypeBishop, PieceInfo.PieceTypeQueen, PieceInfo.PieceTypeKing, PieceInfo.PieceTypeBishop, PieceInfo.PieceTypeKnight, PieceInfo.PieceTypeRook },
            };

            for (int y = 0; y < boradInfo.GetLength(0); y++)
            {
                for (int x = 0; x < boradInfo.GetLength(1); x++)
                {
                    if (y == 0 || y == 1)
                    {
                        boradInfo[y, x] |= PieceInfo.TeamBlack;
                    }
                    if (y == 6 || y == 7)
                    {
                        boradInfo[y, x] |= PieceInfo.TeamWhite;
                    }
                }
            }

            Console.WriteLine("Welcome to 2d chess - by Bjorn");
            Console.WriteLine("This game can be played aginst vs an AI or vs another player");
            Console.WriteLine("Write 'AI' or 1 to fight aginst an AI");
            Console.WriteLine("Write 'Player' or 2 to fight aginst another player");

            while (true)
            {
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int value))
                {
                    value = 0;
                }

                if (value == 1 || input.StartsWith("ai"))
                {
                    aginstAi = true;
                    break;
                }
                else if (value == 2 || input.StartsWith("player"))
                {
                    aginstAi = false;
                    break;
                }
                else
                {
                    continue;
                }
            }

            Console.Clear();

            while (true)
            {
                PrintBoard();

                string data = GetInput(out Command cmd);
            }
        }

        static void GetInputPrintHelp()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("h - help menu");
            Console.WriteLine("m - move a piece");
            Console.WriteLine("p - print the board");
            Console.WriteLine("s - switch input system");
            Console.WriteLine("q - quit the game");
            Console.WriteLine("r - restart the game");
        }

        static string GetInput(out Command cmd)
        {
            if (textBasedInput)
            {
                while (true)
                {
                    string command = Console.ReadLine();
                    if (command.Length > 1)
                    {
                        Console.WriteLine("Invalid input try again");
                        continue;
                    }

                    switch (command)
                    {
                        case "h":
                            GetInputPrintHelp();
                            continue;
                        case "m":
                            cmd = Command.Move;
                            Console.Write("Move piece in Algebraic notation:");
                            string piece = Console.ReadLine();
                            if (char.IsLetter(piece, 0))
                            {

                            }
                            return "";
                        case "p":
                            cmd = Command.Print;
                            return "";
                        case "s":
                            cmd = Command.Retry;
                            return "";
                        case "q":
                            cmd = Command.Quit;
                            return "";
                        case "r":
                            cmd = Command.Restart;
                            return "";
                        default:
                            Console.WriteLine("Invalid input try again");
                            continue;
                    }
                }
            }
            else
            {
                // TODO:
            }
            cmd = Command.None;
            return "";
        }

        static void PrintBoard()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < boradInfo.GetLength(0); y++)
            {
                Console.Write($"{y + 1} "); // Row label

                for (int x = 0; x < boradInfo.GetLength(1); x++)
                {
                    if ((x + y) % 2 == 1) // black background for uneven, white for even.
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    PieceInfo piece = boradInfo[y, x];
                    if ((piece & PieceInfo.PieceTypeMask) == PieceInfo.PieceTypeEmpty)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write(borad[y, x] + " ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h"); // column labels
        }
    }
}

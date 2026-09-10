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
    public static class Chess
    {
        // ♖ ♘ ♗ ♕ ♔ ♙
        static char[,] borad;
        // bit field for pice info
        static PieceInfo[,] boradInfo;
        public static void StartChessGame()
        {
            Console.OutputEncoding = Encoding.UTF8;
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
            string input = Console.ReadLine();

            bool aginst = false;
            if (!int.TryParse(input, out int value))
            {
            }

            while (true)
            {
                PrintBoard();

                GetInput();
            }
        }

        static void GetInput()
        {

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

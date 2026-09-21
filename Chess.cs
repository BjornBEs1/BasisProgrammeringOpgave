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
using static System.Collections.Specialized.BitVector32;

// Reader discretion is advised there is a LOT of os deving tricks in here like Bit manipulation and math.

namespace BasisProgrammeringOpgave
{
    [Flags]
    enum PieceInfo : uint
    {
        None = 0x0000,
        King = 0x0001,
        Pawn = 0x0002,
        Knight = 0x0003,
        Bishop = 0x0004,
        Rook = 0x0005,
        Queen = 0x0006,
        Mask = 0x0007,
        White = 0x0008,
        Black = 0x0010,
        FirstMove = 0x0020,
        CanCastle = 0x0040,
        CanEnPassant = 0x0080,
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
    [Flags]
    enum Tile
    {
        None = 0x00,
        A = 0x01,
        B = 0x02,
        C = 0x03,
        D = 0x04,
        E = 0x05,
        F = 0x06,
        G = 0x07,
        H = 0x08,
        _1 = 0x10,
        _2 = 0x20,
        _3 = 0x30,
        _4 = 0x40,
        _5 = 0x50,
        _6 = 0x60,
        _7 = 0x70,
        _8 = 0x80,
    }
    public static class Chess
    {
        /// <summary>
        /// Indicates that the game is being played.
        /// </summary>
        static bool isPlaying = false;

        /// <summary>
        /// Is the user playing against an AI or another user
        /// </summary>
        static bool againstAi = false;

        /// <summary>
        /// Use text based input (true) or use keybindings (false)
        /// </summary>
        static bool textBasedInput = true;

        /// <summary>
        /// The board
        /// </summary>
        static char[,] board;

        /// <summary>
        /// a bitfield for piece info bc we can't use objects... fuck!!!!!
        /// </summary>
        static PieceInfo[,] boardInfo;

        static PieceInfo moveTurn;

        /// <summary>
        /// The current move number. Starts at 1 and increments after Black's move. A game on move 20 with White to play shows '20'.
        /// </summary>
        static int moveNumber = 1;

        /// <summary>
        /// Counts moves since the last pawn move or capture. Used to enforce the 50-move draw rule. Resets to 0 after any pawn move or capture.
        /// </summary>
        static int halfMoveClock = 0;

        static int cursorX = 0;
        static int cursorY = 0;

        static Tile selectTile;
        static bool tileSelected;

        const string StartFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        /// <summary>
        /// Starts the chess game
        /// </summary>
        public static void Start()
        {
            initializeGame();
            startChessGame();
            Console.Clear();
        }

        static void initializeGame()
        {
            // First start the output encoding to use UTF-8 so we can you unicode characters
            Console.OutputEncoding = Encoding.UTF8;

            // initializing variables
            moveTurn = PieceInfo.White;
            moveNumber = 1;
            halfMoveClock = 0;

            // initializing the board and pieces
            board = new char[8, 8];
            boardInfo = new PieceInfo[8, 8];

            Console.WriteLine("Welcome to 2d chess - by Bjorn");
            Console.WriteLine("This game can be played against vs an AI or vs another player");
            Console.WriteLine("Write 'AI' or 1 to fight against an AI");
            Console.WriteLine("Write 'Player' or 2 to fight against another player");

            while (true)
            {
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int value))
                {
                    continue;
                }

                if (value == 1 || input.StartsWith("ai"))
                {
                    againstAi = true;
                    break;
                }
                else if (value == 2 || input.StartsWith("player"))
                {
                    againstAi = false;
                    break;
                }
                else
                {
                    continue;
                }
            }

            Console.Clear();
            loadPositionFromFen(/*"5k2/2p5/8/1P6/8/4P1P1/3P3P/R1N1KN1R b KQ - 20 50"*/ StartFEN);
        }

        /// <summary>
        /// Enters the main game loop. Continues until <see cref="isPlaying"/> is false.
        /// The loop prints the board, reads and parses user input, validates and executes moves,
        /// and handles high-level commands like quit, restart and retry.
        /// </summary>
        static void startChessGame()
        {
            isPlaying = true;
            while (isPlaying)
            {
                PrintBoard();

                string data = GetInput(out Command cmd);

                switch (cmd)
                {
                    case Command.Move:
                        {
                            string[] selections = data.Split('|')[0].Split(',');
                            string[] movements = data.Split('|')[1].Split(',');

                            int selectionX = int.Parse(selections[0]);
                            int selectionY = int.Parse(selections[1]);
                            int movementX = int.Parse(movements[0]);
                            int movementY = int.Parse(movements[1]);

                            PieceInfo piece = boardInfo[selectionY, selectionX];

                            // check if it is that "teams" turn
                            if (!piece.HasFlag(moveTurn))
                            {
                                WriteLog("Can't move that piece (wrong color). Try again");
                                continue;
                            }
                            if (piece == PieceInfo.None)
                            {
                                WriteLog("Can't move none. Try again");
                            }
                            // WriteLog($"moving {piece} from ({selectionX}, {selectionY})");

                            bool canMove = false;
                            // get the piece type using the bitmask
                            PieceInfo pieceType = piece & PieceInfo.Mask;
                            switch (pieceType)
                            {
                                case PieceInfo.Pawn:
                                    {
                                        canMove = checkMovePawn(piece, selectionX, selectionY, ref movementX, ref movementY);
                                        if (canMove)
                                        {
                                            halfMoveClock = -1;
                                        }
                                    }
                                    break;
                                case PieceInfo.Knight:
                                    {
                                        canMove = checkMoveKnight(piece, selectionX, selectionY, ref movementX, ref movementY);
                                    }
                                    break;
                                case PieceInfo.Bishop:
                                    {
                                        canMove = checkMoveBishop(piece, selectionX, selectionY, ref movementX, ref movementY);
                                    }
                                    break;
                                case PieceInfo.Rook:
                                    {
                                        canMove = checkMoveRook(piece, selectionX, selectionY, ref movementX, ref movementY);
                                    }
                                    break;
                                case PieceInfo.Queen:
                                    {
                                        canMove = checkMoveQueen(piece, selectionX, selectionY, ref movementX, ref movementY);
                                    }
                                    break;
                                case PieceInfo.King:
                                    {
                                        canMove = checkMoveKing(piece, selectionX, selectionY, ref movementX, ref movementY);
                                    }
                                    break;
                            }

                            if (!canMove)
                            {
                                WriteLog("That move was illegal. Try again");
                                continue;
                            }
                            if (!tryGetPiece(movementX, movementY, out PieceInfo otherPiece))
                            {
                                WriteLog("That move was illegal (oob). try again");
                                continue;
                            }
                            if (isPieceOnSameTeam(piece, otherPiece))
                            {
                                WriteLog("That move was illegal (same team). try again");
                                continue;
                            }

                            movePiece(selectionX, selectionY, movementX, movementY);

                            if (moveTurn == PieceInfo.Black)
                            {
                                moveNumber++;
                                moveTurn = PieceInfo.White;
                            }
                            else
                            {
                                moveTurn = PieceInfo.Black;
                            }
                            halfMoveClock++;
                            break;
                        }
                    case Command.Retry:
                        break;
                    case Command.Quit:
                        isPlaying = false;
                        break;
                    case Command.Restart:
                        initializeGame();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Returns true when the provided <paramref name="targetPiece"/> has previously moved.
        /// The implementation relies on the <see cref="PieceInfo.FirstMove"/> flag being set for pieces that have not moved yet.
        /// </summary>
        /// <param name="targetPiece">Piece info flags to inspect.</param>
        /// <returns>True if the piece has moved at least once; otherwise false.</returns>
        static bool pieceHasMoved(PieceInfo targetPiece)
        {
            return !targetPiece.HasFlag(PieceInfo.FirstMove);
        }

        /// <summary>
        /// Checks whether the masked piece type of <paramref name="targetPiece"/> equals <paramref name="info"/>.
        /// Uses the <see cref="PieceInfo.Mask"/> to isolate the piece type bits.
        /// </summary>
        /// <param name="targetPiece">The piece flags to inspect (may include color and other flags).</param>
        /// <param name="info">The piece type to compare against (e.g., <see cref="PieceInfo.Pawn"/>).</param>
        /// <returns>True if the types match; otherwise false.</returns>
        static bool isPiece(PieceInfo targetPiece, PieceInfo info)
        {
            if ((targetPiece & PieceInfo.Mask) == info)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the target piece is empty (no piece present).
        /// </summary>
        /// <param name="targetPiece">Piece info flags to inspect.</param>
        /// <returns>True if the masked piece type equals <see cref="PieceInfo.None"/>; otherwise false.</returns>
        static bool isPieceEmpty(PieceInfo targetPiece)
        {
            if ((targetPiece & PieceInfo.Mask) == PieceInfo.None)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether two pieces belong to the same team (color).
        /// Returns false if either piece is <see cref="PieceInfo.None"/>.
        /// </summary>
        /// <param name="piece1">First piece's flags.</param>
        /// <param name="piece2">Second piece's flags.</param>
        /// <returns>True if both pieces are on the same team; otherwise false.</returns>
        static bool isPieceOnSameTeam(PieceInfo piece1, PieceInfo piece2)
        {
            if (piece2 == PieceInfo.None || piece1 == PieceInfo.None)
            {
                return false;
            }
            // mask with 0x18 which is PieceInfo.White and PieceInfo.Black
            if (piece1.HasFlag(piece2 & PieceInfo.White | PieceInfo.Black))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to get the <see cref="PieceInfo"/> at the given board coordinate.
        /// </summary>
        /// <param name="x">Column index (0..7).</param>
        /// <param name="y">Row index (0..7).</param>
        /// <param name="pieceInfo">When successful, receives the piece info at the coordinate; otherwise <see cref="PieceInfo.None"/>.</param>
        /// <returns>True if the coordinates are inside board bounds; otherwise false.</returns>
        static bool tryGetPiece(int x, int y, out PieceInfo pieceInfo)
        {
            pieceInfo = PieceInfo.None;
            if (y < 0 || y >= board.GetLength(0))
            {
                return false;
            }
            if (x < 0 || x >= board.GetLength(1))
            {
                return false;
            }

            pieceInfo = boardInfo[y, x];
            return true;
        }

        /// <summary>
        /// Checks whether the straight line between two squares is unobstructed (exclusive).
        /// This supports rook/bishop/queen movement checks. The function verifies that
        /// the movement is linear (straight or diagonal) and then walks the path checking for pieces.
        /// </summary>
        /// <param name="originX">Origin X coordinate.</param>
        /// <param name="originY">Origin Y coordinate.</param>
        /// <param name="targetX">Target X coordinate.</param>
        /// <param name="targetY">Target Y coordinate.</param>
        /// <param name="pieceX">If an obstruction is found, set to the obstructing piece X; otherwise 0.</param>
        /// <param name="pieceY">If an obstruction is found, set to the obstructing piece Y; otherwise 0.</param>
        /// <returns>
        /// True if an obstructing piece was found between origin and target (exclusive) and <paramref name="pieceX"/>/<paramref name="pieceY"/> contain its coordinates;
        /// False if there is no obstruction or the movement is not linear (invalid for this check).
        /// </returns>
        static bool isClearInBetween(int originX, int originY, int targetX, int targetY, out int pieceX, out int pieceY)
        {
            int deltaX = targetX - originX;
            int deltaY = targetY - originY;

            int stepX = 0;
            if (deltaX != 0)
            {
                stepX = Math.Abs(deltaX) / Math.Abs(deltaX);
                if (deltaX < 0)
                {
                    stepX = -stepX;
                }
            }
            int stepY = 0;
            if (deltaY != 0)
            {
                stepY = Math.Abs(deltaY) / Math.Abs(deltaY);
                if (deltaY < 0)
                {
                    stepY = -stepY;
                }
            }

            if (deltaX != 0 && deltaY != 0 && Math.Abs(deltaX) != Math.Abs(deltaY))
            {
                pieceX = 0;
                pieceY = 0;
                return false;
            }

            int x = originX + stepX;
            int y = originY + stepY;

            while (x != targetX || y != targetY)
            {
                if (tryGetPiece(x, y, out PieceInfo piece) && !isPieceEmpty(piece))
                {
                    pieceX = x;
                    pieceY = y;
                    return true;
                }
                x += stepX;
                y += stepY;
            }
            pieceX = 0;
            pieceY = 0;
            return false;
        }

        /// <summary>
        /// Moves a piece on the board from one square to another, updating both the display board and the piece info bitfield.
        /// Also clears the source square and unsets the <see cref="PieceInfo.FirstMove"/> flag on the moved piece.
        /// </summary>
        /// <param name="fromX">Source X coordinate.</param>
        /// <param name="fromY">Source Y coordinate.</param>
        /// <param name="toX">Destination X coordinate.</param>
        /// <param name="toY">Destination Y coordinate.</param>
        static void movePiece(int fromX, int fromY, int toX, int toY)
        {
            WriteLog($"moving {board[fromY, fromX]} ({boardInfo[fromY, fromX]})/({fromX},{fromY}) to {board[toY, toX]} ({boardInfo[toY, toX]})/({toX},{toY})");

            // OS dev trick/bit level trick here
            // ~0x0010 = 0xFFEF
            // 0x0010 & 0xFFEF = 0x0000
            // yes that is true here we unset (zero) the PieceInfo.PieceDataFirstMove bit becurse you have moved that piece.
            // shit is happening
            boardInfo[fromY, fromX] &= ~PieceInfo.FirstMove;

            board[toY, toX] = board[fromY, fromX];
            board[fromY, fromX] = ' ';

            boardInfo[toY, toX] = boardInfo[fromY, fromX];
            boardInfo[fromY, fromX] = PieceInfo.None;
        }

        /// <summary>
        /// Computes the delta between a piece's position and a target movement, and flips the sign depending on current move turn.
        /// For Black's turn the deltas are inverted so that movement logic can be evaluated from a white-perspective.
        /// </summary>
        /// <param name="piecePosX">Piece X coordinate.</param>
        /// <param name="piecePosY">Piece Y coordinate.</param>
        /// <param name="movementX">Target X coordinate.</param>
        /// <param name="movementY">Target Y coordinate.</param>
        /// <param name="deltaX">Computed delta X (piecePosX - movementX), possibly inverted for Black.</param>
        /// <param name="deltaY">Computed delta Y (piecePosY - movementY), possibly inverted for Black.</param>
        static void getDelta(int piecePosX, int piecePosY, int movementX, int movementY, out int deltaX, out int deltaY)
        {
            deltaX = piecePosX - movementX;
            deltaY = piecePosY - movementY;

            if (moveTurn.HasFlag(PieceInfo.Black))
            {
                deltaY = -deltaY;
                deltaX = -deltaX;
            }
        }

        /// <summary>
        /// Validates pawn movement rules, including single and double advances, captures and en-passant handling.
        /// The function may modify <paramref name="movementX"/> and <paramref name="movementY"/> to reflect legal target coordinates
        /// and updates en-passant flags on neighboring pawns when a two-square advance is performed.
        /// </summary>
        /// <param name="piece">The pawn's flags (including color and move state).</param>
        /// <param name="piecePosX">Pawn current X coordinate.</param>
        /// <param name="piecePosY">Pawn current Y coordinate.</param>
        /// <param name="movementX">Requested destination X coordinate (passed by ref).</param>
        /// <param name="movementY">Requested destination Y coordinate (passed by ref).</param>
        /// <returns>True if the requested pawn move is legal; otherwise false.</returns>
        static bool checkMovePawn(PieceInfo piece, int piecePosX, int piecePosY, ref int movementX, ref int movementY)
        {
            getDelta(piecePosX, piecePosY, movementX, movementY, out int deltaX, out int deltaY);

            if (deltaX < 0)
            {
                deltaX *= -1;
            }
            if (deltaY < 0)
            {
                deltaY *= -1;
            }

            // a pawn can move 2 tiles in it's first move
            bool hasLongMove = !pieceHasMoved(piece);

            if (deltaX > 1 && deltaY > 2)
            {
                return false;
            }

            if (deltaY > 1 && !hasLongMove)
            {
                return false;
            }

            PieceInfo otherPiece;
            if (deltaY == 2 && hasLongMove)
            {
                if (tryGetPiece(movementX + 1, movementY, out otherPiece) && isPiece(otherPiece, PieceInfo.Pawn))
                {
                    boardInfo[movementY, movementX + 1] |= PieceInfo.CanEnPassant;
                }
                if (tryGetPiece(movementX - 1, movementY, out otherPiece) && isPiece(otherPiece, PieceInfo.Pawn))
                {
                    boardInfo[movementY, movementX - 1] |= PieceInfo.CanEnPassant;
                }
                return true;
            }

            if (deltaX != 0)
            {
                if (tryGetPiece(movementX, piecePosY, out otherPiece) && isPiece(otherPiece, PieceInfo.Pawn))
                {
                    boardInfo[piecePosY, piecePosX] &= ~PieceInfo.CanEnPassant;
                    boardInfo[piecePosY, movementX] = PieceInfo.None;
                    board[piecePosY, movementX] = ' ';
                    return true;
                }
                if (tryGetPiece(movementX, movementY, out otherPiece))
                {
                    if (isPieceEmpty(otherPiece))
                    {
                        return false;
                    }
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates knight movement: must be an L-shape (2 by 1).
        /// </summary>
        /// <param name="piece">The knight's flags.</param>
        /// <param name="piecePosX">Knight current X coordinate.</param>
        /// <param name="piecePosY">Knight current Y coordinate.</param>
        /// <param name="movementX">Requested destination X coordinate (passed by ref).</param>
        /// <param name="movementY">Requested destination Y coordinate (passed by ref).</param>
        /// <returns>True if the requested knight move is legal; otherwise false.</returns>
        static bool checkMoveKnight(PieceInfo piece, int piecePosX, int piecePosY, ref int movementX, ref int movementY)
        {
            getDelta(piecePosX, piecePosY, movementX, movementY, out int deltaX, out int deltaY);

            if (deltaX < 0)
            {
                deltaX *= -1;
            }
            if (deltaY < 0)
            {
                deltaY *= -1;
            }

            // if the any delta are not 2 then they are invalid input
            if (deltaX != 2 && deltaY != 2)
            {
                return false;
            }

            // if the any delta are not 1 then they are invalid input
            if (deltaX != 1 && deltaY != 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates bishop movement: must move strictly diagonally and the path must be unobstructed.
        /// </summary>
        /// <param name="piece">The bishop's flags.</param>
        /// <param name="piecePosX">Bishop current X coordinate.</param>
        /// <param name="piecePosY">Bishop current Y coordinate.</param>
        /// <param name="movementX">Requested destination X coordinate (passed by ref).</param>
        /// <param name="movementY">Requested destination Y coordinate (passed by ref).</param>
        /// <returns>True if the requested bishop move is legal; otherwise false.</returns>
        static bool checkMoveBishop(PieceInfo piece, int piecePosX, int piecePosY, ref int movementX, ref int movementY)
        {
            getDelta(piecePosX, piecePosY, movementX, movementY, out int deltaX, out int deltaY);

            if (deltaX < 0)
            {
                deltaX *= -1;
            }
            if (deltaY < 0)
            {
                deltaY *= -1;
            }

            if ((deltaX == 0 && deltaY != 0) || (deltaX != 0 && deltaY == 0) || (deltaX != deltaY))
            {
                return false;
            }

            if (isClearInBetween(piecePosX, piecePosY, movementX, movementY, out int pieceX, out int pieceY))
            {
                if (tryGetPiece(pieceX, pieceY, out PieceInfo otherPiece))
                {
                    if (isPieceOnSameTeam(piece, otherPiece))
                    {
                        return false;
                    }
                    else
                    {
                        // movementX = pieceX;
                        // movementY = pieceY;
                        // return true;
                        // could be made different but that ^ but... then this isn't chess compliant.
                        // - BjornBEs 12:29 osdev joke.
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        static bool checkMoveRook(PieceInfo piece, int piecePosX, int piecePosY, ref int movementX, ref int movementY)
        {
            getDelta(piecePosX, piecePosY, movementX, movementY, out int deltaX, out int deltaY);

            if (deltaX != 0 && deltaY != 0)
            {
                return false;
            }

            if (isClearInBetween(piecePosX, piecePosY, movementX, movementY, out int pieceX, out int pieceY))
            {
                if (tryGetPiece(pieceX, pieceY, out PieceInfo otherPiece))
                {
                    if (isPieceOnSameTeam(piece, otherPiece))
                    {
                        return false;
                    }
                    else
                    {
                        // movementX = pieceX;
                        // movementY = pieceY;
                        // return true;
                        // could be made different but that ^ but... then this isn't chess compliant.
                        // - BjornBEs 12:29 osdev joke.
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        static bool checkMoveQueen(PieceInfo piece, int piecePosX, int piecePosY, ref int movementX, ref int movementY)
        {
            getDelta(piecePosX, piecePosY, movementX, movementY, out int deltaX, out int deltaY);

            if (deltaX < 0)
            {
                deltaX *= -1;
            }
            if (deltaY < 0)
            {
                deltaY *= -1;
            }

            if (deltaX != 0 && deltaY != 0 && Math.Abs(deltaX) != Math.Abs(deltaY))
            {
                return false;
            }

            if (isClearInBetween(piecePosX, piecePosY, movementX, movementY, out int pieceX, out int pieceY))
            {
                if (tryGetPiece(pieceX, pieceY, out PieceInfo otherPiece))
                {
                    if (isPieceOnSameTeam(piece, otherPiece))
                    {
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates king movement including normal single-square moves and castling.
        /// - Normal moves: allow deltaX and deltaY in range [-1, 1].
        /// - Castling: if the king has not moved and attempts a multi-square horizontal move on its back rank,
        ///   the function verifies the rook, clear path and rook's first-move state, then performs the rook move
        ///   and adjusts <paramref name="movementX"/> to the castled king destination.
        /// </summary>
        /// <param name="piece">The king's <see cref="PieceInfo"/> flags.</param>
        /// <param name="piecePosX">Current king X coordinate.</param>
        /// <param name="piecePosY">Current king Y coordinate.</param>
        /// <param name="movementX">Requested destination X coordinate (passed by ref; may be modified for castling).</param>
        /// <param name="movementY">Requested destination Y coordinate (passed by ref).</param>
        /// <returns>
        /// True if the requested move is legal (including successful castling adjustments); otherwise false.
        /// </returns>
        static bool checkMoveKing(PieceInfo piece, int piecePosX, int piecePosY, ref int movementX, ref int movementY)
        {
            getDelta(piecePosX, piecePosY, movementX, movementY, out int deltaX, out int deltaY);

            if (deltaY > 1 || deltaY < -1)
            {
                return false;
            }

            int kingY = 0;
            if (moveTurn == PieceInfo.Black)
            {
                kingY = 7;
            }

            if (!pieceHasMoved(piece) && (deltaX > 1 || deltaX < -1) && kingY == movementY)
            {
                PieceInfo rook;
                int rookPosX = 0;
                int castlePosX = 0;
                int castleRookPosX = 0;
                if (deltaX < 0)
                {
                    if (tryGetPiece(7, movementY, out rook))
                    {
                        rookPosX = 7;
                        castlePosX = 6;
                        castleRookPosX = 5;
                    }
                    if (isPieceEmpty(rook))
                    {
                        return false;
                    }
                }
                else
                {
                    if (tryGetPiece(0, movementY, out rook))
                    {
                        rookPosX = 0;
                        castlePosX = 2;
                        castleRookPosX = 3;
                    }
                    if (isPieceEmpty(rook))
                    {
                        return false;
                    }
                }
                if (isPiece(rook, PieceInfo.Rook) && !pieceHasMoved(rook))
                {
                    if (!isClearInBetween(4, kingY, rookPosX, movementY, out int x, out int y))
                    {
                        movePiece(rookPosX, kingY, castleRookPosX, kingY);
                        movementX = castlePosX;
                        return true;
                    }
                    else
                    {
                        WriteLog($"something is at {x},{y} which is {boardInfo[y, x]}");
                    }
                }
                else
                {
                    WriteLog($"{rook} is not a rook or it has moved");
                }
                return false;
            }
            if (pieceHasMoved(piece) && (deltaX > 1 || deltaX < -1))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Maps a single-character FEN piece symbol (lowercase) to its <see cref="PieceInfo"/> type and a display character.
        /// </summary>
        /// <param name="symbol">Lowercase piece symbol (e.g. 'r','n','b','q','k','p').</param>
        /// <param name="boardSymbol">Output display character used on the console board.</param>
        /// <returns>The corresponding <see cref="PieceInfo"/> piece type, or <see cref="PieceInfo.None"/> for unknown symbols.</returns>
        static PieceInfo pieceTypeFromSymbol(char symbol, out char boardSymbol)
        {
            switch (symbol)
            {
                case 'r':
                    boardSymbol = '♖';
                    return PieceInfo.Rook;
                case 'n':
                    boardSymbol = '♘';
                    return PieceInfo.Knight;
                case 'b':
                    boardSymbol = '♗';
                    return PieceInfo.Bishop;
                case 'q':
                    boardSymbol = '♕';
                    return PieceInfo.Queen;
                case 'k':
                    boardSymbol = '♔';
                    return PieceInfo.King;
                case 'p':
                    boardSymbol = '♙';
                    return PieceInfo.Pawn;
            }
            boardSymbol = ' ';
            return PieceInfo.None;
        }
        /// <summary>
        /// Loads a board position from a FEN string.
        /// - Parses piece placement, active color, castling rights and move counters.
        /// - Sets piece characters on <see cref="board"/> and flags in <see cref="boardInfo"/>.
        /// </summary>
        /// <param name="fen">FEN string describing the position.</param>
        static void loadPositionFromFen(string fen)
        {
            // code taken from https://github.com/SebLague/Chess-Coding-Adventure/blob/Chess-V1-Unity/Assets/Scripts/Core/FenUtility.cs
            string[] fenSegments = fen.Split(' ');
            string fenBoard = fenSegments[0];

            int file = 0;
            int rank = 7;

            if (fenSegments[1] == "w")
            {
                moveTurn = PieceInfo.White;
            }
            else if (fenSegments[1] == "b")
            {
                moveTurn = PieceInfo.Black;
            }

            foreach (char symbol in fenBoard)
            {
                if (symbol == '/')
                {
                    file = 0;
                    rank--;
                }
                else
                {
                    if (char.IsDigit(symbol))
                    {
                        file += (int)char.GetNumericValue(symbol);
                    }
                    else
                    {
                        PieceInfo pieceColor = char.IsUpper(symbol) ? PieceInfo.White : PieceInfo.Black;
                        PieceInfo pieceType = pieceTypeFromSymbol(char.ToLower(symbol), out char pieceChar);
                        boardInfo[rank, file] = pieceColor | pieceType | PieceInfo.FirstMove;
                        board[rank, file] = pieceChar;
                        file++;
                    }
                }
            }

            if (fenSegments[2].Contains("K"))
            {
                boardInfo[0, 7] |= PieceInfo.CanCastle;
            }
            if (fenSegments[2].Contains("Q"))
            {
                boardInfo[0, 0] |= PieceInfo.CanCastle;
            }
            if (fenSegments[2].Contains("k"))
            {
                boardInfo[7, 7] |= PieceInfo.CanCastle;
            }
            if (fenSegments[2].Contains("q"))
            {
                boardInfo[7, 0] |= PieceInfo.CanCastle;
            }

            if (fenSegments[4] != "-")
            {
                halfMoveClock = int.Parse(fenSegments[4]);
            }
            if (fenSegments[5] != "-")
            {
                moveNumber = int.Parse(fenSegments[5]);
            }
        }

        /// <summary>
        /// Prints the available text-based input commands to the console and clears the input area below the help.
        /// </summary>
        static void GetInputPrintHelp()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("h - help menu");
            Console.WriteLine("m - move a piece");
            Console.WriteLine("p - print the board");
            Console.WriteLine("s - switch input system");
            Console.WriteLine("q - quit the game");
            Console.WriteLine("r - restart the game");
            int inputCursorY = Console.CursorTop;
            for (int i = 0; i < Console.WindowHeight - inputCursorY - 1; i++)
            {
                Console.WriteLine("".PadLeft(Console.WindowWidth - 1));
            }
            Console.SetCursorPosition(0, inputCursorY);
        }

        /// <summary>
        /// Reads user input based on the current input mode (<see cref="textBasedInput"/>).
        /// Supports the text-based command set:
        /// - 'h' show help, 'm' move piece (prompts for algebraic squares), 'p' print board,
        /// - 's' switch input, 'q' quit, 'r' restart.
        /// </summary>
        /// <param name="cmd">Out parameter that receives the parsed <see cref="Command"/>.</param>
        /// <returns>
        /// For moves, returns a string formatted as "selX,selY|dstX,dstY". For other commands returns an empty string.
        /// </returns>
        static string GetInput(out Command cmd)
        {
            if (textBasedInput)
            {
                while (true)
                {
                    GetInputPrintHelp();
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
                            {
                                cmd = Command.Move;
                                Console.Write("Select a piece in Algebraic notation:");
                                string piece = Console.ReadLine().ToUpper();
                                string selection = "";
                                if (piece.Length == 0)
                                {
                                    cmd = Command.Retry;
                                    return "";
                                }
                                if (char.IsLetter(piece, 0) && char.IsDigit(piece, 1))
                                {
                                    // represents the rows a through h as an index 0 through 7
                                    int row = piece[0] - 'A';

                                    // represents the columns 1 through 8 as an index 0 through 7
                                    int col = piece[1] - '1';

                                    selection = $"{row},{col}";
                                }
                                Console.Write("Where should that piece move to in Algebraic notation:");
                                piece = Console.ReadLine().ToUpper();
                                if (piece.Length == 0)
                                {
                                    cmd = Command.Retry;
                                    return "";
                                }
                                if (char.IsLetter(piece, 0) && char.IsDigit(piece, 1))
                                {
                                    // represents the rows a through h as an index 0 through 7
                                    int row = piece[0] - 'A';

                                    // represents the columns 1 through 8 as an index 0 through 7
                                    int col = piece[1] - '1';

                                    cmd = Command.Move;
                                    return selection + $"|{row},{col}";
                                }
                                return "";
                            }
                        case "p":
                            cmd = Command.Print;
                            return "";
                        case "s":
                            Console.Clear();
                            textBasedInput = false;
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
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (cursorY == 0)
                    {
                        cursorY = board.GetLength(0) - 1;
                    }
                    else
                    {
                        cursorY--;
                    }
                }
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (cursorY == board.GetLength(0) - 1)
                    {
                        cursorY = 0;
                    }
                    else
                    {
                        cursorY++;
                    }
                }
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (cursorX == 0)
                    {
                        cursorX = board.GetLength(1) - 1;
                    }
                    else
                    {
                        cursorX--;
                    }
                }
                if (key.Key == ConsoleKey.RightArrow)
                {
                    if (cursorX == board.GetLength(1) - 1)
                    {
                        cursorX = 0;
                    }
                    else
                    {
                        cursorX++;
                    }
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    if (tileSelected == true)
                    {
                        tileSelected = false;
                        cmd = Command.None;
                        return "";
                    }
                    cmd = Command.Quit;
                    return "";
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    if (tileSelected == false)
                    {
                        selectTile = (Tile)(cursorX) | (Tile)((cursorY) << 4);
                        tileSelected = true;
                    }
                    else
                    {
                        cmd = Command.Move;
                        int selectionX = (int)selectTile & 0x0F;
                        int selectionY = ((int)selectTile >> 4) & 0x0F;
                        tileSelected = false;
                        return $"{selectionX},{selectionY}|{cursorX},{cursorY}";
                    }
                }
            }
            cmd = Command.None;
            return "";
        }

        /// <summary>
        /// Renders the current board state to the console including turn/move counters and coordinate labels.
        /// Uses background/foreground colors to visually separate squares and piece colors.
        /// </summary>
        static void PrintBoard()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"number of turns {moveNumber} half {halfMoveClock}");
            Console.WriteLine($"it is {moveTurn} turn now");
            for (int y = boardInfo.GetLength(0) - 1; y != -1; y--)
            {
                Console.Write($"{y + 1} "); // Row label

                for (int x = 0; x < boardInfo.GetLength(1); x++)
                {
                    if ((x + y) % 2 == 1) // black background for uneven, white for even.
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    }
                    PieceInfo piece = boardInfo[y, x];

                    if (piece.HasFlag(PieceInfo.Black))
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    if ((piece & PieceInfo.Mask) == PieceInfo.None)
                    {
                        if (textBasedInput == false && x == cursorX && y == cursorY)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("[ ]");
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }
                    else
                    {
                        if (textBasedInput == false && x == cursorX && y == cursorY)
                        {
                            ConsoleColor save = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($"[");
                            Console.ForegroundColor = save;
                            Console.Write($"{board[y, x]}");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($"]");
                        }
                        else
                        {
                            Console.Write(" " + board[y, x] + " ");
                        }
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine("   a  b  c  d  e  f  g  h"); // column labels
        }

        //==========================================================
        //             Debugging functions and variables            
        //==========================================================

        static int logCursorX = 30;
        static int logCursorY = 0;

        static void WriteLog(string message)
        {
            int oldCursorX = Console.CursorLeft;
            int oldCursorY = Console.CursorTop;
            Console.SetCursorPosition(logCursorX, logCursorY);
            Console.WriteLine(message);
            logCursorY = Console.CursorTop % 30;
            Console.SetCursorPosition(oldCursorX, oldCursorY);
        }
    }
}

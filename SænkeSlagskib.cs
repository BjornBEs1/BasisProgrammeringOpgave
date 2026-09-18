using System.Security.Cryptography;
using System.Threading;
namespace BasisProgrammeringOpgave
{
    class SænkeSlagskib
    {

        public static void Start()
        {
            Console.WriteLine("Welcome to Sænke Slagskib!");
            Console.WriteLine("Press any key to start the game");
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("OPTIONS MENU");
            Console.WriteLine("Ready to play? (yes/no)");
            Console.WriteLine("Settings");



            string ready = Console.ReadLine(); //Spillerens input (yes) læses ind og gemmes i variablen "ready".
            if (ready == "yes")
            {
                Console.WriteLine();
                Console.WriteLine("Deploying ships...");
                Console.WriteLine("Ships deployed! Let the battle begin!");
                Console.WriteLine();

                char[,] computerBoard = new char[10, 10]; // Computerens bræt oprettes som et 10x10 gitter.
                for (int i = 0; i < 10; i++) // Ydre løkke , der går gennem hver række i computerens bræt.
                {
                    for (int j = 0; j < 10; j++) // Indre løkke, der går gennem hver kolonne i computerens bræt.
                    {
                        computerBoard[i, j] = '0'; // Hvert felt i computerens bræt sættes til 0 (0 = tomt felt).
                    }
                }

                int[] shipSize = { 7, 5, 4, 3, 3 }; // Array der indeholder størrelsen af de skibe, der skal placeres på computerens bræt.
                Random rnd = new Random(); // Random objekt oprettes for at generere tilfældige tal til placering af skibene på computerens bræt.
                foreach (int size in shipSize)
                {
                    bool placed = false; // Variabel der holder styr på, om skibet er placeret på brættet.
                    while (!placed) // Løkke der fortsætter, indtil skibet er placeret på brættet.                           
                    {
                        int row = rnd.Next(0, 10); // Tilfældig række genereres for skibets placering.
                        int col = rnd.Next(0, 10); // Tilfældig kolonne genereres for skibets placering.
                        bool horizontal = rnd.Next(0, 2) == 0; // Tilfældig retning for skibet (vandret eller lodret).
                        if (horizontal)
                        {
                            if (col + size <= 10) // Her tjekkes for plads til skibene gennem kolonnerne (Vandret). 
                            {
                                bool free = true;
                                for (int i = 0; i < size; i++) // For-løkke der går igennem hvert felt skibet ville fylde
                                {
                                    if (computerBoard[row, col + i] != '0')
                                        free = false; // Jeg bruger en if statement for at computeren kan vurderer, om der er plads til skibet.
                                }

                                if (free)
                                {
                                    for (int i = 0; i < size; i++)
                                        computerBoard[row, col + i] = 'x';
                                    placed = true; // Computeren placerer skibet, fordi der er plads på den tilfældigt valgte position.
                                }
                            }
                        }
                        else
                        {
                            if (row + size <= 10) // Her tjekkes for plads til skibene gennem rækkerne (Lodret).
                            {
                                bool free = true;
                                for (int i = 0; i < size; ++i)
                                {
                                    if (computerBoard[row + i, col] != '0')
                                        free = false;
                                }

                                if (free)
                                {
                                    for (int i = 0; i < size; i++)
                                        computerBoard[row + i, col] = 'x';

                                    placed = true;
                                }
                            }
                        }
                    }
                }
                Console.BackgroundColor = ConsoleColor.Blue; // Farver spilbrættet blåt, så det ligner et hav. 
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("THE ENEMY");

                PrintBoard(computerBoard);
                void PrintBoard(char[,] a)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            Console.Write(a[i, j] + " "); // Hvert felt i computerens bræt udskrives til konsollen, så det kan ses af spilleren.
                        }
                        Console.WriteLine(); // Går ned på næste linje, så næste række kan udskrives.
                    }

                }
                Console.WriteLine();
                Console.WriteLine("FRIENDLY FORCES");
                char[,] playerBoard = new char[10, 10]; // Spillerens bræt oprettes som et 10x10 gitter.
                for (int i = 0; i < 10; i++) // Ydre løkke , der går gennem hver række i spillerens bræt.
                {
                    for (int j = 0; j < 10; j++) // Indre løkke, der går gennem hver kolonne i spillerens bræt.
                    {
                        playerBoard[i, j] = '0'; // Hvert felt i spillerens bræt sættes til 0. 
                    }
                }
                foreach (int size in shipSize)
                {
                    bool placed = false;

                    while (!placed)
                    {
                        Console.WriteLine($"Placer dit skib på længde {size}");
                        Console.WriteLine("Skriv række og kolonne Fx: 0 0 eller 9 9");
                        string[] input = Console.ReadLine().Split(' ');
                        int row = int.Parse(input[0]);
                        int col = int.Parse(input[1]);

                        Console.WriteLine("Vandret eller lodret? (Tast: v/l)");
                        string direction = Console.ReadLine();
                        bool vandret = direction == "v";

                        bool free = true;

                        if (vandret)
                        {
                            if (col + size > 10)
                                free = false;
                            else
                            {
                                for (int i = 0; i < size; i++)
                                    if (playerBoard[row, col + i] != '0')
                                        free = false;

                            }
                        }
                        else
                        {
                            if (row + size > 10)
                                free = false;
                            else
                            {
                                for (int i = 0; i < size; i++)
                                    if (playerBoard[row + i, col] != '0')
                                        free = false;
                            }
                        }
                        if (free)
                        {
                            if (vandret)
                            {
                                for (int i = 0; i < size; i++)
                                    playerBoard[row, col + i] = 'x';
                            }
                            else
                            {
                                for (int i = 0; i < size; i++)
                                    playerBoard[row + i, col] = 'x';
                            }


                            Console.WriteLine();
                            Console.WriteLine("Ship deployed!");
                            Console.WriteLine();
                            placed = true;

                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Can't deploy the ship here, Sir! The position might already be taken or the ship might be too big for the position, Sir! (Place your ship between 0 0 & 9 9");
                            Console.WriteLine();
                        }

                    }
                    Console.Clear();


                }
                Console.WriteLine("Go To Battle? Type: Jubii");
                String GoToBattle = Console.ReadLine();
                if (GoToBattle == "Jubii")

                    Console.Clear();
                Console.WriteLine("THE ENEMY");
                Console.WriteLine();
                PrintBoard(computerBoard);

                Console.WriteLine();

                Console.WriteLine("FRIENDLY FORCES");
                Console.WriteLine();
                PrintBoard2(playerBoard);
                void PrintBoard2(char[,] a)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            Console.Write(a[i, j] + " "); // Hvert felt i spillerens bræt udskrives til konsollen, så det kan ses af spilleren.
                        }
                        Console.WriteLine();
                    }
                }

                while (true)
                {
                    PlayerShoot(computerBoard); // Spilleren skyder
                    {
                        if (AllShipsSunk(computerBoard))
                        {
                            Console.WriteLine("WE WON THE BATTLE, CAPTAIN!");
                            Thread.Sleep(3000);
                            break; // Spillet stopper, fordi spilleren vandt
                        }
                    }
                    computerShoot(playerBoard); // Computeren skyder
                    {
                        if (AllShipsSunk(playerBoard))
                        {
                            Console.WriteLine("We lost, Captain");
                            Thread.Sleep(3000);
                            break; // Spillet stopper, fordi computeren vandt
                        }
                    }

                    Console.Clear();
                    Console.WriteLine("THE ENEMY"); // Begge bræt opdateres
                    PrintBoard(computerBoard);

                    Console.WriteLine();
                    Console.WriteLine("FRIENDLY FORCES");
                    PrintBoard2(playerBoard);
                }


            }
            else if (ready == "no")
            {


                Console.WriteLine("Okay, maybe next time ):");
            }



        }
        public static void PlayerShoot(char[,] computerBoard)
        {
            Console.WriteLine("Fire the canons!");
            Console.WriteLine("Fx: 0 5 or 4 7");
            string[] shot = Console.ReadLine().Split(' ');
            int row = int.Parse(shot[0]);
            int col = int.Parse(shot[1]);

            if (computerBoard[row, col] == 'x')
            {
                computerBoard[row, col] = 'H';
                Console.WriteLine("It´s a HIT!");
                Thread.Sleep(3000);
            }
            else if (computerBoard[row, col] == '0')
            {
                computerBoard[row, col] = 'M';
                Console.WriteLine("It´s a miss");
                Thread.Sleep(3000);
            }
            else
            {
                Console.WriteLine("This position have already been hit, Sir");
                Thread.Sleep(3000);
            }
        }
        public static void computerShoot(char[,] playerBoard)
        {
            Random rnd = new Random();
            int row = rnd.Next(0, 10);
            int col = rnd.Next(0, 10);

            if (playerBoard[row, col] == 'x')
            {
                playerBoard[row, col] = 'H';
                Console.WriteLine("The enemy hit one of our ships!");
                Thread.Sleep(3000);
            }
            else if (playerBoard[row, col] == '0')
            {
                playerBoard[row, col] = 'M';
                Console.WriteLine("They missed!");
                Thread.Sleep(3000);
            }
            else
            {
                computerShoot(playerBoard); // Skyder igen hvis feltet er brugt
            }
            
        }
        public static bool AllShipsSunk(char[,] board) // Tjekker om alle skibe er ramt
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 'x')
                        return false; // Der er stadig et eller flere skibe
                }
            }

            return true; // Ingen skibe tilbage = tabt eller vundet
        }
    }
}

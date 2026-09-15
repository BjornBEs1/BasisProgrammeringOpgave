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

                char[,] playerBoard = new char[10, 10]; // Spillerens bræt oprettes som et 10x10 gitter.
                for (int i = 0; i < 10; i++) // Ydre løkke , der går gennem hver række i spillerens bræt.
                {
                    for (int j = 0; j < 10; j++) // Indre løkke, der går gennem hver kolonne i spillerens bræt.
                    {
                        playerBoard[i, j] = '0'; // Hvert felt i spillerens bræt sættes til 0. 
                    }
                }
                
      
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
            }   

            else if (ready == "no")
            {


                Console.WriteLine("Okay, maybe next time ):");
            }
            
        }
}   }

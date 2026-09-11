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



            string ready = Console.ReadLine(); //Spillerens input (yes) læses ind og gemmes i variablen "ready"
            if (ready == "yes")
            {
                Console.WriteLine();
                Console.WriteLine("Deploying ships...");
                Console.WriteLine("Ships deployed! Let the battle begin!");
                Console.WriteLine();
                    
                char[,] computerboard = new char[6, 6]; // Computerens bræt oprettes som et 6x6 gitter
                for (int i = 0; i < 6; i++) // Ydre løkke , der går gennem hver række i computerens bræt
                {
                    for (int j = 0; j < 6; j++) // Indre løkke, der går gennem hver kolonne i computerens bræt
                    { 
                      computerboard[i, j] = '0'; // Hvert fældt i computerens bræt sættes til 0 
                    }                       
                }
                PrintBoard(computerboard);
                void PrintBoard(char[,] a)
                {
                    for (int i = 0; i < 6; i++)
                    {
                         for (int j = 0; j < 6; j++)
                         {
                             Console.Write(a[i, j] + " "); // Hvert fældt i computerens bræt udskrives til konsollen, så det kan ses af spilleren
                         }
                             Console.WriteLine();
                    }

                }
                Console.WriteLine();

                char[,] playerboard = new char[6, 6]; // Spillerens bræt oprettes som et 6x6 gitter
                for (int i = 0; i < 6; i++) // Ydre løkke , der går gennem hver række i spillerens bræt
                {
                    for (int j = 0; j < 6; j++) // Indre løkke, der går gennem hver kolonne i spillerens bræt
                    {
                        playerboard[i, j] = '0'; // Hvert fældt i spillerens bræt sættes til 0 
                    }
                }
                PrintBoard2(playerboard);
                void PrintBoard2(char[,] a)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            Console.Write(a[i, j] + " "); // Hvert fældt i spillerens bræt udskrives til konsollen, så det kan ses af spilleren
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
    }
}
namespace BasisProgrammeringOpgave
{
    class SænkeSlagskib
    {
        
    
        public static void Start()
        {
            
             Console.Clear();
            
            
            Console.WriteLine("Welcome to Sænke Slagskib!");
            Console.WriteLine("Press any key to start the game");
            Console.WriteLine();
            Console.ReadKey();
            Console.Clear();    

            Console.WriteLine("OPTIONS MENU");
            Console.WriteLine("Ready to play? (yes/no)");
            Console.WriteLine("Settings");


            
            string ready = Console.ReadLine(); //Spillerens input (yes) læses ind og gemmes i variablen "ready"
            if (ready == "yes")
            {
                Console.Clear();
                
                Console.WriteLine();
                Console.WriteLine("Deploying ships...");
                Console.WriteLine("Ships deployed! Let the battle begin!");
                Console.WriteLine();
                    
                char[,] computerboard = new char[10, 10]; // Computerens bræt oprettes som et 10x10 gitter
                for (int i = 0; i < 10; i++) // Ydre løkke , der går gennem hver række i computerens bræt
                {
                    for (int j = 0; j < 10; j++) // Indre løkke, der går gennem hver kolonne i computerens bræt 
                    { 
                      computerboard[i, j] = '0'; // Hvert fældt i computerens bræt sættes til 0 
                    }                       
                }
                int[] shipSize = {7, 5, 4, 3, 3}; // Array der indeholder størrelsen af de skibe, der skal placeres på computerens bræt
                Random rnd = new Random(); // Random objekt oprettes for at generere tilfældige tal til placering af skibene på computerens bræt
                foreach (int size in shipSize)
                {
                    bool placed = false; // Variabel der holder styr på, om skibet er placeret på brættet
                    while (!placed) // Løkke der fortsætter, indtil skibet er placeret på brættet                           
                    {
                        int row = rnd.Next(0, 10); // Tilfældig række genereres for skibets placering
                        int col = rnd.Next(0, 10); // Tilfældig kolonne genereres for skibets placering
                        bool horisontal = rnd.Next(0, 2) == 0; // Tilfældig retning for skibet (horisontal eller vertikal)         
                    }                                                                                        
                }
                PrintBoard(computerboard);
                void PrintBoard(char[,] a)
                {
                    for (int i = 0; i < 10; i++)
                    {
                         for (int j = 0; j < 10; j++)
                         {
                             Console.Write(a[i, j] + " "); // Hvert fældt i computerens bræt udskrives til konsollen, så det kan ses af spilleren
                         }
                             Console.WriteLine();
                    }

                }
                Console.WriteLine(); // Linjeskift for at adskille computerens bræt fra spillerens bræt
                Console.WriteLine();

                char[,] playerboard = new char[10, 10]; // Spillerens bræt oprettes som et 10x10 gitter
                for (int i = 0; i < 10; i++) // Ydre løkke , der går gennem hver række i spillerens bræt
                {
                    for (int j = 0; j < 10; j++) // Indre løkke, der går gennem hver kolonne i spillerens bræt
                    {
                        playerboard[i, j] = '0'; // Hvert fældt i spillerens bræt sættes til 0 
                    }
                }
                PrintBoard2(playerboard);
                void PrintBoard2(char[,] a)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
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
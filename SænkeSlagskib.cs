using System.Security.Cryptography;
using System.Threading;
namespace BasisProgrammeringOpgave
{
    class SænkeSlagskib
    {

        public static void Start()
        {
            Console.WriteLine("Welcome to Sænke Slagskib! (Made by Rasmus)");
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
                foreach (int size in shipSize) // Foreach, så der vælges et skib ad gangen - Sørger for at gå videre til næste skib, når et skib placeres korrekt.
                {
                    bool placed = false;

                    while (!placed) // While-løkke der kører indtil hvert skib enkeltvis er placeret korrekt
                    {
                        Console.WriteLine($"Placer dit skib på længde {size}");
                        Console.WriteLine("Skriv række og kolonne Fx: 0 0 eller 9 9");
                        string[] input = Console.ReadLine().Split(' '); // deler spilleren input med et mellemrum. Vi får et array Fx [3, 7]
                        int row = int.Parse(input[0]); // Et spilbræt i C# er et 2D array, så vi kan kun bruge heltal. Derfor "tekst -> heltal(int).  
                        int col = int.Parse(input[1]);

                        Console.WriteLine("Vandret eller lodret? (Tast: v/l)");
                        string direction = Console.ReadLine(); // Spilleren vælger I hvilken retning skibet skal vende (Vandret eller lodret).
                        bool vandret = direction == "v";

                        bool free = true; // Variablen "free" bruges til at afgøre om skibet kan placeres uden overlap eller at gå ud over kanten.

                        if (vandret) // En if-sætning, der bruges til at finde ud af om ovenstående er muligt.
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
                            placed = true; // Skib placeret.

                        }
                        else
                        {
                            Console.WriteLine(); // Skib ikke placeret.
                            Console.WriteLine("Can't deploy the ship here, Sir! The position might already be taken or the ship might be too big for the position, Sir! (Place your ship between 0 0 & 9 9");
                            Console.WriteLine();
                        }

                    }
                    Console.Clear(); // Bruges til at fjerne en allerede gennemført del af spillet -> Giver et mere clean look.


                }
                Console.WriteLine("Go To Battle? Type: Jubii");
                String GoToBattle = Console.ReadLine();
                if (GoToBattle == "Jubii")

                    Console.Clear();
                Console.WriteLine("THE ENEMY");
                Console.WriteLine();
                PrintBoard(computerBoard); // Computerens bræt med skibe synliggøres.

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
                            Console.Write(a[i, j] + " "); // Spillerens bræt med skibe synliggøres.
                        }
                        Console.WriteLine();
                    }
                }

                while (true)
                {
                    PlayerShoot(computerBoard); // Spilleren skyder.
                    {
                        if (AllShipsSunk(computerBoard)) // En funktion, der afslutter spillet, når alle skibe er sunket på enten computerens eller spillerens bræt.
                        {
                            Console.WriteLine("WE WON THE BATTLE, CAPTAIN!");
                            Thread.Sleep(3000); // Får Console.WriteLine til at forblive synlig i fx 3 sekunder, før teksten så forsvinder igen.

                            GameOverAnimation(); // En sjov GAME OVER animation. 

                            string choice = EndOfGameMenu(); // Spilleren får mulighed for at starte et nyt spil eller gå tilbage til hovedmenuen.
                            if (choice == "1")
                            {
                                Console.ResetColor();
                                Console.Clear();
                                Start(); // Starter nyt spil. 
                            }
                            else if (choice == "2")
                            {
                                
                                Console.ResetColor();
                                Console.Clear();
                                return; // Tilbage til hovedmenuen.
                            }

                            break; // Spillet stopper, fordi spilleren vandt
                        }
                    }
                    computerShoot(playerBoard); // Computeren skyder
                    
                        if (AllShipsSunk(playerBoard))
                        {
                            Console.WriteLine("We lost, Captain");
                            Thread.Sleep(3000);

                            GameOverAnimation();

                            string choice = EndOfGameMenu();

                            if (choice == "1")
                            {
                                Console.Clear();
                                Start();
                            }
                            else if (choice == "2")
                            {
                                
                                Console.ResetColor();
                                Console.Clear();
                                return;
                            }
                            
                            break; // Spillet stopper, fordi computeren vandt
                        }
                    

                    Console.Clear();
                    Console.WriteLine("THE ENEMY"); // Begge bræt opdateres efter hvert skud.
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
            Console.WriteLine("Fire the canons!"); // Muligheder for spilleren, som vises når spillet er i gang.
            Console.WriteLine("Fx: 0 5 or 4 7");
            Console.WriteLine("Type: NUKE -> destroy all enemy ships instantly");

            string input = Console.ReadLine().Trim();

            //NUKE
            if (input.ToUpper() == "NUKE") // Hurtigt WIN!
            {
                NukeEnemyFleet(computerBoard); // En "NUKE" funktion, der gør det muligt for spilleren altid at afgøre spillet hurtigt i tilfælde af fx aftensmad.

                if(AllShipsSunk(computerBoard))
                {
                    Console.WriteLine("Enemy fleet destroyed! We are victorious!");
                    Thread.Sleep(2000);

                    GameOverAnimation();

                    string choice = EndOfGameMenu();

                    if (choice == "1")
                    {
                        Console.Clear();
                        Start();
                    }
                    else if (choice == "2")
                    {
                        
                        Console.ResetColor();
                        Console.Clear();
                        return;
                    }
                }
                return;
            }
            // Koordinatskydning
            string[] shot = input.Split(' ', StringSplitOptions.RemoveEmptyEntries); // Fjerner ekstra mellemrum hvis spilleren ved en fejl placerer dem.

            if (shot.Length != 2)
            {
                Console.WriteLine("Invalid input. Type row and column like: 6 5");
                return;
            }

            int row = int.Parse(shot[0]);
            int col = int.Parse(shot[1]);

            if (computerBoard[row, col] == 'x') // En neutral eller ukendt position er markeret med et "X".
            {
                computerBoard[row, col] = 'H'; // Når et skib rammes markeres positionen med "H"(Hit).
                Console.WriteLine("It´s a HIT!"); 
                Thread.Sleep(3000);
            }
            else if (computerBoard[row, col] == '0')
            {
                computerBoard[row, col] = 'M'; // En forbier makeres med et "M"(Miss"
                Console.WriteLine("It´s a miss");
                Thread.Sleep(3000);
            }
            else
            {
                Console.WriteLine("This position have already been hit, Sir"); // Hvis positionen allerede er blevet beskudt.
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
                computerShoot(playerBoard);
            }
            
        }
        public static bool AllShipsSunk(char[,] board) // Tjekker om alle skibe er sunket.
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 'x')
                        return false; // Der er stadig et eller flere skibe tilbage.
                }
            }

            return true; // Ingen skibe tilbage = tabt eller vundet
        }
        public static void GameOverAnimation()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green; // GAME OVER skrives med grøn tekst.

            string[] frames =
            {
                "G",
                "GA",
                "GAM",
                "GAME",
                "GAME O",
                "GAME OV",
                "GAME OVE",
                "GAME OVER",
                "GAME OVER!",
                "GAME OVER!!"
            };

            foreach (string frame in frames)
            {
                Console.Clear();
                Console.WriteLine(frame);
                Thread.Sleep(200); // 0,2 sek pr. Frame

            }

            Console.ResetColor();
            Thread.Sleep(1000);
        }
        public static string EndOfGameMenu() // Spilleren for et valg om at starte et nyt spil (1) eller gå tilbage til hovedmenuen (2).
        {
            Console.WriteLine();
            Console.WriteLine("1: New Battle! -> FOR GLORY!!!");
            Console.WriteLine("2: Return to main menu");

            string choice = Console.ReadLine(); // Spilleren taster "1" eller "2".

            return choice;
        }
        public static void NukeEnemyFleet(char[,] computerBoard) // Her ses "NUKE" funktionen. 
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (computerBoard[i, j] == 'x')
                    {
                        computerBoard[i, j] = 'H'; // Markerer alle skibe som ramt/sunket.
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NUKE LAUNCHED! the enemy fleet has been obliterated, Sir!"); // Rød tekst som oplyser spilleren om, at han/hun har vundet spillet med en "NUKE". 
            Console.ResetColor();
            Thread.Sleep(3000);
        }
    }
}

// Nedenstående viser "fog of war" funktionen, som bare direkte kan erstatte "PrintBoard(computerBoard)".
// Jeg undlader det i min kode for nu, da jeg hellere vil fremvise computerens evne til at vælge sine skibsplaceringer.
// Derudover ønsker jeg i vores video let at kunne fremvise et hit, uden at skulle lede efter computerens skibe haha.

// Void PrintFogOfWar(char[,] board)
// {
//     for (int i = 0; i < 10; i++)
//     {   
//          for (int j = 0; j < 10; j++)
//          {
//              if (board[i, j] == 'H')
//              Console.Write("H ");  // Et skib er ramt og positionen vises nu på brættet som et "H" for "Hit"
//              else if (board[i, j] == 'M')
//              Console.Write("M ");  // Intet skib er ramt og positionen vises nu på brættet som et "M" for "Miss"
//              else
//              Console.Write("0 "); // Skjuler skibe som tomme felter.
//          }
//          Console.WriteLine();
//      }
// }
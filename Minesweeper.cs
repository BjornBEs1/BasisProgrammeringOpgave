using System;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace BasisProgrammeringOpgave
{
    public static class Minesweeper
    {
        public static void Start()
        {
            //The player is first presented with question = name
            Console.WriteLine("Hvad er dit navn");
            string navn = Console.ReadLine();

            while (true)
            {   // The player gets a menu with grettings and multipull choice selection
                Console.WriteLine("Velkommen til den sejeste Minesweeper ever");
                Console.WriteLine("");
                Console.WriteLine("Vælg en af følgene muligheder ved at trykke 1,2 eller 3");
                Console.WriteLine("1. Start spil");
                Console.WriteLine("2. Credit");
                Console.WriteLine("3.Afslut spil");
                string svar = Console.ReadLine();

                if (svar == "1")
                {
                    StartGame(); //If answer "1", the function "StartGame" is called upon
                }
                else if (svar == "2")
                {
                    Console.WriteLine("lavet af Emil");
                }
                else if (svar == "3")
                {
                    Console.Clear();
                    break;
                }

            }
        }




        public static void StartGame()
        {

            Console.Clear();
            char[,] BoardArray = new char[10, 10];
            char[,] BombeArray = new char[10, 10];
            //Fyld board med "?"
            for (int y = 0; y < BoardArray.GetLength(0); y++)
            {
                for (int x = 0; x < BoardArray.GetLength(1); x++)
                {
                    BoardArray[y, x] = '?';
                }
            }
            //Kalder funktionen MinePlacering for at sætte "Rnd" bomber
            MinePlacering(BombeArray);

            for (int y = 0; y < BoardArray.GetLength(0); y++)
            {
                for (int x = 0; x < BoardArray.GetLength(1); x++)
                {
                    Console.Write(BoardArray[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
            BrugerInput(BombeArray);
        }


        public static void MinePlacering(char[,] BombeArray)
        {
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {

                int y = rnd.Next(0, 10);
                int x = rnd.Next(0, 10);

                BombeArray[y, x] = '*';

            }
        }

        public static void BrugerInput(char[,] BombeArray)
        {
            while (true)
            {
                Console.Write("X: ");
                int x = Convert.ToInt32(Console.ReadLine());

                Console.Write("Y: ");
                int y = Convert.ToInt32(Console.ReadLine());


                if (BombeArray[y, x] == '*')
                {
                    Console.Clear();
                    Console.WriteLine("BOOM!");
                    Console.WriteLine("Du ramte sku en bombe min ven");
                    Console.WriteLine("Ønsker du at hoppe til hovedmenuen?");
                    Console.WriteLine("Ja eller nej");
                    string svar = Console.ReadLine();
                    if (svar == ("ja"))
                    {
                        Console.Clear();
                      Start();
                        Console.Clear();
                    }
                    else
                    {
                        if (svar == ("nej"))
                            break;
                    }



                    
                    break;
                }
                else
                {
                    {
                        Console.WriteLine("Sikker");
                    }
                }

            }
        }
    }
}







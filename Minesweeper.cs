using System;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace BasisProgrammeringOpgave
{
    public static class Minesweeper
    {
        static char[,] BoardArray;
        static char[,] BombeArray;
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
            BoardArray = new char[10, 10];
            BombeArray = new char[10, 10];
            //Fills the board with "?"
            for (int y = 0; y < BoardArray.GetLength(0); y++)
            {
                for (int x = 0; x < BoardArray.GetLength(1); x++)
                {
                    BoardArray[y, x] = '?';
                }
            }
            //Calls upon the funktion named MinePlacering to insert "Rnd" bombs
            MinePlacering();
            int bombeTæller = 0;
            BrugerInput();
        }




        public static void MinePlacering()
        {
            //here the 10 randoms bombs is generated
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {

                int y = rnd.Next(0, 10);
                int x = rnd.Next(0, 10);

                BombeArray[y, x] = '*';

            }
            
        }




        public static void BrugerInput()
        {
            //Here we start a while loop 
            while (true)
            {
                Console.Clear();
                int y = 0;
                int x = 0;
               // Here the second board is made that will be used for bombs
                for (y = 0; y < BoardArray.GetLength(0); y++)
                {
                    Console.Write($"{y+1}".PadRight(4));
                    for (x = 0; x < BoardArray.GetLength(1); x++)
                    {
                        Console.Write(BoardArray[y, x] + " ");
                    }
                    Console.WriteLine();
                }
                //This will make sure you cant write anything but a legal number
                Console.Write("X: ");
                string userInputX = Console.ReadLine();
                if (!int.TryParse(userInputX, out x))
                {
                    Console.WriteLine("Invalid Input For X Cordinates");
                    continue;
                }

                //This will make sure you cant write anything but a legal number
                Console.Write("Y: ");
                string userInputY = Console.ReadLine();
                if (!int.TryParse(userInputY, out y))
                {
                    Console.WriteLine("Invalid Input For Y Cordinates");
                    continue;
                }

                x -= 1;
                y -= 1;
                //Here is all the text and option that revel itself if you hit a bomb.
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
                }
                else
                {
                    {
                        BoardArray[y, x] = ('O');
                        Console.WriteLine("Sikker");
                    }
                }
                //Here the bombs arround the selected point is reveled 
                int bombeTæller = 0;

               // Here the BombArray on the left revels itself if there are bombs located
                if (BombeArray[y - 1, x - 1] == '*') bombeTæller++;
                if (BombeArray[y - 1, x] == '*') bombeTæller++;
                if (BombeArray[y - 1, x + 1] == '*') bombeTæller++;

                // Here the BombArray on the top and buttom revels itself if there are bombs located
                if (BombeArray[y, x - 1] == '*') bombeTæller++;
                if (BombeArray[y, x + 1] == '*') bombeTæller++;

                // Here the BombArray on the right revels itself if there are bombs located
                if (BombeArray[y + 1, x - 1] == '*') bombeTæller++;
                if (BombeArray[y + 1, x] == '*') bombeTæller++;
                if (BombeArray[y + 1, x + 1] == '*') bombeTæller++;

                BoardArray[y, x] = bombeTæller.ToString()[0];

            }
        }

    }
}
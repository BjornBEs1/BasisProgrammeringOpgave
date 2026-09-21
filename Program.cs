/*
 * File: Program.cs
 * File Created: 11 Sep 2026
 * Author: BjornBEs
 * -----
 * Last Modified: 11 Sep 2026
 * Modified By: BjornBEs
 * -----
 */

namespace BasisProgrammeringOpgave;

class Program
{
    /// <summary>
    /// Makes a new menu with options and input
    /// </summary>
    /// <param name="options">An array of options to choose</param>
    /// <param name="choice">The returning choice from options or String.Empty if the function returns -1</param>
    /// <returns>The index in options that the user choose</returns>
    static int OpenMenu(string[] options, out string choice)
    {
        int cursor = 0;

        while (true)
        {
            // setting the cursor to 0, 0 so we don't have too clear the screen all the time
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Use the Up and Down arrows to move the cursor and select using enter");
            for (int i = 0; i < options.Length; i++)
            {
                if (i == cursor)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"\t{options[i]}");
                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                // check for exit
                choice = String.Empty;
                return -1;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                // check for accept
                choice = options[cursor];
                return cursor;
            }
            else if (key.Key == ConsoleKey.UpArrow && cursor != 0)
            {
                // limiting the cursor so that it can't go under 0
                cursor--;
            }
            else if (key.Key == ConsoleKey.DownArrow && cursor < options.Length - 1)
            {
                // limiting the cursor so that it can't go over options.Length
                cursor++;
            }
        }
    }

    static void choiceWelcome()
    {
        Console.WriteLine("Welcome to the program!");
        Console.WriteLine("In this program can you play these games");
        Console.WriteLine("option1, option2 and option3");
        Console.WriteLine("you also also write 'help' to print the help text");
    }

    static void choiceOption1()
    {
        SænkeSlagskib.Start();
    }

    static void choiceOption2()
    {
        Minesweeper.Start();
    }

    static void choiceOption3()
    {
        Chess.Start();
    }

    static void choiceHelp()
    {
        Console.WriteLine("Sænke Slagskib   - Will play Sænke Slagskib");
        Console.WriteLine("Minesweeper      - Will play Minesweeper");
        Console.WriteLine("chess            - Will play chess");
        Console.WriteLine("help             - Will show this text");
        Console.WriteLine("credit           - Will show who was apart of this program");
        Console.WriteLine("exit             - Will close the program");
    }

    static void choiceCredit()
    {
        Console.Clear();
        string[] credits =
        {
            "BjornBEs - Made the menu",
            "BjornBEs - Made chess",
            "Emil - Made minesweeper",
            "Rasmus - Made Sænke Slagskib"
        };

        int middle = Console.BufferWidth / 2;
        for (int i = 0; i < credits.Length; i++)
        {
            string s = credits[i];
            int length = s.Length;
            int point = middle - (length / 2);
            Console.SetCursorPosition(point, i);
            Console.WriteLine(s);
        }

        Console.WriteLine("press any key to continue.");
        Console.ReadLine();
        Console.Clear();
    }

    static void Main(string[] args)
    {
        // only for debugging here
        // becurse vscode doesn't work
        // FUCK YOU MICROSOFT AND WINDOWS FOR NOT GIVING LINUX VS IDE
        if (args.Length > 0)
        {
            // check the command line's arguments for "chess"
            if (args[0].Equals("chess"))
            {
                choiceOption3();
                Environment.Exit(0);
            }
        }

        // the 'normal' path

        // resetting the console colors and contents
        Console.Clear();
        Console.ResetColor();
        Console.CursorVisible = false;

        // options for menu and actions for those options
        string[] options = { "Sænke Slagskib",  "Minesweeper",  "Chess",        "help",     "credit",       "exit" };
        Action[] actions = { choiceOption1,     choiceOption2,  choiceOption3,  choiceHelp, choiceCredit };

        while (true)
        {
            // a feature that i wanted to use from day 1, but i couldn't becurse of Denni.
            // - BjornBEs
            int index = OpenMenu(options, out string choice);
            
            // if it is within the length of the actions array then use that
            if (index < actions.Length)
            {
                Console.WriteLine();
                actions[index]();
            }

            // else it is most likely 'exit' but we need to check

            switch (choice)
            {
                case "exit":
                    // could also do return in Main but Environment.Exit reads more clean
                    Environment.Exit(0);
                    break;
            }
        }
    }
}

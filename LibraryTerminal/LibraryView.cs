using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class LibraryView
    {
        //Displays the list of media.
        public static void DisplayMedias(List<Media> list)
        {
            Console.WriteLine();
            for (int i = 0; i < list.Count; i++)
            {
                DisplayMenu(i + 1, list[i].Title);
            }
            Console.WriteLine();
        }

        //Displays information about an individual media.
        public static void DisplayMedia(Media media)
        {
            Console.WriteLine();
            Console.WriteLine($"{media.Title} is in the format {media.MediaType}\nAvailable: {!media.CheckedOut}");
        }

        //Displays each menu item as a single line
        public static void DisplayMenu(int count, string listing)
        {
            Console.WriteLine($"{count}- {listing}");
        }

        //Displays a list of media.
        public static void DisplayMainMenu(string[] menu)
        {
            Console.WriteLine();
            for (int i = 0; i < menu.Length; i++)
            {
                LibraryView.DisplayMenu(i + 1, menu[i]);
            }
            Console.WriteLine();

        }

        //Displays an error message.
        public static void EmptyFileError()
        {
            Console.WriteLine("File is empty");
        }

        //Propmts the user for input
        public static void PromptUser(string message)
        {
            Console.WriteLine(message);
        }

        //Propmts the user for input
        public static void PromptUserAndWait(string message)
        {
            Console.Write(message);
        }
    }
}

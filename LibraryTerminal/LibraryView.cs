using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class LibraryView
    {
        public static void DisplayMedias(List<Media> list)
        {
            Console.WriteLine();
            for (int i = 0; i < list.Count; i++)
            {
                DisplayMenu(i + 1, list[i].Title);
            }
            Console.WriteLine();
        }

        public static void DisplayMedia(Media media)
        {
            Console.WriteLine();
            Console.WriteLine($"{media.Title} is in the format {media.MediaType}\nAvailable: {!media.CheckedOut}");
        }

        public static void DisplayMenu(int count, string listing)
        {
            Console.WriteLine($"{count}- {listing}");
        }

        public static void DisplayMainMenu(string[] menu)
        {
            Console.WriteLine();
            for (int i = 0; i < menu.Length; i++)
            {
                LibraryView.DisplayMenu(i + 1, menu[i]);
            }
            Console.WriteLine();

        }

        public static void EmptyFileError()
        {
            Console.WriteLine("File is empty");
        }

        public static void PromptUser(string message)
        {
            Console.WriteLine(message);
        }

        public static void PromptUserAndWait(string message)
        {
            Console.Write(message);
        }
    }
}

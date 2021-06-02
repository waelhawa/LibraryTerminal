using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class MainMenu
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("Please make a selection:");
            Console.WriteLine("1. Display book/media list");
            Console.WriteLine("2. Search by author");
            Console.WriteLine("3. Search by title keyword ");
            Console.WriteLine("4. Search by media type");
            Console.WriteLine("5. Checkout a book/media");
            Console.WriteLine("6. Return a book/media");
        }

        public static void MenuSelection()
        {
            DisplayMenu();

            int userSelection = ValidateMenuSelection();
            switch(userSelection)
            {
                case 1:
                    //method that displays the list of books/media
                    break;

                case 2:
                    
                    break;

                case 3:
                    // method that searches by title keyword
                    break;

                case 4:
                    // method that seaches by nedia type
                    break;

                case 5:
                    // method that checksout a book
                    break;

                case 6:
                    // method that returns a book
                    break;
            }

        }
            public static int ValidateMenuSelection()
            {
                Console.WriteLine("Please enter a menu number:");
            int userSelection;
            while (!int.TryParse(Console.ReadLine(), out userSelection) || userSelection < 1 || userSelection > 6)
            {
                Console.WriteLine("That is not an option. Please try again.");
                DisplayMenu();
                Console.WriteLine("Please enter a menu number:");
            }
            return userSelection;
            }

        // Search Option:
        // Prompt a field to search for an author
        // take user input
        // Compare input to list of last names
        // foreach loop (using a list)
        // display the available options
        // Prompt the user for final selection.

        public static void SearchByAuthor(List<Media> mediaList)
        {
            List<Media> searchList = new List<Media>();
            Console.WriteLine("Please input author's name");
            string input = Console.ReadLine().ToLower();
            foreach (Media media in mediaList)
            {
                if (media.Author.ToLower() == input)
                {
                    searchList.Add(media);
                }

            }
            foreach(Media media in searchList)
            {
                Console.WriteLine(media.Author, media.Title, media.MediaType, media.CheckedOut, media.DueDate);
            }
            // Call/get text file.
            // Foreach loop to index the author
            // display media with that author
        }

        public static void SearchByTitle(List<Media> mediaList)
        {
            List<Media> searchList = new List<Media>();
            Console.WriteLine("Please input title keyword");
            string input = Console.ReadLine().ToLower();
            foreach (Media media in mediaList)
            {
                if (media.Author.ToLower().Contains(input))
                {
                    searchList.Add(media);
                }

            }
            foreach (Media media in searchList)
            {
                Console.WriteLine(media.Author, media.Title, media.MediaType, media.CheckedOut, media.DueDate);
            }
        }
    }
}

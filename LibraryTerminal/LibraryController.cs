using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibraryTerminal
{
    class LibraryController
    {
        //Predefined messages and errors for the user.
        const string fileName = "Library";
        const string overrideConfirmation = "Are you sure you want to override existing file? (Y/N): ";
        const string yesOrNoError = "Please select Y or N";
        const string confirmLoad = "Loading a file will erase any unsaved data. Are you sure? (Y/N): ";
        const string fileNotLoaded = "File was not loaded.";
        const string fileFound = "There's a save file, would you like to load it? (Y/N): ";
        const string success = "Action completed";
        const string emptyFileError = "File is empty";
        const string mediaExists = "This already exist";
        const string menuSelectionPrompt = "Select from menu: ";
        const string selectionNotInMenu = "This selection is not valid.";
        const string numberOnlyError = "Use numbers only";
        const string confirmDeletion = "Are you sure you want to delete saved file? (Y/N): ";
        const string fileNotFoundError = "File not found!";
        const string displayMediaStatus = "Would you like to see the status of this media? (Y/N): ";
        const string emptyList = "The Library list is empty, please add a media";
        const string exitMessage = "Goodbye!";
        const string emptyFieldError = "Field cannot be empty";
        const string unsupportedMedia = "This media type is not supported";

        //Predefined menu items
        static readonly string [] menu = {"Display Library", "Add Media", "Remove Media", "Save to file", "Load from file", "Delete saved file", "Search by...", "Return an item", "Exit" };
        static readonly string[] searchMenu = { "Search by Title", "Search by Author", "Back to Main Menu" };

        public List<Media> Media { get; set; } //Main properties that holds the main list of media.
        public List<Media> SearchList { get; set; } //Holds a list of media the user searches for.
        public List<Media> CheckoutList { get; set; } //Holds the list of items currently checked out of the library

        //Reads the main list and saves it to a file.
        public void Save(List<Media> list, string fileName)
        {
            StreamWriter writer = new StreamWriter($"../../../{fileName}.txt");
            foreach (Media media in list)
            {
                writer.WriteLine($"{media.Title}|{media.Author}|{media.MediaType}|{media.CheckedOut}|{media.DueDate}");
            }
            writer.Close();
        } //end Save


       //Checks if there is a saved file, and prompts the user to load a file.
        public void PreLoad()
        {
            if (File.Exists($"../../../{fileName}.txt"))
            {
                if (Verification(fileFound, yesOrNoError))
                {
                    Load(Media, fileName, true);
                    LibraryView.PromptUser(success);
                }
            }
            StartHere();
        } //end PreLoad

        //Read the text file and load its contents into the main menu.
        //Creates a corresponding object to each media.
        public void Load(List<Media> mediaList, string fileName, bool preLoadVerified)
        {
            if (preLoadVerified || Verification(confirmLoad, yesOrNoError))
            {
                mediaList.Clear();
                StreamReader reader = ReadFile(fileName);
                if (reader != null)
                {
                    string text = reader.ReadLine();
                    string[] properties;

                    while (text != null)
                    {
                        properties = text.Split('|');
                        if (properties[2] == "book")
                        {
                            mediaList.Add(new Book(properties[0], properties[1], properties[2], bool.Parse(properties[3]), DateTime.Parse(properties[4])));
                        }
                        else if (properties[2] == "cd")
                        {
                            mediaList.Add(new CD(properties[0], properties[1], properties[2], bool.Parse(properties[3]), DateTime.Parse(properties[4])));
                        }
                        else if (properties[2] == "magazine")
                        {
                            mediaList.Add(new Magazine(properties[0], properties[1], properties[2], bool.Parse(properties[3]), DateTime.Parse(properties[4])));
                        }
                        else
                            mediaList.Add(new Movie(properties[0], properties[1], properties[2], bool.Parse(properties[3]), DateTime.Parse(properties[4])));
                        text = reader.ReadLine();
                    }
                    if (mediaList.Count > 0)
                    {
                        //LibraryView.DisplayMedias(mediaList);
                    }
                    else
                    {
                        LibraryView.PromptUser(emptyFileError);
                    }
                    reader.Close();
                }
                else
                {
                    LibraryView.PromptUser(emptyFileError);
                }
            }
            else
            {
                LibraryView.PromptUser(fileNotLoaded);
            }

        } //end Load

        //Checks if there is a saved file, and returns a reference to the reader object. 
        //Returns null if there is no file
        public StreamReader ReadFile(string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader($"../../../{fileName}.txt");
                return reader;

            }
            catch (FileNotFoundException)
            {
                return null;
            }
        } //end ReadFile

        //Checks if there is a save file, and if the user wants to override the file.
        //If no file exists the file is saved.
        public void CheckSavedFile()
        {
            if (File.Exists($"../../../{fileName}.txt"))
            {
                if (Verification(overrideConfirmation, yesOrNoError))
                {
                    Save(Media, fileName);
                }
            }
            else
            {
                Save(Media, fileName);
            }
        } //end CheckSavedFile

        //Adds media to a list of other media.
        public void AddToList(Media media)
        {
            Media.Add(media);
        } //end AddToList

        //Allows the user to remove references to the media, and removes that specific media.
        public void RemoveFromList(Media media)
        {
            Media.Remove(media);
            LibraryView.PromptUser(success);
        } //end RemoveFromList

        //Checks if there is already a media that exists with the name.
        public void CheckListForEntry(Media media)
        {
            bool exists = false;
            foreach (Media m in Media)
            {
                if (m.Title == media.Title)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                AddToList(media);
                LibraryView.PromptUser(success);
                StartHere();
            }
            else
            {
                LibraryView.PromptUser(mediaExists);
                StartHere();
            }
        } //end CheckListForEntry

        //Makes sure the number returned is an integer.
        public int IntegerEntry(string phrase, string error)
        {
            string text;
            int integerNumber;
            while (true)
            {
                LibraryView.PromptUserAndWait(phrase);
                text = Console.ReadLine().Trim().ToLower();
                if (int.TryParse(text, out integerNumber))
                {
                    return integerNumber;
                }
                else
                {
                    LibraryView.PromptUser(error);
                }
            }
        } //end IntegerEntry

        //If the item specified is within the menu range the selection will be valid.
        public int MenuSelection(int menuStart, int menuEnd)
        {
            int item;
            while (true)
            {
                item = IntegerEntry(menuSelectionPrompt, numberOnlyError);
                if (item >= menuStart && item <= menuEnd)
                {
                    return item;
                }
                else
                {
                    LibraryView.PromptUser(selectionNotInMenu);
                }
            }
        } //End MenuSelection

        //Returns a validation of yes, or no.
        public bool Verification(string phrase, string error)
        {
            string text;
            while (true)
            {
                LibraryView.PromptUserAndWait(phrase);
                text = Console.ReadLine().Trim().ToLower();

                switch (text)
                {
                    case "y":
                    case "yes":
                        return true;


                    case "n":
                    case "no":
                        return false;


                    default:
                        LibraryView.PromptUser(error);
                        break;
                }
            }
        } //end Verification

        //Beginning of the program.
        public void StartHere()
        {
            LibraryView.DisplayMainMenu(menu);
            int selection = MenuSelection(1, menu.Length);
            MainMenu(selection);
        } //end StartHere

        //Takes in user input as text.
        public string UserInput(string property)
        {
            string text;
            LibraryView.PromptUserAndWait($"Enter {property}: ");
            text = Console.ReadLine();
            return text.Trim().ToLower();
        } //end UserInput

        //Allows the user to create a new media specified by the media type the user chooses.
        public void NewMedia()
        {
            Media newMedia = null;
            string text;
            bool resume = true;
            while (resume)
            {
                text = UserInput("Media Type");
                switch (text)
                {
                    case "book":
                        newMedia = new Book { Title = MultipleWordIndent(UserInput("Title")), Author = MultipleWordIndent(UserInput("Author")), MediaType = MultipleWordIndent(text) };
                        resume = false;
                        break;
                    case "movie":
                        newMedia = new Movie { Title = MultipleWordIndent(UserInput("Title")), Author = MultipleWordIndent(UserInput("Author")), MediaType = MultipleWordIndent(text) };
                        resume = false;
                        break;
                    case "cd":
                        newMedia = new CD { Title = MultipleWordIndent(UserInput("Title")), Author = MultipleWordIndent(UserInput("Author")), MediaType = MultipleWordIndent(text) };
                        resume = false;
                        break;
                    case "magazine":
                        newMedia = new Magazine { Title = MultipleWordIndent(UserInput("Title")), Author = MultipleWordIndent(UserInput("Author")), MediaType = MultipleWordIndent(text) };
                        resume = false;
                        break;
                    default:
                        LibraryView.PromptUser(unsupportedMedia);
                        break;
                }
            }
            CheckListForEntry(newMedia);
        } //end NewCountry

        //The menu where the user begins, and decides how to use/enable to program and its options.
        public void MainMenu(int selection)
        {
            bool verifyChoice;
            int choice;
            switch (selection)
            {
                case 1:
                    if (Media.Count > 0)
                    {
                        LibraryView.DisplayMedias(Media);
                        verifyChoice = Verification(displayMediaStatus, yesOrNoError);
                        SubMenuOne(verifyChoice);
                    }
                    else
                    {
                        LibraryView.PromptUser(emptyList);
                        StartHere();
                    }
                    break;
                case 2:
                    NewMedia();
                    break;
                case 3:
                    if (Media.Count > 0)
                    {
                        LibraryView.DisplayMedias(Media);
                        choice = MenuSelection(1, Media.Count);
                        RemoveFromList(Media[choice - 1]);
                        StartHere();
                    }
                    else
                    {
                        LibraryView.PromptUser(emptyList);
                        StartHere();
                    }
                    break;
                case 4:
                    CheckSavedFile();
                    StartHere();
                    break;
                case 5:
                    Load(Media, fileName, false);
                    StartHere();
                    break;
                case 6:
                    DeleteSavedFile();
                    StartHere();
                    break;
                case 7:
                    SearchMenu();
                    break;
                case 8:
                    ReturnMedia();
                    break;
                default:
                    LibraryView.PromptUser(exitMessage);
                    Environment.Exit(0);
                    break;
            }
        } //end MainMenu

        //Allows the user to search for a media by a title keyword, or by an author.
        public void SearchMenu()
        {
            LibraryView.DisplayMainMenu(searchMenu);
            int selection = MenuSelection(1, searchMenu.Length);
            switch (selection)
            {
                case 1:
                    SeachByTitle();
                    break;
                case 2:
                    SearchByAuthor();
                    break;
                default:
                    StartHere();
                    break;
            }
        } //end SearchMenu

        //Checks out whatever is in the search list.
        public void CheckoutMenu ()
        {   //1. select from the searchlist the item they want to checkout

            LibraryView.DisplayMedias(SearchList);

            //getting the user menu selection
            int menuItem = MenuSelection(1, SearchList.Count) - 1;

            //checking it out and setting the duedate
            if (!SearchList[menuItem].CheckedOut)
            {
                SearchList[menuItem].CheckedOut = true;
                SearchList[menuItem].DueDate = DateTime.Now.AddDays(14);

                Console.WriteLine("You have checked out this item.");
                Console.WriteLine($"Due date: {SearchList[menuItem].DueDate}");
            }
            else
            {
                Console.WriteLine($"This item is not available. It will be available by {SearchList[menuItem].DueDate}");
            }
            SearchList.Clear();
            StartHere();
        }

        //Searches the list of media based on a keyword entered by the user.
        public void SeachByTitle()
        {
            string text = UserInput("Title");
            //bool checker;
            foreach (Media media in Media)
            {
                if (media.Title.ToLower().Contains(text.ToLower()))
                {
                    SearchList.Add(media);
                }
            }
            LibraryView.DisplayMedias(SearchList);
            // checking if the user wants to check out an item from the list

            
            
                //checker = Verification("Would you like to check out an item? ", yesOrNoError); //checker will be T/F

                if ( SearchList.Count > 0 && Verification("Would you like to check out an item? ", yesOrNoError))
                {
                    CheckoutMenu();
                }
                else
                {
                    StartHere();
                }
            

            
        } //end SearchByTitle

        //Displays a list of authors for the user to choose from.
        //The user enters the selected author, and associated media are returned.
        public void SearchByAuthor()
        {
            List<string> authors = new List<string>();
                        
            //bool checker;

            foreach (Media media in Media)
            {
                if (!authors.Contains(media.Author))
                {
                    authors.Add(media.Author);
                }
            }

            foreach (string author in authors)
            {
                Console.WriteLine(author);
            }

            string text = UserInput("Author");

            foreach (Media media in Media)
            {
                if (media.Author.ToLower().Contains(text.ToLower()))
                {
                    SearchList.Add(media);
                }
            }

            LibraryView.DisplayMedias(SearchList);
            //checker = Verification("Would you like to check out an item? ", yesOrNoError); //checker will be T/F

            if (SearchList.Count > 0 && Verification("Would you like to check out an item? ", yesOrNoError))
            {
                CheckoutMenu();
            }
            else
            {
                StartHere();
            }
                        
        } //end SearchByAuthor

        public void ReturnMedia()  //the user selects an item from the checkedout list to return an item
        {
            foreach (Media media in Media)
            {
                if (media.CheckedOut) //if the item is checked, add it to the checkoutlist.
                {
                    CheckoutList.Add(media);
                    
                }
            }

            if (CheckoutList.Count > 0)
            {
                LibraryView.DisplayMedias(CheckoutList); //diplay the checkout list
                int menuSelection = MenuSelection(1, CheckoutList.Count) - 1;

                //checking if the item is overdue

                if (DateTime.Now.CompareTo(CheckoutList[menuSelection].DueDate) >= 0)
                {
                    Console.WriteLine("This item is overdue.");
                    CheckoutList[menuSelection].CheckedOut = false;

                }
                else
                {
                    Console.WriteLine("Thank you for returning the item on time.");
                    CheckoutList[menuSelection].CheckedOut = false;
                }
            }
            else
            {
                Console.WriteLine("No items checked out");
            }

            CheckoutList.Clear();
            StartHere();

        }

        //Prompts the used to delete the saved file.
        public void DeleteSavedFile()
        {
            if (File.Exists($"../../../{fileName}.txt"))
            {
                if (Verification(confirmDeletion, yesOrNoError))
                {
                    File.Delete($"../../../{fileName}.txt");
                    LibraryView.PromptUser(success);
                }
            }
            else
            {
                LibraryView.PromptUser(fileNotFoundError);
            }
        } //end DeleteSavedFile

        //Shows the entire list of media in the "library".
        public void SubMenuOne(bool choice)
        {
            if (choice)
            {
                int item = MenuSelection(1, Media.Count);
                LibraryView.DisplayMedia(Media[item - 1]);
                if (!Media[item - 1].CheckedOut)
                {
                    if (Verification("Item is available. Would you like to check it out? ", yesOrNoError))
                    {
                        SearchList.Add(Media[item - 1]);
                        CheckoutMenu();
                    } else
                    {
                        StartHere();
                    }
                }
                else
                {
                Console.WriteLine($"This item is not available. It will be available by {Media[item-1].DueDate}");
                }
                MainMenu(1);
            }
            else
            {
                StartHere();
            }
        } //end SubMenuOne

        //Capitalizes the first letter of the input text, and puts the remainder to lower case.
        public string Indent(string text)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
        } //end Indent

        //Capitalizes the first letter of each word input by the user.
        public string MultipleWordIndent(string text)
        {
            string[] words;
            char[] delimiters = { ' ', '.', ':', '-' };
            StringBuilder newText = new StringBuilder();
            words = text.Split(delimiters);
            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    newText.Append(Indent(word));
                }
            }
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].ToString().ToLower() != newText[i].ToString().ToLower())
                {
                    newText.Insert(i, text[i]);
                }
            }
            return newText.ToString().Trim();
        } //end MultipleWords

    }
}

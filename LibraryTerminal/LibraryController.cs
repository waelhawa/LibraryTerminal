﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibraryTerminal
{
    class LibraryController
    {
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
        static readonly string [] menu = {"Display Library", "Add Media", "Remove Media", "Save to file", "Load from file", "Delete saved file", "Search by...", "Checkout", "Exit" };
        static readonly string[] searchMenu = { "Search by Title", "Search by Author", "Back to Main Menu" };
        public List<Media> Media { get; set; }
        public List<Media> SearchList { get; set; }
        public List<Media> CheckoutList { get; set; }

        public void Save(List<Media> list, string fileName)
        {
            StreamWriter writer = new StreamWriter($"../../../{fileName}.txt");
            foreach (Media media in list)
            {
                writer.WriteLine($"{media.Title}|{media.Author}|{media.MediaType}|{media.CheckedOut}|{media.DueDate}");
            }
            writer.Close();
        } //end Save

        //public void CheckSavedFile(List<Media> mediaList)
        //{
        //    if (File.Exists($"../../../{fileName}.txt"))
        //    {
        //        if (Verification(overrideConfirmation, yesOrNoError))
        //        {
        //            Save(mediaList, fileName);
        //        }
        //    }
        //    else
        //    {
        //        Save(mediaList, fileName);
        //    }
        //} //end CheckSavedFile

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

        public void Load(List<Media> mediaList, string fileName, bool preLoadVerified)
        {
            if (preLoadVerified || Verification(confirmLoad, yesOrNoError))
            {
                mediaList.Clear();
                StreamReader reader = ReadFile(fileName);
                if (reader != null)
                {
                    string text = reader.ReadLine().ToLower();
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
                        LibraryView.DisplayMedias(mediaList);
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

        public void AddToList(Media media)
        {
            Media.Add(media);
        } //end AddToList

        public void RemoveFromList(Media media)
        {
            Media.Remove(media);
            LibraryView.PromptUser(success);
        } //end RemoveFromList

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

        public void StartHere()
        {
            LibraryView.DisplayMainMenu(menu);
            int selection = MenuSelection(1, menu.Length);
            MainMenu(selection);
        } //end StartHere

        public string UserInput(string property)
        {
            string text;
            LibraryView.PromptUserAndWait($"Enter {property}: ");
            text = Console.ReadLine();
            return text.Trim().ToLower();
        } //end UserInput

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
                    break;
                default:
                    LibraryView.PromptUser(exitMessage);
                    break;
            }
        } //end MainMenu

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

        public void CheckoutMenu ()
        {

        }

        public void SeachByTitle()
        {
            string text = UserInput("Title");
            foreach (Media media in Media)
            {
                if (media.Title.ToLower().Contains(text.ToLower()))
                {
                    SearchList.Add(media);
                }
            }
            LibraryView.DisplayMedias(SearchList);
        } //end SearchByTitle

        public void SearchByAuthor()
        {
            string text = UserInput("Author");
            foreach (Media media in Media)
            {
                if (media.Author.ToLower().Equals(text.ToLower()))
                {
                    SearchList.Add(media);
                }
            }
            LibraryView.DisplayMedias(SearchList);
        } //end SearchByAuthor

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

        public void SubMenuOne(bool choice)
        {
            if (choice)
            {
                int item = MenuSelection(1, Media.Count);
                LibraryView.DisplayMedia(Media[item - 1]);
                MainMenu(1);
            }
            else
            {
                StartHere();
            }
        } //end SubMenuOne

        public string Indent(string text)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
        } //end Indent

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
using System;

namespace LibraryTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            //MainMenu mainMenu = new MainMenu();
            //mainMenu.MenuSelection();

            LibraryController controller = new LibraryController();
            controller.StartHere();
        }
    }
}

using System;
using System.Collections.Generic;

namespace LibraryTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            //MainMenu mainMenu = new MainMenu();
            //mainMenu.MenuSelection();

            LibraryController controller = new LibraryController();
            controller.StartHere();
=======
            LibraryController c = new LibraryController();
            c.Media = new List<Media>();
            c.CheckoutList = new List<Media>();
            c.SearchList = new List<Media>();
            c.PreLoad();
>>>>>>> 3d78fdc53848e55d4166f292ba23584249e07628
        }
    }
}

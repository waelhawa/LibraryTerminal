using System;
using System.Collections.Generic;

namespace LibraryTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            LibraryController c = new LibraryController();
            c.Media = new List<Media>();
            c.CheckoutList = new List<Media>();
            c.SearchList = new List<Media>();
            c.PreLoad();
        }
    }
}

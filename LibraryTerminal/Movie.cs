using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class Movie : Media
    {
        public Movie()
        {

        }

        public Movie(string title, string author, string mediaType) : base(title, author, mediaType)
        {

        }
        public Movie(string title, string author, string mediaType, bool checkedOut, DateTime dueDate) : base(title, author, mediaType, checkedOut, dueDate)
        {

        }
    }
}

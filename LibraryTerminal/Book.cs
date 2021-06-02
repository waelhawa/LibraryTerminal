using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class Book : Media
        // A class that inherits properties from the parent "Media" class.
    {
        public Book() 
        {

        }
        public Book(string title, string author, string mediaType) : base(title, author, mediaType)
        {

        }

        public Book(string title, string author, string mediaType, bool checkedOut, DateTime dueDate) : base(title, author, mediaType, checkedOut, dueDate)
        {

        }
    }
}

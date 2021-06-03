using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class CD : Media
    // A class that inherits properties from the parent "Media" class.
    {
        public CD()
        {

        }

        public CD(string title, string author, string mediaType) : base(title, author, mediaType)
        {

        }

        public CD(string title, string author, string mediaType, bool checkedOut, DateTime dueDate) : base(title, author, mediaType, checkedOut, dueDate)
        {

        }
    }
}

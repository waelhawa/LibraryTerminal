using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class Magazine : Media
    // A class that inherits properties from the parent "Media" class.
    {
        public Magazine()
        {

        }

        public Magazine(string title, string author, string mediaType) : base(title, author, mediaType)
        {

        }

        public Magazine(string title, string author, string mediaType, bool checkedOut, DateTime dueDate) : base(title, author, mediaType, checkedOut, dueDate)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class Magazine : Media 
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

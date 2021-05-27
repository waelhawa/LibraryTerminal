using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    abstract class Media
    {
        public string Title { get; set; }
        public string Author{ get; set; }
        public string MediaType { get; set; }
        public bool CheckedOut { get; set; }
        public DateTime DueDate { get; set; }

        public Media()
        {

        }

        public Media(string title, string author, string mediaType, bool checkedOut, DateTime dueDate)
        {
            Title = title;
            Author = author;
            MediaType = mediaType;
            CheckedOut = checkedOut;
            DueDate = dueDate;

        }

        public Media(string title, string author, string mediaType)
        {
            Title = title;
            Author = author;
            MediaType = mediaType;
            CheckedOut = false;

        }
    }
} //Media something = new Book or CD or...()

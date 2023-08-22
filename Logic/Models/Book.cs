using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Book : Items
    {
        public string Author { get; set; }

        public Book(string title, string author, Genres genre, string publisher, DateTime releaseDate, double price) : base(title, genre, publisher, releaseDate, price)
        {
            Author = author;
        }

        public override string ToString()
        {
            string rentStatus;

            if (isRented)
                rentStatus = "rented";
            else
                rentStatus = "not rented";

            return $"{Title}, by {Author} [{rentStatus}]";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Magazine : Items
    {
       

    public Magazine(string title, DateTime releaseDate, Genres genre, string publisher, double price) : base(title, genre, publisher, releaseDate, price)
        {
        }

        public override string ToString()
        {
            string rentStatus;

            if (isRented)
                rentStatus = "rented";
            else
                rentStatus = "not rented";

            return $"{Title}, {ReleaseDate:d} [{rentStatus}]";
        }
    }
}

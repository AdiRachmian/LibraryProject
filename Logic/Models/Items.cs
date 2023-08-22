using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class Items : IComparable
    {
        public string Title { get; set; }
        public Genres Genre { get; set; }
        public string Publisher { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }

        public int ISBN { get; set; }

        private static int codeGenerator = 1100;

        public bool isRented = false;
        public DateTime RentDate { get; set; }

        public bool isLate = false;

        public Items(string title, Genres genre, string publisher, DateTime releaseDate, double price)
        {
            Title = title;
            Genre = genre;
            Publisher = publisher;
            ReleaseDate = releaseDate;
            Price = price;
            FinalPrice = price;
            ISBN = codeGenerator++;
        }

        public override string ToString()
        {
            return Title;
        }

        public int CompareTo(object obj)
        {
            Items item = obj as Items;

            return String.Compare(this.Title, item.Title);
        }
    }
}

using LibraryProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Logic
{
    public class Manager
    {
        private User currUser;
        private SaleManager s_mngr;

        public Manager()
        {
            Inventory[0].isRented = true;
            Inventory[0].RentDate = new DateTime(2023, 2, 2);

            s_mngr = new SaleManager();
            s_mngr.AddSale(new Sale(SaleType.Genre, Genres.Newspaper.ToString(), 20), Inventory);
            s_mngr.AddSale(new Sale(SaleType.Author, "J.K. Rowling", 30), Inventory);
        }

        public List<Items> Inventory = new List<Items>()
        {
            // new Book("", "", Genres., "",new DateTime(, , ), ),
            // new Magazine("", Genres., "",new DateTime(, , ), ),

            new Book("Harry Potter and the Philosopher’s Stone", "J.K. Rowling", Genres.Fiction, "Bloomsbury", new DateTime(1997, 6, 26), 20),
            new Book("Harry Potter and the Chamber of Secrets", "J.K. Rowling", Genres.Fiction, "Bloomsbury", new DateTime(1998, 2, 7), 20),
            new Book("Harry Potter and the Prisoner of Azkaban", "J.K. Rowling", Genres.Fiction, "Bloomsbury", new DateTime(1999, 8, 7), 20),
            new Book("The Perks of Being a Wallflower", "Stephen Chbosky", Genres.YoungAdult, "Pocket Books", new DateTime(1999, 1, 2), 15),
            new Book("Bird Box", "Josh Malerman", Genres.Thriller, "Harper Voyager", new DateTime(1997, 3, 27), 25),
            new Magazine("La'isha", new DateTime(2023, 2, 1), Genres.Fashion, "Yediot", 10),
            new Magazine("La'isha", new DateTime(2020, 9, 1), Genres.Fashion, "Yediot", 10),
            new Magazine("Maariv Lenoar", new DateTime(2006, 5, 1), Genres.YoungAdult, "Maariv", 9),
            new Magazine("Maariv Lenoar", new DateTime(2006, 6, 1), Genres.YoungAdult, "Maariv", 9),
            new Magazine("Yediot Aharonot", new DateTime(2023, 5, 1), Genres.Newspaper, "Yediot", 5),
            new Magazine("Yediot Aharonot", new DateTime(2023, 5, 2), Genres.Newspaper, "Yediot", 5),
            new Magazine("Yediot Aharonot", new DateTime(2023, 5, 3), Genres.Newspaper, "Yediot", 5)
        };

        public List<User> UserList = new List<User>()
        {
            new User("admin", "asdf", true),
            new User("user", "1234", false)
        };

        public SaleManager GetSaleManager()
        {
            return s_mngr;
        }

        public bool IsLibrarian()
        {
            return currUser.IsLibrarian;
        }

        public bool Login(string username, string password)
        {
            foreach (User user in UserList)
            {
                if (user.Username == username && user.Password == password)
                {
                    currUser = user;
                    return true;
                }
            }

            return false;
        }

        public void AddItem(Items item)
        {
            Inventory.Add(item);
        }

        public void RemoveItem(Items item)
        {
            if (item != null)
                Inventory.Remove(item);
        }

        public void EditItem(Items item, string field, string input)
        {
            switch (field)
            {
                case "title":
                    item.Title = input;
                    break;
                case "author":
                    ((Book)item).Author = input;
                    break;
                case "genre":
                    Genres g = (Genres)Enum.Parse(typeof(Genres), input);
                    item.Genre = g;
                    break;
                case "publisher":
                    item.Publisher = input;
                    break;
                case "price":
                    item.Price = double.Parse(input);
                    break;
                case "releaseDate":
                    var parsedDate = DateTime.Parse(input);
                    item.ReleaseDate = parsedDate;
                    break;
                default:
                    break;
            }
        }

        public List<Items> Search(string field, string input)
        {
            List<Items> SearchList = new List<Items>();

            string lowercaseInput = input.ToLower();

            switch (field)
            {
                case "Show All":
                    SearchList = Inventory;
                    break;
                case "Title":
                    SearchList = Inventory.FindAll((Items item) => item.Title.ToLower().Contains(lowercaseInput));
                    break;
                case "Author":
                    SearchList = Inventory.FindAll((Items item) => item is Book && ((Book)item).Author.ToLower().Contains(lowercaseInput));
                    break;
                case "Genre":
                    SearchList = Inventory.FindAll((Items item) => item.Genre == ((Genres)Enum.Parse(typeof(Genres), input)));
                    break;
                case "Publisher":
                    SearchList = Inventory.FindAll((Items item) => item.Publisher.ToLower().Contains(lowercaseInput));
                    break;
            }

            return SearchList;
        }

        public void RentItem(Items item)
        {
            if (item != null)
            {
                item.isRented = true;
                item.RentDate = DateTime.Now;
            }
        }

        public void ReturnItem(Items item)
        {
            if (item != null)
            {
                item.isRented = false;
                item.isLate = false;
            }
        }

        public void IsLate(Items item)
        {
            if (item.isRented == true && item.RentDate.AddDays(14) < DateTime.Now.Date)
                item.isLate = true;
        }

        public DailyReport CreateDailyReport()
        {
            DailyReport report = new DailyReport
            {
                itemsCount = Inventory.Count,
                booksCount = Inventory.Where(item => item is Book).Count(),
                magazineCount = Inventory.Where(item => item is Magazine).Count(),
                rentedCount = Inventory.Where(item => item.isRented).Count()
            };

            return report;
        }
    }
}

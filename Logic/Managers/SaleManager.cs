using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace Logic
{
    public class SaleManager
    {
        public List<Sale> SalesList = new List<Sale>();

        public SaleManager()
        {
            
        }

        public void AddSale(Sale sale, List<Items> inventory)
        {
            SalesList.Add(sale);
            ManageSale(sale, inventory, true);
        }

        public void EndSale(Sale sale, List<Items> inventory)
        {
            SalesList.Remove(sale);
            ManageSale(sale, inventory, false);
        }

        public void ManageSale(Sale sale, List<Items> inventory, bool isStart)
        {
            if (sale.Type is SaleType.Publisher)
            {
                foreach (var item in inventory)
                {
                    if(item.Publisher == sale.Input)
                    {
                        item.FinalPrice = CalcFinalPrice(item, sale.DiscountPrecentage, isStart);
                    }
                }
            }

            if (sale.Type is SaleType.Author)
            {
                foreach (var book in inventory)
                {
                    if (book is Book && (book as Book).Author == sale.Input)
                    {
                        book.FinalPrice = CalcFinalPrice(book, sale.DiscountPrecentage, isStart);
                    }
                }
            }

            if (sale.Type is SaleType.Genre)
            {
                string genre;

                foreach (var item in inventory)
                {
                    genre = item.Genre.ToString();

                    if (genre == sale.Input)
                    {
                        item.FinalPrice = CalcFinalPrice(item, sale.DiscountPrecentage, isStart);
                    }
                }
            }

            if (sale.Type is SaleType.Date)
            {
                DateTime date;
                date = DateTime.Parse(sale.Input);

                foreach (var item in inventory)
                {
                    if (item.ReleaseDate.Date == date.Date)
                    {
                        item.FinalPrice = CalcFinalPrice(item, sale.DiscountPrecentage, isStart);
                    }
                }
            }

            if (sale.Type is SaleType.Year)
            {
                int year;
                year = DateTime.Parse(sale.Input).Year;

                foreach (var item in inventory)
                {

                    if (item.ReleaseDate.Year == year)
                    {
                        item.FinalPrice = CalcFinalPrice(item, sale.DiscountPrecentage, isStart);
                    }
                }
            }
        }

        public double CalcFinalPrice(Items item, int precentage, bool isStart)
        {
            double finalPrice;

            if (isStart)
            {
                finalPrice = item.Price * ((100.0 - precentage) / 100.0);
                return finalPrice < item.FinalPrice ? finalPrice : item.FinalPrice;
            }

            else
                return item.Price;
        }
    }
}

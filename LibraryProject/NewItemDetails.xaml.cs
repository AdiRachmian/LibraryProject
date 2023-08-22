using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LibraryProject
{
    public sealed partial class NewItemDetails : Page
    {
        Manager mngr;
        Items Item;
        private bool isBook, isEdit;

        public NewItemDetails()
        {
            this.InitializeComponent();

            string[] genreNames = Enum.GetNames(typeof(Genres));

            for (int i = 0; i < genreNames.Length; i++)
                GenreField.Items.Add(genreNames[i]);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Tuple<Manager, Items, bool> tuple = e.Parameter as Tuple<Manager, Items, bool>;
            mngr = tuple.Item1;
            if (tuple.Item2 != null)
            {
                Item = tuple.Item2;
                isEdit = true;
            }
            isBook = tuple.Item3;

            if(!isBook)
            {
                AuthorBlock.Visibility = Visibility.Collapsed;
                AuthorField.Visibility = Visibility.Collapsed;
            }

            if (isEdit)
            {
                if (isBook)
                    AuthorField.Text = (Item as Book).Author;

                TitleField.Text = Item.Title;
                PublisherField.Text = Item.Publisher;
                GenreField.SelectedItem = Item.Genre.ToString();
                ReleaseDateField.Date = Item.ReleaseDate;
                PriceField.Text = Item.Price.ToString();
            }
        }

        private void PriceField_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            double tempDouble;
            args.Cancel = !(double.TryParse(args.NewText, out tempDouble) | args.NewText == "");
        }

        private void SubmitItemButton_Click(object sender, RoutedEventArgs e)
        {          
            StringBuilder sb = new StringBuilder();

            if ((AuthorField.Text == null || AuthorField.Text.Length < 1) && isBook)
                sb.AppendLine("Author must have a value.");
            if (TitleField.Text == null || TitleField.Text.Length < 1)
                sb.AppendLine("Title must have a value.");
            if (PublisherField.Text == null || PublisherField.Text.Length < 1)
                sb.AppendLine("Publisher must have a value.");
            if (GenreField.SelectedItem == null)
                sb.AppendLine("Genre must have a value.");
            if (ReleaseDateField.SelectedDate == null)
                sb.AppendLine("Release date must have a value.");
            if (PriceField.Text == null || PriceField.Text.Length < 1)
                sb.AppendLine("Price must have a value.");            
            if (sb.ToString().Length > 20)
                MainPage.ShowMessageBox(sb.ToString());

            double parsedPrice;
            double.TryParse(PriceField.Text, out parsedPrice);

            string stringDate = ReleaseDateField.SelectedDate.Value.ToString("d");
            DateTime parsedDate;
            parsedDate = DateTime.Parse(stringDate);

            if (isEdit)
            {
                if (isBook)
                    mngr.EditItem(Item, "author", AuthorField.Text);

                mngr.EditItem(Item, "title", TitleField.Text);
                mngr.EditItem(Item, "genre", GenreField.SelectedItem.ToString());
                mngr.EditItem(Item, "publisher", PublisherField.Text);
                mngr.EditItem(Item, "price", PriceField.Text);
                Item.FinalPrice = Item.Price;
                mngr.EditItem(Item, "releaseDate", stringDate);

                Submitted();

                return;
            }

            if (isBook)
            {
                mngr.AddItem(new Book(title: TitleField.Text,
                    author: AuthorField.Text,
                    genre: (Genres)Enum.Parse(typeof(Genres), GenreField.SelectedItem.ToString()), 
                    publisher: PublisherField.Text,
                    releaseDate: parsedDate,
                    price: parsedPrice));
                Submitted();
            }

            else
            {
                mngr.AddItem(new Magazine(title: TitleField.Text,
                    genre: (Genres)Enum.Parse(typeof(Genres), GenreField.SelectedItem.ToString()),
                    publisher: PublisherField.Text,
                    releaseDate: parsedDate,
                    price: parsedPrice));
                Submitted();
            }
        }

        public void Submitted()
        {
            MainPage.ShowMessageBox("Item submitted successfully");
            Frame.Navigate(typeof(Inventory), mngr);
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Inventory), mngr);
        }

        private void DailyReport_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DailyReportPage), mngr);
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddItem), mngr);
        }

        private void ManageSales_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SaleManagerPage), mngr);
        }
    }
}

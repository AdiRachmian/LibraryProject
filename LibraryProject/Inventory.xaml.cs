using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LibraryProject
{
    public sealed partial class Inventory : Page
    {
        private Manager mngr;
        Items item;

        public Inventory()
        {
            this.InitializeComponent();

            string[] genreNames = Enum.GetNames(typeof(Genres));

            for (int i = 0; i < genreNames.Length; i++)
                GenreComboBox.Items.Add(genreNames[i]);

            FilterComboBox.Items.Add("Show All");
            FilterComboBox.Items.Add("Title");
            FilterComboBox.Items.Add("Author");
            FilterComboBox.Items.Add("Genre");
            FilterComboBox.Items.Add("Publisher");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mngr = e.Parameter as Manager;

            InventoryList.ItemsSource = null;
            mngr.Inventory.Sort();
            InventoryList.ItemsSource = mngr.Inventory;

            FilterTextBox.IsEnabled = false;
            GenreComboBox.Visibility = Visibility.Collapsed;

            if (!mngr.IsLibrarian())
            {
                EditButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
                TopBar.Visibility = Visibility.Collapsed;
            }
        }

        public string ItemDetails(Items item)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"ISBN: {item.ISBN}\n");
            sb.AppendLine($"Title: {item.Title}\n");
            if(item is Book)
                sb.AppendLine($"Written by: {((Book)item).Author}\n");
            sb.AppendLine($"Genre: {item.Genre}\n");
            sb.AppendLine($"Publisher: {item.Publisher}\n");
            sb.AppendLine($"Release Date: {item.ReleaseDate:d}\n");
            if(item.Price == item.FinalPrice)
                sb.AppendLine($"Price: {item.Price:c}\n");
            else
            {
                sb.AppendLine($"Price: {item.Price:c}\n");
                sb.AppendLine($"Final Price: {item.FinalPrice:c}\n");
            }
            if (item.isRented)
                sb.AppendLine($"Rent date: {item.RentDate:d}");

            return sb.ToString();
        }

        private void InventoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            item = InventoryList.SelectedItem as Items;
            
            if(item != null)
            {
                DetailsBlock.Text = ItemDetails(item);

                mngr.IsLate(item);
                if (item.isLate)
                    LateBox.Text = "LATE RETURN";
                else
                    LateBox.Text = "";
            }
        }

        private void RentButton_Click(object sender, RoutedEventArgs e)
        {
            item = InventoryList.SelectedItem as Items;

            if(item == null)
            {
                MainPage.ShowMessageBox("Please select an item to rent");
                return;
            }

            if (item != null && item.isRented)
                MainPage.ShowMessageBox("Sorry! Item is already rented");
            else
            {
                mngr.RentItem(item);
                InventoryList.ItemsSource = null;
                InventoryList.ItemsSource = mngr.Inventory;
                DetailsBlock.Text = "";
                MainPage.ShowMessageBox("Item rented successfully");
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            item = InventoryList.SelectedItem as Items;

            if (item == null)
            {
                MainPage.ShowMessageBox("Please select an item to return");
                return;
            }

            if (item.isRented == false)
                MainPage.ShowMessageBox("Sorry! The item isn't currently rented");
            else
            {
                mngr.ReturnItem(item);
                InventoryList.ItemsSource = null;
                InventoryList.ItemsSource = mngr.Inventory;
                DetailsBlock.Text = "";
                LateBox.Text = "";
                MainPage.ShowMessageBox("Item returned successfully");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            item = InventoryList.SelectedItem as Items;
            Tuple<Manager, Items, bool> tuple;

            if (item == null)
            {
                MainPage.ShowMessageBox("Please select an item to edit");
                return;
            }

            if (item is Book)
                tuple = new Tuple<Manager, Items, bool>(mngr, item, true);
            else
                tuple = new Tuple<Manager, Items, bool>(mngr, item, false);

            Frame.Navigate(typeof(NewItemDetails), tuple);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            item = InventoryList.SelectedItem as Items;

            if (item == null)
            {
                MainPage.ShowMessageBox("Please select an item to delete");
            }
            else
            {
                mngr.RemoveItem(item);
                InventoryList.ItemsSource = null;
                InventoryList.ItemsSource = mngr.Inventory;
                DetailsBlock.Text = "";
                LateBox.Text = "";
                MainPage.ShowMessageBox("Item deleted successfully");
            }
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InventoryList.ItemsSource = null;

            switch (FilterComboBox.SelectedItem)
            {
                case "Show All":
                    InventoryList.ItemsSource = mngr.Search("Show All", null);
                    break;
                case "Title":
                    InventoryList.ItemsSource = mngr.Search("Title", FilterTextBox.Text);
                    break;
                case "Author":
                    InventoryList.ItemsSource = mngr.Search("Author", FilterTextBox.Text);
                    break;
                case "Publisher":
                    InventoryList.ItemsSource = mngr.Search("Publisher", FilterTextBox.Text);
                    break;
                default:
                    break;
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InventoryList.ItemsSource = mngr.Inventory;

            if (FilterComboBox.SelectedItem.ToString() == "Show All")
            {
                GenreComboBox.Visibility = Visibility.Collapsed;
                FilterTextBox.IsEnabled = false;
            }
            else
            {
                FilterTextBox.IsEnabled = true;
            }

            if (FilterComboBox.SelectedItem.ToString() == "Title")
            {
                InventoryList.ItemsSource = mngr.Search("Title", "");
            }

            if (FilterComboBox.SelectedItem.ToString() == "Author")
            {
                InventoryList.ItemsSource = mngr.Search("Author", "");
            }

            if (FilterComboBox.SelectedItem.ToString() == "Genre")
            {
                FilterTextBox.Visibility = Visibility.Collapsed;
                GenreComboBox.Visibility = Visibility.Visible;
            }
            else
            {
                FilterTextBox.Visibility = Visibility.Visible;
                GenreComboBox.Visibility = Visibility.Collapsed;
            }

        }

        private void GenreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InventoryList.ItemsSource = mngr.Search("Genre", GenreComboBox.SelectedItem.ToString());
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

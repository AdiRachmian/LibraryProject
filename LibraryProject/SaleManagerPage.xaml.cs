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
    public sealed partial class SaleManagerPage : Page
    {
        Manager mngr;
        SaleManager s_mngr;
        Sale sale;

        public SaleManagerPage()
        {
            this.InitializeComponent();

            string[] genreNames = Enum.GetNames(typeof(Genres));
            for (int i = 0; i < genreNames.Length; i++)
                GenreComboBox.Items.Add(genreNames[i]);

            FilterComboBox.Items.Add("Genre");
            FilterComboBox.Items.Add("Author");
            FilterComboBox.Items.Add("Publisher");
            FilterComboBox.Items.Add("Release date");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mngr = e.Parameter as Manager;
            s_mngr = mngr.GetSaleManager();
            MainPage.UpdateListView(SalesList, s_mngr.SalesList);
            InputTextBox.IsEnabled = false;
            GenreComboBox.Visibility = Visibility.Collapsed;
            EndSaleButton.Visibility = Visibility.Collapsed;
            DiscountByDatePicker.Visibility = Visibility.Collapsed;
        }

        public string SaleDetails(Sale sale)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Type: by {sale.Type}\n");
            sb.AppendLine($"{sale.Type}: {sale.Input}\n");
            sb.AppendLine($"Discount: {sale.DiscountPrecentage}%\n");

            return sb.ToString();
        }

        private void SalesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sale = SalesList.SelectedItem as Sale;

            if (sale != null)
            {
                EndSaleButton.Visibility = Visibility.Visible;
            }
        }

        private void AddSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterComboBox.SelectedItem == null ||
                (string.IsNullOrEmpty(InputTextBox.Text) && GenreComboBox.SelectedItem == null && DiscountByDatePicker.SelectedDate == null) ||
                string.IsNullOrEmpty(PrecentageBox.Text))
            {
                MainPage.ShowMessageBox("Plese fill all the fields to continue");
                return;
            }

            int parsedPrecentage = int.Parse(PrecentageBox.Text);

            if (parsedPrecentage < 1 || parsedPrecentage > 100)
            {
                MainPage.ShowMessageBox("Precentage must have a value between 1-100");
                return;
            }

            if (FilterComboBox.SelectedItem.ToString() == "Genre")
            {
                sale = new Sale(type: SaleType.Genre,
                    input: GenreComboBox.SelectedItem.ToString(),
                    discountPrecentage: int.Parse(PrecentageBox.Text));
                s_mngr.AddSale(sale, mngr.Inventory);
            }

            else if(FilterComboBox.SelectedItem.ToString() == "Release date")
            {
                DateTime formattedDate = DiscountByDatePicker.SelectedDate.Value.Date;

                sale = new Sale(type: SaleType.Date,
                    input: formattedDate.ToShortDateString(),
                    discountPrecentage: int.Parse(PrecentageBox.Text));
                s_mngr.AddSale(sale, mngr.Inventory);
            }

            else
            {
                sale = new Sale(type: (SaleType)Enum.Parse(typeof(SaleType), FilterComboBox.SelectedItem.ToString()),
                    input: InputTextBox.Text,
                    discountPrecentage: int.Parse(PrecentageBox.Text));
                s_mngr.AddSale(sale, mngr.Inventory);
            }

            MainPage.UpdateListView(SalesList, s_mngr.SalesList);
            MainPage.ShowMessageBox("Sale added successfully");
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(FilterComboBox.SelectedItem.ToString() == "Genre")
            {
                InputTextBox.Visibility = Visibility.Collapsed;
                DiscountByDatePicker.Visibility = Visibility.Collapsed;
                GenreComboBox.Visibility = Visibility.Visible;
            }
            else if (FilterComboBox.SelectedItem.ToString() == "Release date")
            {
                InputTextBox.Visibility = Visibility.Collapsed;
                GenreComboBox.Visibility = Visibility.Collapsed;
                DiscountByDatePicker.Visibility = Visibility.Visible;
            }
            else
            {
                GenreComboBox.Visibility = Visibility.Collapsed;
                DiscountByDatePicker.Visibility = Visibility.Collapsed;
                InputTextBox.Visibility = Visibility.Visible;
                InputTextBox.IsEnabled = true;
            }
        }

        private void PrecentageBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void EndSaleButton_Click(object sender, RoutedEventArgs e)
        {
            s_mngr.EndSale(SalesList.SelectedItem as Sale, mngr.Inventory);
            MainPage.UpdateListView(SalesList, s_mngr.SalesList);
            MainPage.ShowMessageBox("Sale ended successfully");
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

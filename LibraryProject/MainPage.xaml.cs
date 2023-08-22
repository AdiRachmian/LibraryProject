using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Logic;
using System.Threading;
using System.Globalization;
using Windows.UI.Popups;

namespace LibraryProject
{
    public sealed partial class MainPage : Page
    {
        Manager mngr;

        public MainPage()
        {
            this.InitializeComponent();

            mngr = new Manager();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-IL");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(UsernameInput.Text) || string.IsNullOrEmpty(PasswordInput.Password))
            {
                ShowMessageBox("Please enter username and password");
                return;
            }

            bool loginSuccess = mngr.Login(UsernameInput.Text, PasswordInput.Password);

            if(loginSuccess)
            {
                Frame.Navigate(typeof(Inventory), mngr);
                return;
            }
            else
            {
                ShowMessageBox("Incorrect username or password");
                return;
            }
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Inventory), mngr);
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddItem), mngr);
        }

        private void DailyReport_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DailyReportPage), mngr);
        }

        private void ManageSales_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SaleManagerPage), mngr);
        }

        async static public void ShowMessageBox(string message)
        {
            var messageDialog = new MessageDialog(message);
            await messageDialog.ShowAsync();
        }

        public static void UpdateListView<T>(ListView listView, List<T> sourceList)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = sourceList;
        }
    }
}

using Logic;
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

namespace LibraryProject
{
    public sealed partial class AddItem : Page
    {
        Tuple<Manager, Items, bool> tuple;
        Manager mngr;

        public AddItem()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mngr = e.Parameter as Manager;
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            tuple = new Tuple<Manager, Items, bool>(mngr, null,true);
            Frame.Navigate(typeof(NewItemDetails), tuple);
        }

        private void AddMagazine_Click(object sender, RoutedEventArgs e)
        {
            tuple = new Tuple<Manager, Items, bool>(mngr, null, false);
            Frame.Navigate(typeof(NewItemDetails), tuple);
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

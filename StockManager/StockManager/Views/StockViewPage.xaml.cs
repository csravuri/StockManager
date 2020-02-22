using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StockManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StockViewPage : ContentPage
	{
        private ObservableCollection<Item> _itemsFromDB;
        private App app = Application.Current as App;

        public StockViewPage ()
		{
			InitializeComponent ();            
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                LoadItemsFromDB();
                this.searchBar.Text = "";
            }
            catch (SQLiteException e)
            {
                // nothing
            }
        }

        private async void LoadItemsFromDB()
        {
            if (app.StockCreateDB == null)
            {
                app.StockCreateDB = new StockCreateDB();
            }

            try
            {
                var abc = await app.StockCreateDB.GetItems();

                _itemsFromDB = new ObservableCollection<Item>(abc);
                this.listView.ItemsSource = _itemsFromDB;
            }
            catch(SQLiteException ee)
            {
                //
            }
        }


        private void CreateNewStock(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StockCreatePage("Create Item"));
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue) && _itemsFromDB != null)
            {
                this.listView.ItemsSource = _itemsFromDB.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            else
            {
                this.listView.ItemsSource = _itemsFromDB;                
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Item item = e.Item as Item;
            Navigation.PushAsync(new StockCreatePage("Update Item", item));
            
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.listView.SelectedItem = null;
        }

        private void OnItemDelete(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Item selectedItem = menuItem.CommandParameter as Item;

            _itemsFromDB.Remove(selectedItem);
            app.StockCreateDB.DeleteItem(selectedItem);
        }
    }
}
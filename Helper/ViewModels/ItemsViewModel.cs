using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Helper.Models;
using Helper.Views;

namespace Helper.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;                
                await DataStore.SaveItemAsync(newItem);
                LoadItemsCommand.Execute(null);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Item>(this, "AddItem", async (obj, Item) =>
            {
                Item newItem = Item as Item;
                await DataStore.SaveItemAsync(newItem);
                LoadItemsCommand.Execute(null);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Item>(this, "DeleteItem", async (obj, Item) =>
            {
                Item currentItem = Item as Item;
                Items.Remove(currentItem);
                await DataStore.DeleteItemAsync(currentItem);

            });


        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(); //Exeption!!!
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
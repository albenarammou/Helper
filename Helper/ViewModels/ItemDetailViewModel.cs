using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

using Helper.Models;
using Helper.Views;

namespace Helper.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        //public Item Item { get; set; }
        //public ItemDetailViewModel(Item item = null)
        //{
        //    Title = item?.Text;
        //    Item = item;
        //}
        public Item Item { get; set; }
        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; set; }

        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());


            MessagingCenter.Subscribe<ItemDetailPage, Item>(Item, "AddItem", async (obj, Item) =>
            {
                Item newItem = Item;
                await DataStore.SaveItemAsync(newItem);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Item>(Item, "DeleteItem", async (obj, Item) =>
            {
                Item currentItem = Item as Item;
                Items.Remove(currentItem);
                await DataStore.DeleteItemAsync(currentItem);

            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            Items.Clear();
            var items = await DataStore.GetItemsAsync();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }


    }
}

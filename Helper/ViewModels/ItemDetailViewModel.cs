using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

using Helper.Models;
using Helper.Views;

namespace Helper.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}

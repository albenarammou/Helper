using System;
using System.ComponentModel;
using Xamarin.Forms;
using Helper.Models;
using Helper.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Helper.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;
        public Item Item { get; set; }
        private MediaFile _mediaFile;
        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            Item = viewModel.Item;  //me

        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            //await Navigation.PopAsync();
            await Navigation.PopModalAsync();
        }
        async void Delete_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteItem", Item);
            //await Navigation.PopAsync();
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void PickUpPhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No PickPhoto", ":(No PickPhoto available.", "OK");
                return;
            }
            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null) return;
            Url.Text = _mediaFile.Path;

            FileImage.Source = ImageSource.FromStream(() => { return _mediaFile.GetStream(); });
        }
        async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Upload_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
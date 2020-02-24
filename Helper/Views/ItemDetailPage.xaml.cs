using System;
using System.ComponentModel;
using Xamarin.Forms;
using Helper.Models;
using Helper.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Net.Http;

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
            await Navigation.PopModalAsync();
        }
        async void Delete_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteItem", Item);
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
            FileImage.Source = ImageSource.FromFile(_mediaFile.Path);
        }
        async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":(No camera available.", "OK");
                return;
            }
            _mediaFile = await CrossMedia.Current.TakePhotoAsync( new StoreCameraMediaOptions { Directory = "Sample", Name="myImage.jpg"});
            if (_mediaFile == null) return;
            Url.Text = _mediaFile.Path;
            FileImage.Source = ImageSource.FromFile(_mediaFile.Path);
            //FileImage.Source = ImageSource.FromStream(() => { return _mediaFile.GetStream(); });
        }
        private async void Upload_Clicked(object sender, EventArgs e)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(_mediaFile.GetStream()),
            "\"file\"",
            $"\"{_mediaFile.Path}\"");

            var httpClient = new HttpClient();
            var uploadServiceBaseAddress = "https://192.168.0.37:5001/api/Files/Uploads";
            var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
            Url.Text = await httpResponseMessage.Content.ReadAsStringAsync();
        }

    }
}
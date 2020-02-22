using System;
using System.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using StockManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StockCreatePage : ContentPage
	{
        private Item _item;
        private App app = Application.Current as App;

        public StockCreatePage (string Title, Item item = null)
		{
			InitializeComponent ();
            this.Title = Title;
            _item = item ?? new Item();
            BindingContext = _item;

		}

        protected override void OnAppearing()
        {
            this.itemImage.Source = this.itemImage.Source ?? Utils.GetDefaultImage();

            base.OnAppearing();            
        }

        private async void CaptureImage(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }            

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Full,
                    Name = Utils.GetDateTimeFileName(".jpg"),
                    AllowCropping = true
                    
                });

                if (file == null)
                    return;

                _item.ImagePath = file.Path;
                this.itemImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
            catch(Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }

            
        }

        private async void OnSave(object sender, EventArgs e)
        {
            if(isCreateFormValid())
            {
                if (app.StockCreateDB == null)
                {
                    app.StockCreateDB = new StockCreateDB();
                }

                if (_item.ItemID != 0)
                {
                    app.StockCreateDB.UpdateItem(_item);
                    await DisplayAlert("Success", $"{_item.Name} updated.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    app.StockCreateDB.InsertItem(_item);
                    await DisplayAlert("Success", $"{_item.Name} saved.", "OK");
                }

                ClearControls();
            }
        }

        private bool isCreateFormValid()
        {
            if(string.IsNullOrWhiteSpace(_item.Name))
            {
                DisplayAlert("Error", $"{this.itemName.Placeholder} is Required!", "OK");
                return false;
            }
            if(!isNumber(_item.Price))
            {
                DisplayAlert("Error", $"{this.price.Placeholder} should be number!", "OK");
                return false;
            }
            if(!isNumber(_item.StockQty))
            {
                DisplayAlert("Error", $"{this.stockQty.Placeholder} should be number!", "OK");
                return false;
            }

            return true;
        }

        private bool isNumber(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return true;
            }

            if(text.All(x => (x >= 48 || x == 46) && (x<= 57 || x == 46)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            var cancelButton = sender as Button;
            cancelButton.IsEnabled = false;
            Navigation.PopAsync();
        }

        private void ClearControls()
        {
            _item.ItemID = 0;
            _item.ImagePath = null;
            this.itemImage.Source = Utils.GetDefaultImage();
            this.itemName.Text = "";
            this.price.Text = null;
            this.stockQty.Text = null;
        }

    }
}
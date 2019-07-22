using AfonsoftIPTV.Interface;
using System;
using Xamarin.Forms;

namespace AfonsoftIPTV.Views
{
    public partial class PrismContentPageSettings : ContentPage
    {
        public static bool isPostBack = false;
        public PrismContentPageSettings()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!isPostBack)
            {
                isPostBack = true;

                

            }
        }

        private void LicenseID_Tapped(object sender, EventArgs e)
        {
            DependencyService.Get<IClipboardService>().SendTextToClipboard(LicenseID.Detail.Replace("ID: ", ""));
            DependencyService.Get<IMessage>().Alert("Licença Copiada para o Clipboard");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PATPALMaps
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Browser.Source = new UrlWebViewSource { Url =  "http://leyendas.gob.pe/zoologia/" };

        }


        private void backClicked(object sender, EventArgs e)
        {
            // Check to see if there is anywhere to go back to
            if (Browser.CanGoBack)
            {
                Browser.GoBack();
            }
            else
            { // If not, leave the view
                Navigation.PopAsync();
            }
        }

        private void forwardClicked(object sender, EventArgs e)
        {
            if (Browser.CanGoForward)
            {
                Browser.GoForward();
            }
        }
    }
}

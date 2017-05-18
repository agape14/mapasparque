using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PATPALMaps
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            CODI_USER.Text = "Bienvenid@s";
            cargarMenu();
        }
        private void cargarMenu()
        {

            List<MenuViewModel> menu = new List<MenuViewModel>();
                      
            menu.Add(new MenuViewModel()
            {
                Icon = App.urlIcons + "ic_shortcut_location_on.png",
                Title = "MAPAS",
                PageName = "MapsPage"
            });
          

            lstView.ItemsSource = menu;
            lstView.HasUnevenRows = true;
            lstView.ItemTemplate = new DataTemplate(() =>
            {
                Label title = new Label();
                title.SetBinding(Label.TextProperty, "Title");
                title.VerticalOptions = LayoutOptions.Center;
                title.TextColor = (Color)App.Current.Resources["MenuFontColor"];


                Image icon = new Image();
                icon.WidthRequest = 45;
                icon.HeightRequest = 45;
                icon.VerticalOptions = LayoutOptions.Center;
                icon.SetBinding(Image.SourceProperty, "Icon");

                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.ColumnDefinitions.Add((new ColumnDefinition { Width = GridLength.Auto }));
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                grid.Children.Add(icon, 0, 0);
                grid.Children.Add(title, 1, 0);

                return new ViewCell
                {
                    View = grid
                };
            });

            lstView.ItemSelected += LstView_ItemSelected;
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {

                lstView.SelectedItem = null;
                var value = e.SelectedItem as MenuViewModel;
                var pageName = value.PageName.ToString();
                App.Master.IsPresented = false;
                switch (pageName)
                {
                    case "MapsPage":
                        Navigate(new MapsPage());
                        break;
                }
            }
        }

        private void Navigate<T>(T page) where T : Page
        {
            App.Navigator.PushAsync(page);
        }


        public class MenuViewModel
        {
            public string Icon { get; set; }
            public string Title { get; set; }
            public string PageName { get; set; }
        }
    }
}
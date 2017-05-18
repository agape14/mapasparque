using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PATPALMaps.wspatapalmaps;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TK.CustomMap;

namespace PATPALMaps
{
    public partial class MapsPage : ContentPage
    {
        List<Clasifications> clasificacion;
        public MapsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cargarClasificaciones();
            busqueda("0","");


            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-12.0716539, -77.086058), Distance.FromKilometers(4)));
            MainMap.MoveToRegion(new MapSpan(new Position(-12.0716539, -77.086058), 0.01, 0.01)); /**/

        }

        private void busqueda(string iIdClasificacionn, string vTitulo)
        {
            MainMap.Pins.Clear();
            Service1Client wsmaps = new Service1Client();
            wsmaps.getListarPinesCompleted += Wsmaps_getListarPinesCompleted;
            wsmaps.getListarPinesAsync(iIdClasificacionn, vTitulo);
        }

        private void cargarClasificaciones() {
            Service1Client wsmaps = new Service1Client();
            wsmaps.getListarClasificacionesCompleted += Wsmaps_getListarClasificacionesCompleted; ;
            wsmaps.getListarClasificacionesAsync();
        }

        private void Wsmaps_getListarClasificacionesCompleted(object sender, getListarClasificacionesCompletedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                ToolbarItem item;
                this.ToolbarItems.Clear();
                clasificacion = new List<Clasifications>();
                clasificacion.Clear();
                if (e.Result != null)
                {
                    foreach (var List in e.Result)
                    {
                        clasificacion.Add(new Clasifications()
                        {
                            iIdClasificacion = List.iIdClasificacion,
                            vClasificacion = List.vClasificacion,
                            vIcono = List.vIcono
                        });
                    }

                    item = new ToolbarItem
                    {
                        Order = ToolbarItemOrder.Secondary,
                        Text = "TODOS",
                        ClassId = "0"
                    };
                    item.Clicked += Item_Clicked;
                    this.ToolbarItems.Add(item);

                    foreach (var datos in clasificacion)
                    {
                        item = new ToolbarItem
                        {
                            Order = ToolbarItemOrder.Secondary,
                            Text = datos.vClasificacion.ToString(),
                            ClassId = datos.iIdClasificacion.ToString()
                            //sIcon = datos.vIcono.ToString();

                        };
                        item.Clicked += Item_Clicked;
                        this.ToolbarItems.Add(item);
                    }
                }
            });
        }

        private void Item_Clicked(object sender, EventArgs e)
        {
            ToolbarItem obj = (ToolbarItem)sender;
            string iIdClasificacion = obj.ClassId.ToString();
            busqueda(iIdClasificacion, "");
        }

        private void Wsmaps_getListarPinesCompleted(object sender, getListarPinesCompletedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (e.Result != null)
                {

                    foreach (var pines in e.Result)
                    {
                        /*TKCustomMapPin pinCustom = new TKCustomMapPin();
                        pinCustom.Position= new Position(
                                Double.Parse(pines.vLongitud),
                                Double.Parse(pines.vLatitud));
                        pinCustom.Title= pines.vTitulo;
                        pinCustom.Image= Device.OnPlatform("icon.png", "icon.png", "Assets/icon.png");
                        MainMap.Pins.Add(pinCustom);*/
                        Pin MyPin = new Pin();
                        MyPin.Position = new Position(
                                Double.Parse(pines.vLongitud),
                                Double.Parse(pines.vLatitud));
                        MyPin.Label = pines.vTitulo;
                        MyPin.Address = pines.vDescripcion;

                        MainMap.Pins.Add(MyPin); 
                    }
                }
                else
                {
                    await DisplayAlert("Alerta", "No tiene marcadores para mostrar", "OK", "Salir");
                }
            });



        }

        internal class Part
        {
            public string PartName { get; set; }
            public int PartId { get; set; }
            public string direccion { get; set; }
            public Double longitud { get; set; }
            public Double latitud { get; set; }

        }

        public class CustomPin
        {
            public Pin Pin { get; set; }
            public string Id { get; set; }
            public string Url { get; set; }
        }

        public class Clasifications
        {
            public string iIdClasificacion { get; set; }
            public string vClasificacion { get; set; }
            public string vIcono { get; set; }
        }

        public class CustomMap : Map
        {
            public List<CustomPin> CustomPins { get; set; }
        }
        /*private async void btnVerUbicacion_Clicked(object sender, EventArgs e)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                if (locator.IsGeolocationEnabled)
                {
                    var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                    LongitudeLabel.Text = position.Longitude.ToString();
                    LatitudeLabel.Text = position.Latitude.ToString();
                }
                else
                {
                    //AlertDialog.Builder builder = new Alert.Builder(trhis);
                    //System.Diagnostics.Debug.WriteLine("Geolocation is disabled!");
                    await DisplayAlert("Atencion: ", "GPS esta deshabilitado", "Ok");
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                await DisplayAlert("Atencion: ", "No se puede obtener la ubicación, verifique si el GPS esta activo: ", "Ok");
            }
        } */
    }


}

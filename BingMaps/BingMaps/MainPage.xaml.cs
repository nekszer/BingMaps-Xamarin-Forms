using BingMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BingMaps
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();
            Web.LoadComplete += Web_LoadComplete;
            BtnRemoveAllPins.Clicked += BtnRemoveAllPins_Clicked;
        }

        private void BtnRemoveAllPins_Clicked(object sender, EventArgs e)
        {
            if (Web.IsLoad)
            {
                Web.Pins.Clear();
            }
        }

        private void Web_LoadComplete(object sender, EventArgs e)
        {
            if (sender is BingMapView view)
            {
                var base64image = "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";
                view.SetCenter(new Center(19.479778, -98.834520, 12));
                var pin = new Pin(19.479778, -98.834520) { Title = "Yo", Data = "Hola que hace!!" };
                
                pin.Click += Pin_Click;

                view.Pins.Add(pin);

                var pin2 = new Pin(19.459778, -98.834520) { Title = "xD", Data = "Pin 2!!" };
                pin2.Click += Pin_Click;

                view.Pins.Add(pin2);
                
                var pin3 = new Pin(19.469778, -98.734520) { Title = "xD", Data = "Pin 3!!" };
                pin3.Click += Pin_Click;

                view.Pins.Add(pin3);

                var pin4 = new Pin(19.489778, -98.634520) { Title = "xD", Data = "Pin 4!!" };
                pin4.Click += Pin_Click;

                view.Pins.Add(pin4);

                var pin5 = new Pin(19.419778, -98.934520) { Title = "xD", Data = "Pin 5!!" };
                pin5.Click += Pin_Click;

                view.Pins.Add(pin5);

                var pin6 = new Pin(19.469778, -98.894520) { Title = "xD", Data = "Pin 6!!" };
                pin6.Click += Pin_Click;

                view.Pins.Add(pin6);

                view.ZoomForAllPins();
            }
        }

        private async void Pin_Click(object sender, string e)
        {
            await DisplayAlert("Bing map", e, "Aceptar");
        }

    }
}

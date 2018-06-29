using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace BingMap
{
    public class BingMapView : View
    {

        /// <summary>
        /// Lista de pins
        /// </summary>
        public ObservableCollection<Pin> Pins { get; private set; }

        public BingMapView()
        {
            if (Pins == null) Pins = new ObservableCollection<Pin>();
            Pins.CollectionChanged += Pins_CollectionChanged;
        }

        /// <summary>
        /// Refrescamos los pins en el mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pins_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            RemoveAllPins();

            foreach (var item in Pins)
            {
                if(item is Pin pin)
                {
                    AddPin(pin);
                }
            }
        }

        public void ZoomForAllPins()
        {
            Action = Action.ZoomForAllPins;
            ReceiveAction?.Invoke(this, null);
        }

        /// <summary>
        /// Remueve todos los items del mapa
        /// </summary>
        private void RemoveAllPins()
        {
            Action = Action.RemoveAllPins;
            ReceiveAction?.Invoke(this, null);
        }

        /// <summary>
        /// Indica si el mapa se cargo o no
        /// </summary>
        public bool IsLoad { get; private set; }

        /// <summary>
        /// Esta variable almacena la action actual o la ultima accion lanzada por la vista
        /// </summary>
        public Action Action { get; private set; }

        /// <summary>
        /// Evento a ejecutar cuando se hace un LoadComplete del mapa
        /// </summary>
        public event EventHandler<EventArgs> LoadComplete;

        /// <summary>
        /// Se ejecuta al cargar el mapa
        /// </summary>
        public void OnLoadComplete(string load)
        {
            if (!string.IsNullOrEmpty(load))
            {
                IsLoad = load == "bingmapv8_loadcomplete";
                if (IsLoad)
                {
                    LoadComplete?.Invoke(this, EventArgs.Empty);
                    System.Diagnostics.Debug.WriteLine("Se ha cargado el mapa", "BingMapV8");
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Permite enviar objetos
        /// </summary>
        public event EventHandler<object> ReceiveAction;

        /// <summary>
        /// Permite asignar el centro del mapa
        /// </summary>
        /// <param name="center"></param>
        public void SetCenter(Center center)
        {
            Action = Action.SetCenter;
            ReceiveAction?.Invoke(this, center);
        }
        
        /// <summary>
        /// Agrega un pin
        /// </summary>
        /// <param name="pin"></param>
        private void AddPin(Pin pin)
        {
            Action = Action.AddPin;
            ReceiveAction?.Invoke(this, pin);
        }

        public T DeserializeObject<T>(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
    
}

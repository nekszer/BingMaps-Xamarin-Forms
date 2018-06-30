# BingMaps-Xamarin-Forms

# Bing Map v8
Para obtener la llave de desarrollador hay que dirigirse al siguiente link:
https://www.bingmapsportal.com

Para usar el plugin, solo importa cada contenido de la carpeta plugin al proyecto correspondiente.

# Android
La carpeta de BingMap debe ir dentro de Assets, y debe tener el archivo index.html la accion de compilacion como AndroidAsset 

Importar Mono.Android.Export.dll de los ensamblados del framework

# iOS
La carpeta BingMap se copia como esta, el archivo index.html debe tener la accion de compilacion como BundleResource en propiedades

# UWP
Solo copiar la carpeta BingMap en Assets

# BingMap/index.html
En este archivo hay que poner la api key que obtuvieron del portal de desarrolladores de Bing

# Funciones del plugin

```csharp

BingMapView view = new BingMapView();
... // add view to page
view.LoadComplete += View_LoadComplete;

```

Cuando el mapa haya cargado...

```csharp
private void View_Complete(object sender, EventArgs e){
  if (sender is BingMapView view){
    // Set Center of Map
    view.SetCenter(new Center(19.479778, 12.834520, 12));

    // Add Pin
    var pin = new Pin(19.479778, 12.834520) { Title = "Yo", Data = "Hola que hace!!" };
    // Evento click en el pin
    pin.Click += Pin_Click;
    view.Pins.Add(pin);

    // Hacer que el mapa muestre todos los pins agregados
    view.ZoomForAllPins();
  }
}
```

```csharp
private async void Pin_Click(object sender, string e)
{
    // sender es un Pin
    var pin = sender as Pin;
    // la variable e es igual a pin.Data // Hola que hace
    await DisplayAlert("Bing map", e, "Aceptar");
}
```

# Los Pins del mapa
La propiedad Pins del mapa, es observable por lo que, si eliminas un pin, el mapa se actualiza automaticamente, si limpiaz la propiedad igual se borran los pins del mapa.


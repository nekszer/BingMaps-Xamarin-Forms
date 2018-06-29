# BingMaps-Xamarin-Forms

# Bing Map v8
Para obtener la llave de desarrollador hay que dirigirse al siguiente link:
https://www.bingmapsportal.com

Para usar el plugin, solo importa cada contenido de la carpeta plugin al proyecto correspondiente.

# Android
La carpeta de BingMap debe ir dentro de Assets, y debe tener el archivo index.html la accion de compilacion como AndroidAsset 

# iOS
La carpeta BingMap se copia como esta, el archivo index.html debe tener la accion de compilacion como BundleResource en propiedades

# UWP
Solo copiar la carpeta BingMap en Assets

# BingMap/index.html
En este archivo hay que poner la api key que obtuvieron del portal de desarrolladores de Bing

# Funciones del plugin

```csharp

BingMapView view = new BingMapView();

// Set Center of Map
view.SetCenter(new Center(19.479778, 12.834520, 12));

// Add Pin
var pin = new Pin(19.479778, 12.834520) { Title = "Yo", Data = "Hola que hace!!" };
// Evento click en el pin
pin.Click += Pin_Click;
view.Pins.Add(pin);

// Hacer que el mapa muestre todos los pins agregados
view.ZoomForAllPins();

```

using BingMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(BingMapView), typeof(BingMap.UWP.BingMap))]
namespace BingMap.UWP
{
    public class BingMap : ViewRenderer<BingMapView, WebView>
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.external.notify(data);}";

        const string PinClick = "function invokePinClick(str){window.external.notify(str);}";

        protected override void OnElementChanged(ElementChangedEventArgs<BingMapView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new WebView());
            }
            if (e.OldElement != null)
            {
                Control.NavigationCompleted -= OnWebViewNavigationCompleted;
                Control.ScriptNotify -= OnWebViewScriptNotify;
            }
            if (e.NewElement != null)
            {
                Control.NavigationCompleted += OnWebViewNavigationCompleted;
                Control.ScriptNotify += OnWebViewScriptNotify;
                Control.Source = new Uri(string.Format("ms-appx-web:///Assets//BingMap//index.html"));
            }

            if (Element != null)
            {
                Element.ReceiveAction += Element_Send;
            }
        }
        private async void Element_Send(object sender, object e)
        {
            if (Control != null)
            {
                if (sender is BingMapView view)
                {
                    switch (view.Action)
                    {
                        case Action.SetCenter:
                            if (e is Center center)
                            {
                                var r = await Control.InvokeScriptAsync("eval", new string[]{ $"setCenter({center.Latitude},{center.Longitude},{center.Zoom})" });
                                System.Diagnostics.Debug.WriteLine(r);
                            }
                            break;

                        case Action.AddPin:
                            if (e is Pin pin)
                            {
                                if (pin.Image != null)
                                {
                                    var r = await Control.InvokeScriptAsync("eval", new string[] { $"addPinImage({pin.GetHashCode()}, {pin.Latitude}, {pin.Longitude}, '{pin.Title}', '{pin.Data}', '{pin.Image.Source}', {pin.Image.X}, {pin.Image.Y})" });
                                    System.Diagnostics.Debug.WriteLine(r);
                                }
                                else
                                {
                                    var r = await Control.InvokeScriptAsync("eval", new string[] { $"addPin({pin.GetHashCode()}, {pin.Latitude}, {pin.Longitude}, '{pin.Title}', '{pin.Data}')" });
                                    System.Diagnostics.Debug.WriteLine(r);
                                }
                            }
                            break;

                        case Action.RemoveAllPins:
                            if (e is null)
                            {
                                var r = await Control.InvokeScriptAsync("eval", new string[] { "removeAllPins()" });
                                System.Diagnostics.Debug.WriteLine(r);
                            }
                            break;

                        case Action.ZoomForAllPins:
                            if(e is null)
                            {
                                var r = Control.InvokeScriptAsync("eval", new string[] { "zoomForAllPins()" });
                                System.Diagnostics.Debug.WriteLine(r);
                            }
                            break;
                            
                        default:
                            break;
                    }
                }
            }
        }

        async void OnWebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                // Inject JS script
                await Control.InvokeScriptAsync("eval", new[] { PinClick });
            }
        }

        void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                var str = e.Value;
                if (str == "bingmapv8_loadcomplete")
                {
                    Element.OnLoadComplete(str);
                }
                else if (str.Contains("HashCode") && str.Contains("Data"))
                {
                    Pin pin = null;
                    // Tengo que lanzar lanzar el click del pin
                    try
                    {
                        pin = Element.DeserializeObject<Pin>(str);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace, "BingMap");
                    }

                    if (pin != null)
                    {
                        var pininlist = Element.Pins.FirstOrDefault(p => p.GetHashCode() == pin.HashCode);
                        if (pininlist != null)
                        {
                            System.Diagnostics.Debug.WriteLine("PinClick", "BingMap");
                            pininlist.OnClick();
                        }
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine(e.Value);
        }
    }
}
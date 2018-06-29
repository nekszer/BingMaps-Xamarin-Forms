using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BingMap;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BingMapView), typeof(BingMap.iOS.BingMap))]
namespace BingMap.iOS
{
    public class BingMap : ViewRenderer<BingMapView, WKWebView>, IWKScriptMessageHandler
    {
        const string PinClick = "function invokePinClick(data){window.webkit.messageHandlers.PinClick.postMessage(data);}";
        const string OnLoadComplete = "function invokeOnLoadComplete(data){window.webkit.messageHandlers.OnLoadComplete.postMessage(data);}";

        const string OnLoadCompleteMethod = "OnLoadComplete";
        const string PinClickMethod = "PinClick";

        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<BingMapView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                userController = new WKUserContentController();
                
                var script = new WKUserScript(new NSString(OnLoadComplete), WKUserScriptInjectionTime.AtDocumentEnd, false);
                userController.AddUserScript(script);

                userController.AddScriptMessageHandler(this, OnLoadCompleteMethod);

                script = new WKUserScript(new NSString(PinClick), WKUserScriptInjectionTime.AtDocumentEnd, false);
                userController.AddUserScript(script);

                userController.AddScriptMessageHandler(this, PinClickMethod);

                var config = new WKWebViewConfiguration { UserContentController = userController };
                var webView = new WKWebView(Frame, config);
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler(OnLoadCompleteMethod);
                userController.RemoveScriptMessageHandler(PinClickMethod);
                var hybridWebView = e.OldElement as BingMapView;
            }
            if (e.NewElement != null)
            {
                string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("BingMap/index.html"));
                Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
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
                                var r = await Control.EvaluateJavaScriptAsync($"setCenter({center.Latitude},{center.Longitude},{center.Zoom})");
                            }
                            break;

                        case Action.AddPin:
                            if (e is Pin pin)
                            {
                                if (pin.Image != null)
                                {
                                    var r = await Control.EvaluateJavaScriptAsync($"addPinImage({pin.GetHashCode()}, {pin.Latitude}, {pin.Longitude}, '{pin.Title}', '{pin.Data}', '{pin.Image.Source}', {pin.Image.X}, {pin.Image.Y})");
                                }
                                else
                                {
                                    var r = await Control.EvaluateJavaScriptAsync($"addPin({pin.GetHashCode()}, {pin.Latitude}, {pin.Longitude}, '{pin.Title}', '{pin.Data}')");
                                }
                            }
                            break;

                        case Action.RemoveAllPins:
                            if (e is null)
                            {
                                var r = await Control.EvaluateJavaScriptAsync("removeAllPins()");
                            }
                            break;

                        case Action.ZoomForAllPins:
                            if (e is null)
                            {
                                var r = await Control.EvaluateJavaScriptAsync("zoomForAllPins()");
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            var name = message.Name;
            var result = message.Body.ToString();

            switch (name)
            {
                case OnLoadCompleteMethod:
                    System.Diagnostics.Debug.WriteLine("OnLoadComplete", "BingMap");
                    Element.OnLoadComplete(result);
                    break;

                case PinClickMethod:

                    Pin pin = null;

                    try
                    {
                        pin = Element.DeserializeObject<Pin>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace, "BingMap");
                    }

                    if (pin != null)
                    {
                        var pinfound = Element.Pins.FirstOrDefault(p => p.GetHashCode() == pin.HashCode);
                        if(pinfound != null)
                        {
                            System.Diagnostics.Debug.WriteLine("PinClick", "BingMap");
                            pinfound.OnClick();
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
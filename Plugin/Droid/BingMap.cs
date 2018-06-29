using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BingMap;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(BingMapView), typeof(BingMap.Droid.BingMap))]
namespace BingMap.Droid
{
    public class BingMap : ViewRenderer<BingMapView, Android.Webkit.WebView>
    {
        const string PinClick = "function invokePinClick(str){jsBridge.pinClick(str);}";
        const string OnLoadComplete = "function invokeOnLoadComplete(str){jsBridge.onLoadComplete(str);}";

        Context _context;

        public BingMap(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<BingMapView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new Android.Webkit.WebView(_context)
                {
                    LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                };
                webView.Settings.SetGeolocationEnabled(true);
                webView.Settings.AllowContentAccess = true;
                webView.Settings.JavaScriptEnabled = true;
                SetNativeControl(webView);
            }

            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.LoadUrl("file:///android_asset/BingMap/index.html");
                InjectJS(OnLoadComplete);
                InjectJS(PinClick);
            }

            if (Element != null)
            {
                Element.ReceiveAction += Element_Send;
            }
        }

        private void Element_Send(object sender, object e)
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
                                Control.EvaluateJavascript($"setCenter({center.Latitude},{center.Longitude},{center.Zoom})", null);
                            }
                            break;

                        case Action.AddPin:
                            if(e is Pin pin)
                            {
                                if(pin.Image != null)
                                {
                                    Control.EvaluateJavascript($"addPinImage({pin.GetHashCode()}, {pin.Latitude}, {pin.Longitude}, '{pin.Title}', '{pin.Data}', '{pin.Image.Source}', {pin.Image.X}, {pin.Image.Y})", null);
                                }
                                else
                                {
                                    Control.EvaluateJavascript($"addPin({pin.GetHashCode()}, {pin.Latitude}, {pin.Longitude}, '{pin.Title}', '{pin.Data}')", null);
                                }
                            }
                            break;

                        case Action.RemoveAllPins:
                            if(e is null)
                            {
                                Control.EvaluateJavascript("removeAllPins()", null);
                            }
                            break;

                        case Action.ZoomForAllPins:
                            if(e is null)
                            {
                                Control.EvaluateJavascript("zoomForAllPins()", null);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        void InjectJS(string script)
        {
            if (Control != null)
            {
                Control.Post(() =>
                {
                    Control.LoadUrl(string.Format("javascript: {0}", script));
                });
            }
        }
    }
}
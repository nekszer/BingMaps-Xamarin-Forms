using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;

namespace BingMap.Droid
{
    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<BingMap> _bingmap;

        public JSBridge(BingMap hybridRenderer)
        {
            _bingmap = new WeakReference<BingMap>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("pinClick")]
        public void PinClick(string str)
        {
            if (_bingmap != null && _bingmap.TryGetTarget(out BingMap bingmap))
            {
                bingmap.Post(() =>
                {
                    Pin pin = null;
                    // Tengo que lanzar lanzar el click del pin
                    try
                    {
                        pin = bingmap.Element.DeserializeObject<Pin>(str);
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace, "BingMap");
                    }

                    if(pin != null)
                    {
                        var pininlist = bingmap.Element.Pins.FirstOrDefault(e => e.GetHashCode() == pin.HashCode);
                        if(pininlist != null)
                        {
                            System.Diagnostics.Debug.WriteLine("PinClick", "BingMap");
                            pininlist.OnClick();
                        }
                    }
                });
            }
        }

        [JavascriptInterface]
        [Export("onLoadComplete")]
        public void OnLoadComplete(string str)
        {
            System.Diagnostics.Debug.WriteLine("OnLoadComplete", "BingMap");
            if (_bingmap != null && _bingmap.TryGetTarget(out BingMap bingmap))
            {
                bingmap.Post(() =>
                {
                    bingmap.Element.OnLoadComplete(str);
                });
            }
        }


    }
}
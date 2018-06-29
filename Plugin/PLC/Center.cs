using System;
using System.Collections.Generic;
using System.Text;

namespace BingMap
{
    public class Center
    {
        /// <summary>
        /// Asigna el centro del mapa
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="zoom"></param>
        public Center(double latitude, double longitude, int zoom = 10)
        {
            Latitude = latitude;
            Longitude = longitude;
            Zoom = zoom;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public int Zoom { get; private set; }
    }
}

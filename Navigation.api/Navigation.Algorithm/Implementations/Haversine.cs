/**************************************************************************
 *                                                                        *
 *  File:        Haversine.cs                                             *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using System;

namespace Navigation.Algorithm.Implementations
{

    /// <summary>
    /// Haversine class will contain the methods used to calculate the estimated cost between nodes.
    /// </summary>
    public class Haversine
    {
        /// <summary>
        /// Returns the distance in kilometers of any two latitude / longitude points.
        /// </summary>
        /// <param name=”node1″><b>node1</b> will represent the start city.</param>
        /// <param name=”node2″><b>node2</b> will represent the destination city.</param>
        /// <returns></returns>
        public static double Distance(Node node1, Node node2)
        {
            double R = 6371;

            double dLat = ToRadian(Math.Abs(node2.Latitude - node1.Latitude));

            double dLong = ToRadian(Math.Abs(node2.Longitude - node1.Longitude));

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(node1.Latitude)) * Math.Cos(ToRadian(node2.Latitude)) *
                Math.Sin(dLong / 2) * Math.Sin(dLong / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c;

            return d;
        }

        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”>Specific value to convert.</param>
        /// <returns></returns>
        private static double ToRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}

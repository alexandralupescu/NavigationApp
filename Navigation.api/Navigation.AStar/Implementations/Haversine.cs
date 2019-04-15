﻿/**************************************************************************
 *                                                                        *
 *  File:        Haversine.cs                                             *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 *                                                                        *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.AStar.Implementations
{

    /// <summary>
    /// The distance type to return the results in.
    /// </summary>
    public enum DistanceType { ml, km };

    /// <summary>
    /// Haversine class will contain the methods used to calculate the estimated cost between nodes.
    /// </summary>
    public class Haversine
    {
        /// <summary>
        /// Returns the distance in miles or kilometers of any two latitude / longitude points.
        /// </summary>
        /// <param name=”node1″></param>
        /// <param name=”node2″></param>
        /// <param name=”type”></param>
        /// <returns>Returns the estimated distance calculated based on geographic coordinates values.</returns>
        public static double Distance(Node node1, Node node2, DistanceType type)
        {
            /* R = Earth's radius in miles or km */
            double R = (type == DistanceType.ml) ? 3960 : 6371;

            double dLat = ToRadian(node2.Latitude - node1.Latitude);

            double dLon = ToRadian(node2.Longitude - node1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(node1.Latitude)) * Math.Cos(ToRadian(node2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

            double d = R * c;

            return d;
        }

        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        private static double ToRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}

/**************************************************************************
 *                                                                        *
 *  File:        CitiesModel.cs                                           *
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

namespace Navigation.Business.Models
{

    /// <summary>
    /// CitiesModel class accepts commands from the CitiesController and processes them.
    /// </summary>
    /// <remarks>
    /// Model class is a bridge between control and the view. The user will activate a control, which calls a method in the model.
    /// </remarks>
    public class CitiesModel
    {

        #region Public Properties
        public string CityName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsResidence { get; set; }

        public string County { get; set; }
        #endregion

    }
}

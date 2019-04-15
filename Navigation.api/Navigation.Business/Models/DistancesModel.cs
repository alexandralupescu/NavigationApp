/**************************************************************************
 *                                                                        *
 *  File:        DistancesModel.cs                                        *
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
    /// DistancesModel class accepts commands from the DistancesController and processes them.
    /// </summary>
    /// <remarks>
    /// Model class is a bridge between control and the view. The user will activate a control, which calls a method in the model.
    /// </remarks>
    public class DistancesModel
    {

        #region Public Properties
        public string StartCity { get; set; }

        public string StopName { get; set; }

        public double Distance { get; set; }

        public bool IsRailway { get; set; }
        #endregion

    }
}

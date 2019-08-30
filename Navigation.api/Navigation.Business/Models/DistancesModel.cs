/**************************************************************************
 *                                                                        *
 *  File:        DistancesModel.cs                                        *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/

namespace Navigation.Business.Models
{

    /// <summary>
    /// DistancesModel class accepts commands from the DistancesController and processes them.
    /// </summary>
    /// <remarks>
    /// Business tier provides services managing business rules and processing data that is shared
    /// by many applications. This middle tier ensures increased performance, flexibility and
    /// scalability through logical centralization of processes.
    /// </remarks>
    public class DistancesModel
    {

        #region Public Properties
        public string StartCity { get; set; }

        public string DestinationCity { get; set; }

        public double Distance { get; set; }

        public bool IsRailway { get; set; }
        #endregion

    }
}

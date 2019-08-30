/**************************************************************************
 *                                                                        *
 *  File:        CitiesModel.cs                                           *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/

namespace Navigation.Business.Models
{

    /// <summary>
    /// CitiesModel class accepts commands from the CitiesController and processes them.
    /// </summary>
    /// <remarks>
    /// Business tier provides services managing business rules and processing data that is shared
    /// by many applications. This middle tier ensures increased performance, flexibility and
    /// scalability through logical centralization of processes.
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

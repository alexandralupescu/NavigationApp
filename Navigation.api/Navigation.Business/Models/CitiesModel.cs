using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.Business.Models
{
    public class CitiesModel
    {

        #region Members
        public string CityName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsResidence { get; set; }
        #endregion

    }
}

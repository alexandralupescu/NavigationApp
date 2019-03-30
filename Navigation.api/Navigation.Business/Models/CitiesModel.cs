using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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

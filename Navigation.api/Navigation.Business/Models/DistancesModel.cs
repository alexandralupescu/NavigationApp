using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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

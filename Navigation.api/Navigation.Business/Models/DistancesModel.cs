using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.Business.Models
{
    public class DistancesModel
    {

        #region Members
        public string StartName { get; set; }

        public string StopName { get; set; }

        public double Distance { get; set; }

        public bool IsRailway { get; set; }
        #endregion

    }
}

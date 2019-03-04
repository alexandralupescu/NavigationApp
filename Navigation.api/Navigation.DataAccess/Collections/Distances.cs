using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.DataAccess.Collections
{
    public class Distances : BaseCollection
    {

        #region Members
        [BsonElement("name_start")]
        public string StartName { get; set; }

        [BsonElement("name_stop")]
        public string StopName { get; set; }

        [BsonElement("distance")]
        public double Distance { get; set; }

        [BsonElement("railway")]
        public bool IsRailway { get; set; }
        #endregion

    }
}

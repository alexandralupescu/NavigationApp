using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.DataAccess.Collections
{
    public class Cities : BaseCollection
    {
        #region Members
        [BsonElement("name")]
        public string CityName { get; set; }

        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }

        [BsonElement("residence")]
        public bool IsResidence { get; set; }

        #endregion





    }
}

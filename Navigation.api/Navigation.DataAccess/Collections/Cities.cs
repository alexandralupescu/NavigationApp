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
        public string cityName { get; set; }

        [BsonElement("latitude")]
        public double latitude { get; set; }

        [BsonElement("longitude")]
        public double longitude { get; set; }

        [BsonElement("residence")]
        public bool isResidence { get; set; }

        #endregion





    }
}

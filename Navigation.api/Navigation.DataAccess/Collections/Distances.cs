using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.DataAccess.Collections
{
    /// <summary>
    /// Representation of Distances collection from the database that inherited BaseCollection class.
    /// </summary>
    /// <remarks>
    /// Distances collection keeps following information:
    ///     - <b>StartName</b> represents the city from which the user will leave;
    ///     - <b>StopName</b> is the destination city; 
    ///     - <b>Distance</b> represents the road distance between those two cities. 
    /// </remarks>
    public class Distances : BaseCollection
    {

        #region Public Properties
        [BsonElement("name_start")]
        public string StartCity { get; set; }

        [BsonElement("name_stop")]
        public string DestinationCity { get; set; }

        [BsonElement("distance")]
        public double Distance { get; set; }

        [BsonElement("railway")]
        public bool IsRailway { get; set; }
        #endregion

    }
}

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Navigation.DataAccess.Collections
{
    /// <summary>
    /// Representation of Cities collection from the database that inherited BaseCollection class.
    /// </summary>
    /// <remarks>
    /// Cities collection keeps following information: 
    ///     - <b>CityName</b> represents the name of the city;
    ///     - geographic coordinates: <b>Latitude</b> and <b>Longitude/b>; 
    ///     - <b>IsResidence</b> tests if a city is a residence of a county.
    ///     - <b>County</b> represents forming the city chief unit of local administration.
    /// </remarks>
    public class Cities : BaseCollection
    {

        #region Public Properties
        [BsonElement("name")]
        public string CityName { get; set; }

        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }

        [BsonElement("residence")]
        public bool IsResidence { get; set; }

        [BsonElement("county")]
        public string County { get; set; }
        #endregion

    }
}

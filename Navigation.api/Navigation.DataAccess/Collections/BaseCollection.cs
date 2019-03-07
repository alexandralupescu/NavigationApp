using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.DataAccess.Collections
{
    public class BaseCollection
    {

        #region Members
        /* designate this property as the document's primary key */
        [BsonId]
        /* allow passing the parameter as type string instead of ObjectId */
        [BsonRepresentation(BsonType.ObjectId)]
        /* the attribute's value represents the property name in the MongoDB collection */
        /* using automatic properties */
        [BsonElement("_id")]
        public string Id { get; set; }
        #endregion

      



    }
}

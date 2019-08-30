/**************************************************************************
 *                                                                        *
 *  File:        BaseCollection.cs                                        *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navigation.DataAccess.Collections
{

    /// <summary>
    /// BaseCollection represents a base class with an <b>Id</b> field.
    /// </summary>
    /// <remarks>
    /// BaseCollection contains an <b>Id</b> field, which is mapped to a collection that can be reused across multiple classes.
    /// </remarks>
    public class BaseCollection
    {

        #region Public Properties
        /// <summary>
        /// Designate this property as the document's primary key.
        /// </summary>
        [BsonId]
        ///<summary>
        /// Allow passing the parameter as type string instead of ObjectId type.
        ///</summary>
        [BsonRepresentation(BsonType.ObjectId)]
        /// <summary>
        /// The attribute's value represents the property name in the database collection.
        /// </summary>
        [BsonElement("_id")]
        public string Id { get; set; }
        #endregion    

    }
}

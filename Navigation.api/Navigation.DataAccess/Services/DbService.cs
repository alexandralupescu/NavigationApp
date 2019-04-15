/**************************************************************************
 *                                                                        *
 *  File:        DbService.cs                                             *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 *                                                                        *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.DataAccess.Services
{

    /// <summary>
    /// Generic DbService class implements the IDbService interface.
    /// </summary>
    /// <typeparam name="T">T type will be the specific collection from the database.</typeparam>
    public class DbService<T> : IDbService<T> where T : BaseCollection
    {

        #region Private Members
        /// <summary>
        /// MongoCollection object representing the collection used in methods.
        /// </summary>
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// Represents the MongoDB parameters used for connection.
        /// </summary>
        private readonly string _dbHost;
        private readonly string _dbName;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that contains the instance of an IConfiguration object.
        /// </summary>
        /// <param name="configuration">Set of value application configuration properties.</param>
        public DbService(IConfiguration configuration)
        {
            _dbHost = configuration.GetConnectionString("DatabaseHost");
            _dbName = configuration.GetConnectionString("DatabaseName");

            /* Reads the server instance for performing database operations. */
            var client = new MongoClient(this._dbHost);

            /* Reads the name of the database. */
            var database = client.GetDatabase(this._dbName);

            /* Gain access to data in a specific collection. */
            _collection = database.GetCollection<T>(typeof(T).Name);
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///  Inserts the provided object as a new document in the collection.
        /// </summary>
        /// <param name="document">Document that will be submited in a specific collection.</param>
        /// <returns>Returns no value, except the awaitable Task. If a record will not be added, it will generate an Exception.</returns>
        public async Task CreateAsync(T document)
        {
            try
            {
                await _collection.InsertOneAsync(document);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DbService - CreateAsync(document) method!"), exception);
            }
        }

        /// <summary>
        /// Deletes a single document matching the provided search crieteria.
        /// </summary>
        /// <param name="id"><b>id</b> represents the matching criteria for delete operation.</param>
        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", id);
                DeleteResult deleteResult = await _collection
                                                    .DeleteOneAsync(filter);

                return deleteResult.IsAcknowledged
                        && deleteResult.DeletedCount > 0;
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DbService - DeleteAsync(id) method!"), exception);
            }
        }

        /// <summary>
        /// Returns all documents in the collection.
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DbService - GetAllAsync() method!"), exception);
            }
        }

        /// <summary>
        /// Returns the document matching the provided search criteria, in this case is by <b>id</b>.
        /// </summary>
        /// <param name="id"><b>id</b> represents the provided search criteria.</param>
        public Task<T> GetByIdAsync(string id)
        {
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq(c => c.Id, id);
                return _collection
                        .Find(filter)
                        .FirstOrDefaultAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DbService - GetByIdAsync(id) method!"), exception);

            }
        }


        /// <summary>
        /// Replaces the single document mathcing the provided search criteria with provided object.
        /// </summary>
        /// <param name="document">Document that will be updated in a specific collection.</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T document)
        {
            try
            {
                ReplaceOneResult actionResult = await _collection
                                .ReplaceOneAsync(d => d.Id.Equals(document.Id)
                                        , document
                                        , new UpdateOptions { IsUpsert = true });

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;

            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DbService - UpdateAsync(document) method!"), exception);
            }
        }
        #endregion

    }
}

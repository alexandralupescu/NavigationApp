using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.DataAccess.Services
{
    public class DbService<T> : IDbService<T> where T : BaseCollection
    {
        #region Members

        /* MongoCollection object representing the collection */
        private readonly IMongoCollection<T> _collection;
        
        /* represents the MongoDB connection string parameters */
        private readonly string _dbHost;
        private readonly string _dbName;

        #endregion

        #region Public Methods
        public DbService(IConfiguration configuration)
        {
            /* get database hostname to connect to database */
            this._dbHost = configuration.GetConnectionString("DatabaseHost");
            /* get database name to access the collections */
            this._dbName = configuration.GetConnectionString("DatabaseName");
            
            /* reads the server instance for performing database operations */
            var client = new MongoClient(this._dbHost);
            /* reads the name of the database */
            var database = client.GetDatabase(this._dbName);
            /* gain access to data in a specific collection */
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        /* inserts the provided object as a new document in the collection */
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

        /* deletes a single document matching the provided search crieteria */
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

        /* returns all documents in the collection */
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

        /* returns the document matching the provided search criteria, in this case is by id */
        public Task<T> GetByIdAsync(string id)
        {
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", id);
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

        /* replaces the single document mathcing the provided search  criteria with provided object */
        public async Task<bool> UpdateAsync(T document)
        {
            try
            {
                ReplaceOneResult actionResult
                = await _collection
                                .ReplaceOneAsync(d => d.Id.Equals(document.Id)
                                        , document
                                        , new UpdateOptions { IsUpsert = true });

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;


            }

            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DbService - UpdateAsync(document,id) method!"), exception);
            }
        }


        #endregion
    }
}

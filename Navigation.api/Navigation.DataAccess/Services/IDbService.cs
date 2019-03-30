using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.DataAccess.Services
{

    /// <summary>
    /// IDBService it is a generic interface.
    /// </summary>
    /// <remarks>
    /// The advantage of this approach is that it ensures we have a common interface for working with any of the collections.
    /// </remarks>
    /// <typeparam name="T">T type will be the specific collection from the database.</typeparam>
    public interface IDbService<T>
    { 

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task CreateAsync(T document);

        Task<bool> DeleteAsync(string id);

        Task<bool> UpdateAsync(T document);

    }
}

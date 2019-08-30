/**************************************************************************
 *                                                                        *
 *  File:        IDbService.cs                                            *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.DataAccess.Services
{

    /// <summary>
    /// IDBService it is a generic interface.
    /// Provides management functionality and is dedicated to data services, respectively logic accesses the
    /// database and validates the service.
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

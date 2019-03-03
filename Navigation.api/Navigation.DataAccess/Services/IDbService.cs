using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.DataAccess.Services
{
    public interface IDbService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task CreateAsync(T document);

        Task<bool> DeleteAsync(string id);

        Task<bool> UpdateAsync(T document);

    }
}

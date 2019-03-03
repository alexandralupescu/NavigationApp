using MongoDB.Bson;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Interfaces
{
    public interface ICitiesLogic
    {
        Task<IEnumerable<Cities>> GetAllCitiesAsync();

        Task<Cities> GetByCityIdAsync(string id);

        Task CreateCityAsync(CitiesModel cities);

        Task<bool> DeleteCityAsync(string id);

        Task<bool> UpdateCityAsync(CitiesModel cities,string id);
    }
}

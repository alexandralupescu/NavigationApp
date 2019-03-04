using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Implementations
{
    public class DistancesLogic : IDistancesLogic
    {

        #region Members
        /* used to access methods from DbService class */
        private readonly IDbService<Distances> _distanceService;
        #endregion

        #region Public Methods
        public DistancesLogic(IDbService<Distances> distanceService)
        {
            _distanceService = distanceService;
        }

        /* inserts a new distance in Distances collection */
        public async Task CreateDistanceAsync(DistancesModel distances)
        {
            try
            {
                var @distance = new Distances
                {
                    StartName = distances.StartName,
                    StopName = distances.StopName,
                    Distance = distances.Distance,
                    IsRailway = distances.IsRailway
                };


                await _distanceService.CreateAsync(distance);
            }
            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in DistancesLogic - CreateDistanceAsync(distances) method!"), exception);
            }
        }

        /* deletes a single distance document mathcing the provided search criteria */
        public async Task<bool> DeleteDistanceAsync(string id)
        {
            try
            {
                return await _distanceService.DeleteAsync(id);
            }

            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in DistancesLogic - DeleteDistanceAsync(id) method!"), exception);
            }
        }

        /* returns all distances from Distances collection */
        public async Task<IEnumerable<Distances>> GetAllDistancesAsync()
        {
            try
            {
                return await _distanceService.GetAllAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DistancesLogic - GetAllDistancesAsync() method!"), exception);
            }
        }

        /* returns the required distance matching the provided search criteria, in this case distance id */
        public Task<Distances> GetByDistanceIdAsync(string id)
        {
            try
            {
                return _distanceService.GetByIdAsync(id);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesLogic - GetByDistanceIdAsync(id) method!"), exception);
            }
        }

        /* replaces the distance document matching the provided search criteria with the provided object */
        public async Task<bool> UpdateDistanceAsync(DistancesModel distances, string id)
        {
            try
            {
                var distanceFromDb = await _distanceService.GetByIdAsync(id);

                var @distance = new Distances
                {

                    StartName = distances.StartName,
                    StopName = distances.StopName,
                    Distance = distances.Distance,
                    IsRailway = distances.IsRailway,
                    Id = distanceFromDb.Id
                };


                return await _distanceService.UpdateAsync(distance);
            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesLogic - UpdateDistanceAsync(distances,id) method!"), exception);
            }
        }
        #endregion

    }
}

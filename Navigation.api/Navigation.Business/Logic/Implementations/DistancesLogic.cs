/**************************************************************************
 *                                                                        *
 *  File:        DistancesLogic.cs                                        *
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
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Implementations
{
    /// <summary>
    /// DistancesLogic implements the IDistancesLogic interface and calls the methods from DBService class.
    /// </summary>
    /// <remarks>
    /// BLL (Business Logic Layer) serves as an intermediary layer for data exchange between the presentation layer and DAL (Data Access Layer).
    /// </remarks>
    public class DistancesLogic : IDistancesLogic
    {

        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly IDbService<Distances> _distanceService;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that contains the instance of an IDBService object.
        /// </summary>
        /// <param name="distanceService">Instance of Distances object.</param>
        public DistancesLogic(IDbService<Distances> distanceService)
        {
            _distanceService = distanceService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inserts a new distance in Distances collection.
        /// </summary>
        /// <param name="distances">Document that will be submited in Distances collection.</param>
        public async Task CreateDistanceAsync(DistancesModel distances)
        {
            try
            {
                var @distance = new Distances
                {
                    StartCity = distances.StartCity,
                    DestinationCity = distances.StopName,
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

        /// <summary>
        /// Deletes a single distance document mathcing the provided search criteria.
        /// </summary>
        /// <param name="id">The matching criteria for delete operation.</param>
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

        /// <summary>
        /// Returns all distances from Distances collection.
        /// </summary>
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

        /// <summary>
        /// Returns the distance information based on the start and destination city. 
        /// </summary>
        /// <param name="startCity">The city from which to start.</param>
        /// <param name="destinationCity">The city we have to reach.</param>
        public async Task<DistancesModel> GetRoadDistanceAsync(string startCity, string destinationCity)
        {
            try
            {
                var saveDistance = await _distanceService.GetAllAsync();
                foreach (Distances distance in saveDistance)
                {
                    if ((distance.StartCity.Contains(startCity)) && (distance.DestinationCity.Contains(destinationCity)))
                    {
                        var distanceSave = new DistancesModel
                        {
                            Distance = distance.Distance
                        };


                        return distanceSave;

                    }
                    
                }

                return null;
                
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesLogic - GetRoadDistanceAsync(startCity,destinationCity) method!"), exception);
            }
        }

        /// <summary>
        /// Returns the required distance matching the provided search criteria, in this case distance <b>id</b>. 
        /// </summary>
        /// <param name="id">The provided search criteria.</param>
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

        /// <summary>
        /// Replaces the distance document matching the provided search criteria with the provided object.
        /// </summary>
        /// <param name="distances">Document that will be updated in Distances collection.</param>
        /// <param name="id">The provided search criteria.</param>
        public async Task<bool> UpdateDistanceAsync(DistancesModel distances, string id)
        {
            try
            {
                var distanceFromDb = await _distanceService.GetByIdAsync(id);

                var @distance = new Distances
                {

                    StartCity = distances.StartCity,
                    DestinationCity = distances.StopName,
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

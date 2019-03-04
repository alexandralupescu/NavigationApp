using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using System;
using System.Threading.Tasks;

namespace Navigation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {

        #region Members
        /* used to access the data from the Business layer */
        private readonly ICitiesLogic _citiesLogic;
        #endregion

        #region Constructors
        public CitiesController(ICitiesLogic citiesLogic)
        {
            _citiesLogic = citiesLogic;
        }
        #endregion

        #region CRUD Methods
        /* GET method */
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return new ObjectResult(await _citiesLogic.GetAllCitiesAsync());
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in CitiesController - Get() method!"), exception);
            }
        }

        /* GET (by id) method */
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var city = await _citiesLogic.GetByCityIdAsync(id);
                return new ObjectResult(city);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Get(id) method!"), exception);
            }
        }

        /* POST method */
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CitiesModel city)
        {
            try
            {
                await _citiesLogic.CreateCityAsync(city);
                return new ObjectResult(city);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Post(city) method!"), exception);
            }
        }

        /* DELETE method */
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var cityFromDb = await _citiesLogic.GetByCityIdAsync(id);
                await _citiesLogic.DeleteCityAsync(id);
                return new OkResult();
            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Delete(id) method!"), exception);
            }
        }

        /* PUT method */
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put([FromBody]CitiesModel city,string id)
        {
            try
            {          
                await _citiesLogic.UpdateCityAsync(city,id);
                return new OkObjectResult(city);

            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Put(city,id) method!"), exception);
            }
        }
        #endregion

    }
}
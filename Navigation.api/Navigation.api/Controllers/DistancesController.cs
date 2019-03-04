using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;

namespace Navigation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistancesController : ControllerBase
    {

        #region Members
        private readonly IDistancesLogic _distancesLogic;
        #endregion

        #region Constructors
        public DistancesController(IDistancesLogic distancesLogic)
        {
            _distancesLogic = distancesLogic;
        }
        #endregion

        #region CRUD Methods
        /* GET method */
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return new ObjectResult(await _distancesLogic.GetAllDistancesAsync());
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DistancesController - Get() method!"), exception);
            }
        }

        /* GET (by id) method */
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var distance = await _distancesLogic.GetByDistanceIdAsync(id);
                return new ObjectResult(distance);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - Get(id) method!"), exception);
            }
        }

        /* POST method */
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DistancesModel distance)
        {
            try
            {
                await _distancesLogic.CreateDistanceAsync(distance);
                return new ObjectResult(distance);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - Post(distance) method!"), exception);
            }
        }

        /* DELETE method */
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var distanceFromDb = await _distancesLogic.GetByDistanceIdAsync(id);
                await _distancesLogic.DeleteDistanceAsync(id);
                return new OkResult();
            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - Delete(id) method!"), exception);
            }
        }
        #endregion

    }
}
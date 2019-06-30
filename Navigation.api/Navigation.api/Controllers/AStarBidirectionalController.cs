using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navigation.Algorithm.Algorithms;
using Navigation.Business.Logic.Interfaces;

namespace Navigation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AStarBidirectionalController : ControllerBase
    {
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;
        private readonly NBAStar _aStarBi;

        public AStarBidirectionalController(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
            _aStarBi = new NBAStar(_citiesLogic, _distancesLogic);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAStarBiResult(string startCity, [FromQuery]List<string> destinationCity)
        {
            try
            {
                Stopwatch myTimer = new Stopwatch();
                myTimer.Start();
                Dictionary<string, List<string>> dict = await _aStarBi.GetNBAStarAsync(startCity, destinationCity);
                myTimer.Stop();
             
                return new OkObjectResult(dict);
            }
            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in AStarController - GetAStarBiResult(startCity,destinationCity) method!"), exception);
            }

        }
    }

   
}
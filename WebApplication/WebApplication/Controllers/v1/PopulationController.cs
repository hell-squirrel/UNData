using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Models;
using AppService.Models.ViewModel;
using AppService.Providers.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]/[action]")]
    public class PopulationController : ControllerBase
    {
        private readonly ILogger<PopulationController> _logger;
        private readonly IPopulationProvider _populationProvider;
        private readonly IMapper _mapper;

        public PopulationController(ILogger<PopulationController> logger, IPopulationProvider provider, IMapper mapper)
        {
            _logger = logger;
            _populationProvider = provider;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Result<IList<PopulationView>>> Get([FromQuery] int location)
        {
            var result = new Result<IList<PopulationView>>();
            try
            {
                var population = this._populationProvider.GetData(location);
                result.Data = population.Select(_mapper.Map<PopulationView>).ToList();
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                result.IsSuccess = false;
            }
            finally
            {
                _logger.LogInformation($"Location for {location} retrieved.");
            }

            return result;
        }

        [HttpGet]
        public async Task<Result<string>> LoadData(DateTime startData, DateTime endDate)
        {
            var result = new Result<string>();
            try
            {
                this._populationProvider.LoadPopulation(startData, endDate);
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                result.IsSuccess = false;
            }
            finally
            {
                _logger.LogInformation($"Location retrieved from UN server.");
            }

            return result;
        }
    }
}
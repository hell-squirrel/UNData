using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Commands;
using AppService.Interfaces;
using AppService.Models;
using AppService.Models.RequestModel;
using AppService.Models.ViewModel;
using AppService.Providers.Interfaces;
using AppService.Queries;
using AutoMapper;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]/[action]")]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        

        public LocationController(ILogger<LocationController> logger, IMapper mapper,IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Result<LocationView>> Get([FromQuery] int locationId)
        {
            var result = new Result<LocationView>();
            try
            {
                var query = new GetLocationQuery(locationId);
                var location = _mediator.Execute(query);
                result.Data = _mapper.Map<LocationView>(location);
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                result.IsSuccess = false;
            }
            finally
            {
                _logger.LogInformation($"Location for {locationId} retrieved.");
            }

            return result;
        }

        [HttpGet]
        public async Task<Result<IEnumerable<LocationView>>> Search(string query, int page = 1, int pageSize = 5)
        {
            var result = new Result<IEnumerable<LocationView>>();
            try
            {  
                var searchQuery = new SearchLocationDescriptionQuery(query,page,pageSize);
                var locations = _mediator.Execute(searchQuery);
                result.Data = locations.Select(_mapper.Map<LocationView>);
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                result.IsSuccess = false;
            }
            finally
            {
                _logger.LogInformation($"Location retrieved.");
            }

            return result;
        }
        
        [HttpPost]
        public async Task<Result<LocationView>> AddDescription([FromForm]DescriptionRequestModel model)
        {
            var result = new Result<LocationView>();
            try
            {
                var addDescriptionqQery = new AddLocationDescriptionCommand(model.Location,model.Description);
                _mediator.Execute(addDescriptionqQery);
                var getLocationQuery = new GetLocationQuery(model.Location);
                var location = _mediator.Execute(getLocationQuery);
                result.Data = _mapper.Map<LocationView>(location);
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                result.IsSuccess = false;
            }
            finally
            {
                _logger.LogInformation($"Location retrieved.");
            }

            return result;
        }
    }
}
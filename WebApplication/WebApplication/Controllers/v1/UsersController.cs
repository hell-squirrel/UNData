using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Models;
using AppService.Models.ViewModel;
using AppService.Providers.Interfaces;
using AutoMapper;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("v1/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserProvider userProvider, IMapper mapper, ILogger<UsersController> logger)
        {
            _userProvider = userProvider;
            _mapper = mapper;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<Result<UserView>> Authenticate([FromBody] AuthenticateModel model)
        {
            var result = new Result<UserView>();
            try
            {
                var user = _userProvider.Authenticate(model.Username, model.Password);
                if (user != null)
                {
                    result.Data = _mapper.Map<UserView>(user);
                }
                else
                {
                    result.Error = "Username or password is incorrect";
                    result.IsSuccess = false;
                }

                return result;
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                result.IsSuccess = false;
            }
            finally
            {
                _logger.Log(LogLevel.Information, "User's token generated.");
            }

            return result;
        }

        [HttpGet]
        public async Task<Result<IList<UserView>>> GetAll()
        {
            var result = new Result<IList<UserView>>();
            try
            {
                var users = _userProvider.GetAll();
                result.Data = users.Select(_mapper.Map<UserView>).ToList();
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                result.IsSuccess = false;
            }
            finally
            {
                _logger.Log(LogLevel.Information, "Users retrieved.");
            }

            return result;
        }

        [HttpPost]
        public async Task<Result<User>> Create(CreateUserModel userModel)

        {
            var result = new Result<User>();
            try
            {
                var user = _mapper.Map<User>(userModel);
                result.Data = _userProvider.Create(user);
                return result;
            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }
            finally
            {
                _logger.Log(LogLevel.Information, $"User created {result.Data.Username}");
            }

            return result;
        }
    }
}
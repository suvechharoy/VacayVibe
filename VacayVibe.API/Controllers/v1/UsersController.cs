using System.Net;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using VacayVibe.API.Models;
using VacayVibe.API.Models.DTO;
using VacayVibe.API.Repository.IRepository;

namespace VacayVibe.API.Controllers.v1;

[Route("api/v{version:apiVersion}/UsersAuth")]
[ApiController]
[ApiVersionNeutral]//this states that this particular controller will be there no matter what the version is
public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;
    protected APIResponse _apiResponse;
    
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        this._apiResponse = new APIResponse();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var loginResponse = await _userRepository.Login(model);
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Invalid username or password");
            return BadRequest(_apiResponse);
        }
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.IsSuccess = true;
        _apiResponse.Result = loginResponse;
        return Ok(_apiResponse);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
    {
        bool isUserUnique = _userRepository.IsUniqueUser(model.UserName);
        if (!isUserUnique)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("User already exists");
            return BadRequest(_apiResponse);
        }
        
        var user = _userRepository.Register(model);
        if (user == null)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Error while registering");
            return BadRequest(_apiResponse);
        }
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.IsSuccess = true;
        return Ok(_apiResponse);
    }
}
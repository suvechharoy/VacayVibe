using VacayVibe.Utility;
using VacayVibe.Web.Models;
using VacayVibe.Web.Models.DTO;
using VacayVibe.Web.Services.IServices;

namespace VacayVibe.Web.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IHttpClientFactory _httpClient;
    private string villaUrl;
    
    public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
    {
        _httpClient = httpClient;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VacayVibeAPI"); //from appsettings.json
    }

    public Task<T> LoginAsync<T>(LoginRequestDTO obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = obj,
            Url = villaUrl + "/api/UsersAuth/login"
        });
    }

    public Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = obj,
            Url = villaUrl + "/api/UsersAuth/register"
        });
    }
}
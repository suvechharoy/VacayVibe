using VacayVibe.Utility;
using VacayVibe.Web.Models;
using VacayVibe.Web.Models.DTO;
using VacayVibe.Web.Services.IServices;

namespace VacayVibe.Web.Services;

public class VillaNumberService : BaseService, IVillaNumberService
{
    private readonly IHttpClientFactory _httpClient;
    private string villaUrl;
    
    public VillaNumberService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
    {
        _httpClient = httpClient;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VacayVibeAPI"); //from appsettings.json
    }

    public Task<T> GetAllAsync<T>(string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/api/VillaNumber",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/api/VillaNumber/"+id,
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/api/VillaNumber",
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/api/VillaNumber/"+dto.VillaNo,
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = villaUrl + "/api/VillaNumber/"+id,
            Token = token
        });
    }
}
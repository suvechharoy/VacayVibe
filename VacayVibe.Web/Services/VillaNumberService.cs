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

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/api/VillaNumber"
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/api/VillaNumber/"+id
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/api/VillaNumber"
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/api/VillaNumber/"+dto.VillaNo
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = villaUrl + "/api/VillaNumber/"+id
        });
    }
}
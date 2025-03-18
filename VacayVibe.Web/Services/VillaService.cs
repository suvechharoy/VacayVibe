using VacayVibe.Utility;
using VacayVibe.Web.Models;
using VacayVibe.Web.Models.DTO;
using VacayVibe.Web.Services.IServices;

namespace VacayVibe.Web.Services;

public class VillaService : BaseService, IVillaService
{
    private readonly IHttpClientFactory _httpClient;
    private string villaUrl;
    
    public VillaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
    {
        _httpClient = httpClient;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VacayVibeAPI"); //from appsettings.json
    }

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/api/Villa"
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/api/Villa/"+id
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/api/Villa"
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/api/Villa/"+dto.Id
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = villaUrl + "/api/Villa/"+id
        });
    }
}
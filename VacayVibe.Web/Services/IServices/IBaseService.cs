using VacayVibe.Web.Models;

namespace VacayVibe.Web.Services.IServices;

public interface IBaseService
{
    APIResponse responseModel { get; set; }
    Task<T> SendAsync<T>(APIRequest request);
}
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using VacayVibe.Utility;
using VacayVibe.Web.Models;
using VacayVibe.Web.Services.IServices;

namespace VacayVibe.Web.Services;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    public IHttpClientFactory httpClient { get; set; }

    public BaseService(IHttpClientFactory httpClient)
    {
        this.responseModel = new();
        this.httpClient = httpClient;
    }
    public async Task<T> SendAsync<T>(APIRequest request)
    {
        try
        {
            var client = httpClient.CreateClient("VacayVibeAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(request.Url);
            if (request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data),
                    Encoding.UTF8, "application/json");
            }

            switch (request.ApiType)
            {
                case StaticDetails.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case StaticDetails.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case StaticDetails.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage apiResponse = null;
            if (!string.IsNullOrEmpty(request.Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);
            }
            apiResponse = await client.SendAsync(message);
            
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            try
            {
                APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                    apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ApiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    ApiResponse.IsSuccess = false;
                    var res = JsonConvert.SerializeObject(ApiResponse);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;
                }
            }
            catch (Exception e)
            {
                var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return exceptionResponse;
            }

            var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return APIResponse;
        }
        catch (Exception e)
        {
            var dto = new APIResponse
            {
                ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                IsSuccess = false,
            };
            var res = JsonConvert.SerializeObject(dto);
            var APIResponse = JsonConvert.DeserializeObject<T>(res);
            return APIResponse;
        }
    }
}
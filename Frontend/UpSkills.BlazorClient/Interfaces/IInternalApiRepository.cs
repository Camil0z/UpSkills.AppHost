using RestSharp;

namespace UpSkills.BlazorClient.Interfaces;

public interface IInternalApiRepository
{
    public Task<RestResponse> Request(string endPoint, Method method = Method.Get, object? jsonBodyContent = null);
}

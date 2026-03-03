using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using UpSkills.BlazorClient.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.BlazorClient.Repositories;

public class InternalApiRepository (HttpClient _HttpClient, EnviromentConfig _EnvConfig) : IInternalApiRepository
{
    private readonly string ApiServer = string.Empty;
    private RestClient Client = null!;

    private void SetApiClient()
    {
        //Omitir validación de certificados de confianza
        RestClientOptions clientOptions = new RestClientOptions(_EnvConfig.ApiServer)
        {
            //RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
            Timeout = TimeSpan.FromSeconds(30)
        };
        Client = new RestClient(_HttpClient, clientOptions);
    }

    //Generar una petición (Request) en la API Interna
    public async Task<RestResponse> Request(string endPoint, Method method = Method.Get, object? jsonBodyContent = null)
    {
        SetApiClient();

        RestRequest request = new($"{ApiServer}{endPoint}", method);
        string requestBody = JsonConvert.SerializeObject(jsonBodyContent, Formatting.Indented);

        if (null != jsonBodyContent)
        {
            request.AddBody(jsonBodyContent, ContentType.Json);
        }

        Debug.WriteLine(
            "Information" + 
            nameof(InternalApiRepository) +
            $"Request to: {ApiServer}{endPoint} ({method})" +
            $"\n\tRequestBody: {requestBody}"
        );

        RestResponse response = await Client.ExecuteAsync(request);

        string responseString;
        if (response.IsSuccessful)
        {
            responseString = JsonConvert.SerializeObject(response, Formatting.Indented);
            //responseString = "Successful";
        }
        else
        {
            if ((response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable) || (response.StatusCode == 0))
            {
                responseString = response.StatusDescription ?? (response.ErrorMessage ?? string.Empty);
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content ?? "{}");
                responseString = "Error: " + errorResponse?.message ?? string.Empty;
            }
        }

        Debug.WriteLine(
            nameof(InternalApiRepository) +
            (response.IsSuccessful ? "Information" : "Error") + ": " +
            $"Response from: {ApiServer}{endPoint}" +
            $"\n\tResponseBody: {responseString}"
        );

        return response;
    }
}

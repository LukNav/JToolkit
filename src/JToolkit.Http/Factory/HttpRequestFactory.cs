using System.Runtime.InteropServices.ComTypes;
using System.Text;
using JToolkit.Http.Exceptions;
using JToolkit.Http.Extensions;
using JToolkit.Http.Models;

namespace JToolkit.Http.Factory;

public class HttpRequestFactory
{
    private readonly HttpRequest _request;
    private readonly HttpClient _client;
    
    public HttpRequestFactory(HttpRequest request)
    {
        _request = request;
        _client = new HttpClient().WithHeaders(request.Headers);
    }

    public async Task<HttpResponseMessage> SendRequest() // TODO: Pass cancellation tokens
    {
        switch (_request.Method)
        {
            case RequestType.Get:
                return GetAsync().Result;
            case RequestType.Post:
                return PostAsync().Result;
            case RequestType.Delete:
                return DeleteAsync().Result;
            case RequestType.Put:
                return PutAsync().Result;
            default:
                throw new UnsupportedRequestTypeException($"The request method '{_request.Method}' is not supported. Supported methods: {string.Join(", ", Enum.GetNames(typeof(RequestType)))}");
        }
    }
    
    public async Task<HttpResponseMessage> GetAsync()
    {
        var response = await _client.GetAsync(_request.Url);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<HttpResponseMessage> PostAsync()
    {
        var content = new StringContent(_request.Body?.ToString() ?? "", Encoding.UTF8, "application/json");
        
        var response = await _client
            .PostAsync(_request.Url, content);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync()
    {
        var response = await _client
            .DeleteAsync(_request.Url);
        response.EnsureSuccessStatusCode();

        return response;
    }
    

    public async Task<HttpResponseMessage> PutAsync()
    {
        var content = new StringContent(_request.Body?.ToString() ?? "", Encoding.UTF8, "application/json");

        var response = await _client
            .PutAsync(_request.Url, content);
        response.EnsureSuccessStatusCode();

        return response;
    }
}
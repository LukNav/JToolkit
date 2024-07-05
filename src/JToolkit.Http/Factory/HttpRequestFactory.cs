using System.Text;
using JToolkit.Http.Exceptions;
using JToolkit.Http.Extensions;
using JToolkit.Http.Models;

namespace JToolkit.Http.Factory;

public class HttpRequestFactory
{
    private readonly HttpRequest _request;
    private readonly HttpRequestFields? _commonRequestFields;
    private readonly HttpClient _client;
    
    public HttpRequestFactory(HttpRequest request, HttpRequestFields? commonRequestFields)
    {
        _request = request;
        _commonRequestFields = commonRequestFields;
        _client = new HttpClient()
            .WithHeaders(request.Headers)
            .WithHeaders(commonRequestFields?.Headers);
    }

    public async Task<HttpResponseMessage> SendRequest() // TODO: Pass cancellation tokens
    {
        RequestType requestType = 
            _request.Method ?? 
            _commonRequestFields?.Method ?? 
            throw new RequestTypeRequiredException("Request type is required. It can be passed in the request or common request fields.");
        
        switch (requestType)
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
        var body = _request.Body?.ToString() ?? _commonRequestFields?.Body?.ToString() ?? "";
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        
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
        var body = _request.Body?.ToString() ?? _commonRequestFields?.Body?.ToString() ?? "";
        var content = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await _client
            .PutAsync(_request.Url, content);
        response.EnsureSuccessStatusCode();

        return response;
    }
}
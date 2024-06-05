using System.Text;

namespace JToolkit.Playground.Random.HttpTools;

public class HttpClient
{
    private readonly System.Net.Http.HttpClient _client;

    public HttpClient()
    {
        _client = new System.Net.Http.HttpClient();
    }
    
    public HttpClient WithBearerToken(string bearerToken)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        return this;
    }
    
    public HttpClient WithAuthTicket(string authTicket)
    {
        _client.DefaultRequestHeaders.Add("authTicket", authTicket);
        return this;
    }

    public async Task<string> GetAsync(string url)
    {
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> PostAsync(string url, string data)
    {
        var content = new StringContent(data, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        return response;
    }
}
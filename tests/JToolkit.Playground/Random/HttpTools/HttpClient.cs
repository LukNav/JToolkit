using System.Text;

namespace JToolkit.Playground.Random.HttpTools;

public class HttpClient
{
    private readonly System.Net.Http.HttpClient _client;
    private readonly string _bearerToken;

    public HttpClient(string bearerToken)
    {
        _client = new System.Net.Http.HttpClient();
        _bearerToken = bearerToken;
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
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
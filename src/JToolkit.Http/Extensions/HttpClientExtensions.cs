using System.Text;

namespace JToolkit.Http.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient WithHeaders(this HttpClient client, IReadOnlyDictionary<string,string>? headers)
    {
        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        return client;
    }
}
using System.Text;
using Newtonsoft.Json;

namespace CSharpApp.Application.Services;

public class HttpClientWrapper(HttpClient client)
{
    public async Task<T?> GetFromJsonAsync<T>(string requestUri)
    {
        var response = await client.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get data from {requestUri}");
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(content);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content)
    {
        var json = JsonConvert.SerializeObject(content);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(requestUri, httpContent);

        return response;
    }

    public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T content)
    {
        var json = JsonConvert.SerializeObject(content);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(requestUri, httpContent);

        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
    {
        var response = await client.DeleteAsync(requestUri);

        return response;
    }
}
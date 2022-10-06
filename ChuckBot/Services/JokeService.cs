using ChuckBot.Models;
using Newtonsoft.Json;

namespace ChuckBot.Services;

public interface IJokeService
{
    Task<string> GetRandomJokeAsync();
}

public class JokeService : IJokeService
{
    private readonly HttpClient _httpClient;
    public JokeService()
    {
        _httpClient = new HttpClient();
    }
    public async Task<string> GetRandomJokeAsync()
    {
        var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random/");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var joke = JsonConvert.DeserializeObject<Joke>(responseBody);
        return joke.value;
    }
}

using GrzesiekBot.Models;
using Newtonsoft.Json;

namespace GrzesiekBot.Services;

public interface IJokeService
{
    Task<string> GetRandomJoke();
}

public class JokeService : IJokeService
{
    private readonly HttpClient _httpClient;
    
    public JokeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> GetRandomJoke()
    {
        var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random/");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var joke = JsonConvert.DeserializeObject<Joke>(responseBody);
        return joke.value;
    }
}

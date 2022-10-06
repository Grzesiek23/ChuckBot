using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using ChuckBot.Handlers;
using ChuckBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChuckBot;

public class Program
{
    static IServiceProvider _services;
    private static IConfiguration _configuration;
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        _services = new ServiceCollection()
            .AddSingleton(new DiscordSocketConfig())
            .AddSingleton(configuration)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<IJokeService, JokeService>()
            .AddSingleton<CommandHandler>()
            .BuildServiceProvider();
        
        
        var client = _services.GetRequiredService<DiscordSocketClient>();
 
        client.Log += Log;
        
        await _services.GetRequiredService<CommandHandler>().InitializeAsync();
        
        await client.LoginAsync(TokenType.Bot, configuration.GetRequiredSection("Settings")["Token"]);
        await client.StartAsync();

        client.Ready += () =>
        {
            Console.WriteLine("Bot is connected!");
            client.SetStatusAsync(UserStatus.Online);
            client.SetGameAsync("Strażnik Teksasu");
            return Task.CompletedTask;
        };

        await Task.Delay(-1);
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
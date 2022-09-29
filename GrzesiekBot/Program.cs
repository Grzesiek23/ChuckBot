using Discord;
using Discord.Net;
using Discord.WebSocket;
using GrzesiekBot.Services;
using Newtonsoft.Json;

namespace GrzesiekBot;

internal static class Program
{
    private static DiscordSocketClient _client = null!;
    private static JokeService _jokeService = null!;
    
    private const long Guild = 0;
    private const string Token = "";

    public static async Task Main(string[] args)
    {
        _client = new DiscordSocketClient();
        _jokeService = new JokeService(new HttpClient());
        
        _client.Log += Log;
        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();

        _client.Ready += () =>
        {
            Console.WriteLine("Bot is connected!");
            _client.SetStatusAsync(UserStatus.Online);
            _client.SetGameAsync("Strażnik Teksasu");
            return Task.CompletedTask;
        };

        _client.Ready += BuildCommands;
        _client.SlashCommandExecuted += SlashCommandHandler;

        await Task.Delay(-1);
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private static async Task BuildCommands()
    {
        var guild = _client.GetGuild(Guild);
        var guildCommand = new SlashCommandBuilder();
        guildCommand.WithName("give-me-chuck");
        guildCommand.WithDescription("Let's see what we'll get!");

        var globalCommand = new SlashCommandBuilder();
        globalCommand.WithName("give-me-chuck");
        globalCommand.WithDescription("Let's see what we'll get!");

        try
        {
            await guild.CreateApplicationCommandAsync(guildCommand.Build());
            await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Message, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    private static async Task SlashCommandHandler(SocketSlashCommand command)
    {
        if (command.Data.Name == "give-me-chuck")
        {
            var joke = await _jokeService.GetRandomJoke();
            await command.RespondAsync(joke);
        }
    }
}
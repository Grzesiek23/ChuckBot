using Discord.Interactions;
using ChuckBot.Handlers;
using ChuckBot.Services;

namespace ChuckBot.Modules;

public class JokeModule : InteractionModuleBase
{
    public InteractionService Commands { get; set; }

    private readonly IJokeService _jokeService;
    private readonly CommandHandler _handler;

    public JokeModule(IJokeService jokeService, CommandHandler handler)
    {
        _jokeService = jokeService;
        _handler = handler;
    }

    [SlashCommand("give-me-chuck", "Let's see what we'll get!")]
    public async Task GiveMeChuck()
    {
        var joke = await _jokeService.GetRandomJokeAsync();
        await RespondAsync(joke);
    }
}
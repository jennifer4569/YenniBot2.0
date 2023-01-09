using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using YenniBotV2.Handlers;
using YenniBotV2.Settings;

namespace YenniBotV2
{
    public class YenniBot
    {
        private readonly ILogger<YenniBot> _logger;
        private readonly IYenniSettings _settings;
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;
        private readonly ReactionHandler _reactionHandler;

        public YenniBot(ILogger<YenniBot> logger, IYenniSettings settings, DiscordSocketClient client, CommandHandler handler, ReactionHandler reactionHandler)
        {
            _logger = logger;
            _settings = settings;
            _client = client;
            _commandHandler = handler;
            _reactionHandler = reactionHandler;
        }

        public async Task RunAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _settings.DiscordToken);
            await _client.StartAsync();
            await _commandHandler.InitCommandsAsync();
            _reactionHandler.InitReactions();

            _client.Log += Log;

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            _logger.LogInformation(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Reflection;
using YenniBotV2.Settings;

namespace YenniBotV2.Handlers
{
    public class CommandHandler
    {
        private readonly ILogger<YenniBot> _logger;
        private readonly IYenniSettings _settings;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(ILogger<YenniBot> logger, IYenniSettings settings, DiscordSocketClient client, CommandService commands, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _settings = settings;
            _client = client;
            _commands = commands;
            _serviceProvider = serviceProvider;
        }

        public async Task InitCommandsAsync()
        {
            _client.MessageReceived += OnMessageReceivedAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            // Don't process the command if it was a system message
            var message = socketMessage as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasStringPrefix(_settings.CommandPrefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _serviceProvider);
        }
    }
}
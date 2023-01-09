using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using YenniBotV2.DiscordHelpers;
using YenniBotV2.Repositories;

namespace YenniBotV2.Handlers
{
    public class ReactionHandler
    {
        private readonly ILogger<YenniBot> _logger;
        private readonly DiscordSocketClient _client;
        private readonly IRoleMessageRepository _roleMessageRepository;

        public ReactionHandler(ILogger<YenniBot> logger, DiscordSocketClient client, IRoleMessageRepository roleMessageRepository)
        {
            _logger = logger;
            _client = client;
            _roleMessageRepository = roleMessageRepository;
        }

        public void InitReactions()
        {
            _roleMessageRepository.Init();
            _client.ReactionAdded += OnReactionAddedAsync;
            _client.ReactionRemoved += OnReactionRemovedAsync;
        }

        private async Task OnReactionAddedAsync(
            Cacheable<IUserMessage, ulong> message,
            Cacheable<IMessageChannel, ulong> channel,
            SocketReaction socketReaction)
        {
            var msg = await message.GetOrDownloadAsync();
            var emoteStr = socketReaction.Emote.Name;
            if (!Emoji.TryParse(emoteStr, out _))
            {
                emoteStr = EmoteParser.EmoteToString((Emote)socketReaction.Emote);
            }
            var roleId = _roleMessageRepository.GetRoleIdForRoleReactionMessage(msg.Id, emoteStr);
            if (roleId > 0)
            {
                var guildChannel = msg.Channel as SocketGuildChannel;
                var user = guildChannel?.Guild.GetUser(socketReaction.UserId);
                if (user != null && !user.IsBot && !user.Roles.Any(role => role.Id == roleId))
                {
                    await user.AddRoleAsync(roleId);
                }
            }
        }

        private async Task OnReactionRemovedAsync(
            Cacheable<IUserMessage, ulong> message,
            Cacheable<IMessageChannel, ulong> channel,
            SocketReaction socketReaction)
        {
            var msg = await message.GetOrDownloadAsync();
            var emoteStr = socketReaction.Emote.Name;
            if (!Emoji.TryParse(emoteStr, out _))
            {
                emoteStr = EmoteParser.EmoteToString((Emote)socketReaction.Emote);
            }
            var roleId = _roleMessageRepository.GetRoleIdForRoleReactionMessage(msg.Id, emoteStr);
            if (roleId > 0)
            {
                var guildChannel = msg.Channel as SocketGuildChannel;
                var user = guildChannel?.Guild.GetUser(socketReaction.UserId);
                if (user != null && user.Roles.Any(role => role.Id == roleId))
                {
                    await user.RemoveRoleAsync(roleId);
                }
            }
        }
    }
}

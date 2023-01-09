using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using YenniBotV2.DataStores.Dbrs;
using YenniBotV2.DiscordHelpers;
using YenniBotV2.Repositories;

namespace YenniBotV2.Commands.AdminModules
{
    public class RoleMessageModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<RoleMessageModule> _logger;
        private readonly IRoleMessageRepository _roleMessageRepository;
        public RoleMessageModule(ILogger<RoleMessageModule> logger, IRoleMessageRepository roleMessageRepository)
        {
            _logger = logger;
            _roleMessageRepository = roleMessageRepository;
        }

        /* Example Usage:
            !roleMessage #CHANNEL_TO_SEND_TO TITLE_HERE
            [ EMOTE ][ ROLE_ID ][ DESCRIPTION ]
            [ EMOTE ][ ROLE_ID ][ DESCRIPTION ]
            ETC.

            !roleMessage #self-roles React here!
            [ 🐼 ][ 836292073258483722 ][Become a panda]
            [ 🤡 ][ 836292073258483722 ][ Become a big clown]
         */
        [Command("roleMessage")]
        [RequireOwner]
        public async Task RoleMessageAsync(string channelSpecialStr, [Remainder] string input)
        {
            try
            {
                var roleReactions = ParseInput(input, out var embedBuilder);
                var channelId = ChannelHelper.TryGetChannelIdFromSpecialString(channelSpecialStr, Context.Channel.Id);
                var channel = Context.Guild.GetTextChannel(channelId);
                var msg = await channel.SendMessageAsync(embed: embedBuilder.Build());

                _roleMessageRepository.AddRoleReactionMessages(roleReactions, msg.Id);
                foreach (var roleReaction in roleReactions)
                {
                    if (roleReaction.IsEmoji && Emoji.TryParse(roleReaction.Emote, out var emoji))
                    {
                        await msg.AddReactionAsync(emoji);
                    }
                    else if (!roleReaction.IsEmoji && Emote.TryParse(roleReaction.Emote, out var emote))
                    {
                        await msg.AddReactionAsync(emote);
                    }
                    else
                    {
                        _logger.LogWarning("Unable to parse Emoji/Emote [{Emote}]. Ignoring adding reaction to message.", roleReaction.Emote);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error in RolesModule.RoleMessageAsync");
                await Context.Message.ReplyAsync("Sorry, I ran into an internal error and could not complete the request.x");
            }
        }

        private List<RoleReactionMessageDbr> ParseInput(string input, out EmbedBuilder embedBuilder)
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var title = lines[0];

            var roleReactions = new List<RoleReactionMessageDbr>();
            var description = "";

            foreach (var line in lines)
            {
                if (line == title)
                {
                    continue;
                }

                // Format: [ EMOTE ][ ROLE_ID ][ DESCRIPTION ]
                var lineArgs = line.Substring(line.IndexOf('[') + 1, line.Length - 2).Split("][");
                var emoteStr = lineArgs[0].Trim();
                var roleId = Convert.ToUInt64(lineArgs[1].Trim());
                var roleDescription = lineArgs[2].Trim();

                var isEmoji = Emoji.TryParse(emoteStr, out _);
                roleReactions.Add(new RoleReactionMessageDbr
                {
                    RoleId = roleId,
                    Emote = emoteStr,
                    IsEmoji = isEmoji
                });

                description += $"{emoteStr} {roleDescription}\n";
            }

            embedBuilder = new EmbedBuilder
            {
                Title = title,
                Description = description,
                Color = Color.Red
            };
            return roleReactions;
        }
    }
}

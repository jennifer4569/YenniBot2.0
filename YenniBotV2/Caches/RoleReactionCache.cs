using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using YenniBotV2.DataStores.Dbrs;

namespace YenniBotV2.Caches
{
    public class RoleReactionCache : IRoleReactionCache
    {
        private readonly ILogger<RoleReactionCache> _logger;

        // MessageId --> ConcurrentDictionary<Emote, RoleReactionMessageDbr>
        private readonly ConcurrentDictionary<ulong, ConcurrentDictionary<string, RoleReactionMessageDbr>> _cache;
        public RoleReactionCache(ILogger<RoleReactionCache> logger)
        {
            _logger = logger;
            _cache = new ConcurrentDictionary<ulong, ConcurrentDictionary<string, RoleReactionMessageDbr>>();
        }

        public void AddRoleReactionMessages(List<RoleReactionMessageDbr> roleMessages, ulong overwriteMessageId = 0)
        {
            foreach (var roleMessage in roleMessages)
            {
                if (overwriteMessageId > 0)
                {
                    roleMessage.MessageId = overwriteMessageId;
                }
                var roleDataByRoleId = _cache.GetOrAdd(roleMessage.MessageId, new ConcurrentDictionary<string, RoleReactionMessageDbr>());
                if (roleMessage.Emote == null)
                {
                    _logger.LogWarning("Cannot add entry with null emote to RoleReactionCache. MessageId {MessageId} RoleId {RoleId} IsEmoji {IsEmoji}",
                        roleMessage.MessageId, roleMessage.RoleId, roleMessage.IsEmoji);
                    continue;
                }

                var success = roleDataByRoleId.TryAdd(roleMessage.Emote, roleMessage);
                if (!success)
                {
                    _logger.LogWarning("Duplicate entry in RoleReactionCache for MessageId {MessageId} and Emote {Emote}.",
                        roleMessage.MessageId, roleMessage.Emote);
                }
            }
        }

        public RoleReactionMessageDbr? GetRoleReactionMessage(ulong messageId, string emote)
        {
            if (_cache.TryGetValue(messageId, out var roleReactionByEmote) &&
                roleReactionByEmote.TryGetValue(emote, out var roleReactionMessage))
            {
                return roleReactionMessage;
            }
            return null;
        }
    }
}

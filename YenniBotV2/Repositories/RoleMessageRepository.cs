using Microsoft.Extensions.Logging;
using YenniBotV2.Caches;
using YenniBotV2.DataStores;
using YenniBotV2.DataStores.Dbrs;

namespace YenniBotV2.Repositories
{
    public class RoleMessageRepository : IRoleMessageRepository
    {
        private readonly ILogger<RoleMessageRepository> _logger;
        private readonly IRoleDataStore _dataStore;
        private readonly IRoleReactionCache _cache;
        public RoleMessageRepository(ILogger<RoleMessageRepository> logger, IRoleDataStore dataStore, IRoleReactionCache cache)
        {
            _logger = logger;
            _dataStore = dataStore;
            _cache = cache;
        }

        public void Init()
        {
            var roleMessages = _dataStore.GetAllRoleReactionMessages();
            _cache.AddRoleReactionMessages(roleMessages);
        }

        public void AddRoleReactionMessages(List<RoleReactionMessageDbr> roleReactions, ulong overwriteMessageId = 0)
        {
            _dataStore.AddRoleReactionMessages(roleReactions, overwriteMessageId);
            _cache.AddRoleReactionMessages(roleReactions, overwriteMessageId);
        }

        public ulong GetRoleIdForRoleReactionMessage(ulong messageId, string emote)
        {
            var roleReaction = _cache.GetRoleReactionMessage(messageId, emote);
            return roleReaction != null ? roleReaction.RoleId : 0;
        }
    }
}

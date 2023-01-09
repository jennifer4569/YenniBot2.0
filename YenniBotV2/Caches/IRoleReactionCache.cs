using YenniBotV2.DataStores.Dbrs;

namespace YenniBotV2.Caches
{
    public interface IRoleReactionCache
    {
        public void AddRoleReactionMessages(List<RoleReactionMessageDbr> roleMessages, ulong overwriteMessageId = 0);
        public RoleReactionMessageDbr? GetRoleReactionMessage(ulong messageId, string emote);
    }
}

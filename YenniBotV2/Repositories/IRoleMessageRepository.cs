using YenniBotV2.DataStores.Dbrs;

namespace YenniBotV2.Repositories
{
    public interface IRoleMessageRepository
    {
        public void Init();
        public void AddRoleReactionMessages(List<RoleReactionMessageDbr> roleReactions, ulong overwriteMessageId = 0);
        public ulong GetRoleIdForRoleReactionMessage(ulong messageId, string emote);
    }
}

using YenniBotV2.DataStores.Dbrs;

namespace YenniBotV2.DataStores
{
    public interface IRoleDataStore
    {
        public void AddRoleReactionMessages(List<RoleReactionMessageDbr> roleReactionMessages, ulong overwriteMessageId = 0);
        public List<RoleReactionMessageDbr> GetAllRoleReactionMessages();
    }
}

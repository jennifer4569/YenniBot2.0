using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using YenniBotV2.DataStores.Dbrs;
using YenniBotV2.Settings;

namespace YenniBotV2.DataStores
{
    public class RoleDataStore : IRoleDataStore
    {
        private readonly ILogger<RoleDataStore> _logger;
        private readonly IYenniSettings _settings;
        public RoleDataStore(ILogger<RoleDataStore> logger, IYenniSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public void AddRoleReactionMessages(List<RoleReactionMessageDbr> roleReactionMessages, ulong overwriteMessageId = 0)
        {
            try
            {
                using var db = new MySqlConnection(_settings.DBConnectionString);
                foreach (var roleReactionMessage in roleReactionMessages)
                {
                    AddToRoleReactionMessage(db,
                        overwriteMessageId == 0 ? roleReactionMessage.MessageId : overwriteMessageId,
                        roleReactionMessage.RoleId,
                        roleReactionMessage.Emote ?? "",
                        roleReactionMessage.IsEmoji);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered Error in RoleDataStore.AddToRoleReactionMessage");
                throw;
            }
        }

        private static void AddToRoleReactionMessage(MySqlConnection db, ulong messageId, ulong roleId, string emote, bool isEmoji)
        {
            db.Query("CALL AddToRoleReactionMessage(@pMessageId, @pRoleId, @pEmote, @pIsEmoji);",
                new
                {
                    pMessageId = messageId,
                    pRoleId = roleId,
                    pEmote = emote,
                    pIsEmoji = isEmoji ? 1 : 0
                });
        }

        public List<RoleReactionMessageDbr> GetAllRoleReactionMessages()
        {
            try
            {
                using var db = new MySqlConnection(_settings.DBConnectionString);
                return db.Query<RoleReactionMessageDbr>("CALL GetAllRoleReactionMessages();").ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered Error in RoleDataStore.GetAllRoleReactionMessages");
                throw;
            }
        }
    }
}

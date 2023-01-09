namespace YenniBotV2.DiscordHelpers
{
    public class RolesHelper
    {
        private static readonly string specialChar = "@&";

        public static ulong TryGetRoleIdFromSpecialString(string specialStr, ulong defaultId = 0)
        {
            return GenericParser.TryGetIdFromSpecialString(specialStr, defaultId, specialChar);
        }

        public static string RoleIdToSpecialString(ulong id)
        {
            return GenericParser.IdToSpecialString(id, specialChar);
        }
    }
}

namespace YenniBotV2.DiscordHelpers
{
    public class ChannelHelper
    {
        private static readonly string specialChar = "#";

        public static ulong TryGetChannelIdFromSpecialString(string specialStr, ulong defaultId = 0)
        {
            return GenericParser.TryGetIdFromSpecialString(specialStr, defaultId, specialChar);
        }

        public static string ChannelIdToSpecialString(ulong id)
        {
            return GenericParser.IdToSpecialString(id, specialChar);
        }
    }
}

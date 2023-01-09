namespace YenniBotV2.DiscordHelpers
{
    public class GenericParser
    {
        // See https://discord.com/developers/docs/reference#message-formatting for formatting

        public static ulong TryGetIdFromSpecialString(string specialStr, ulong defaultId, string specialChar)
        {
            try
            {
                var idStr = specialStr[(specialStr.IndexOf(specialChar) + specialChar.Length)..specialStr.IndexOf('>')];
                return Convert.ToUInt64(idStr);
            }
            catch
            {
                return defaultId;
            }
        }

        public static string IdToSpecialString(ulong id, string specialChar)
        {
            return $"<{specialChar}{id}>";
        }
    }
}

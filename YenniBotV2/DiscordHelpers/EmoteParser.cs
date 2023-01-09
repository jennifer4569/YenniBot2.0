using Discord;

namespace YenniBotV2.DiscordHelpers
{
    public class EmoteParser
    {
        public static string EmoteToString(Emote customEmote)
        {
            return $"<:{customEmote.Name}:{customEmote.Id}>";
        }
    }
}

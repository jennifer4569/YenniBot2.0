using System.Configuration;

namespace YenniBotV2.Settings
{
    public class YenniSettings : IYenniSettings
    {
        public string Name => ConfigurationManager.AppSettings["Name"] ?? "YenniBot2.0";
        public string DiscordToken => ConfigurationManager.AppSettings["DiscordToken"] ?? "";
        public string CommandPrefix => ConfigurationManager.AppSettings["CommandPrefix"] ?? "!";
        public string DBConnectionString => ConfigurationManager.AppSettings["DBConnectionString"] ?? "";
    }
}

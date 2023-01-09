namespace YenniBotV2.Settings
{
    public interface IYenniSettings
    {
        public string Name { get; }
        public string DiscordToken { get; }
        public string CommandPrefix { get; }
        public string DBConnectionString { get; }
    }
}

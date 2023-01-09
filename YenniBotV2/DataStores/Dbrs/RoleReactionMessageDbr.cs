namespace YenniBotV2.DataStores.Dbrs
{
    public class RoleReactionMessageDbr
    {
        public ulong MessageId { get; set; }
        public ulong RoleId { get; set; }
        public string? Emote { get; set; }
        public bool IsEmoji { get; set; }
    }
}

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using YenniBotV2.Caches;
using YenniBotV2.DataStores;
using YenniBotV2.Handlers;
using YenniBotV2.Repositories;
using YenniBotV2.Settings;

namespace YenniBotV2
{
    public class Startup
    {
        public static Task Main(string[] args) => MainAsync(args);

        public static async Task MainAsync(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var bot = host.Services.GetRequiredService<YenniBot>();
            await bot.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Config Settings
                    var config = new DiscordSocketConfig
                    {
                        MessageCacheSize = 100,
                        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                    };
                    services.TryAddSingleton<IYenniSettings, YenniSettings>();
                    services.TryAddSingleton(config);

                    // Discord.Net Stuffs
                    services.TryAddSingleton<DiscordSocketClient>();
                    services.TryAddSingleton<CommandService>();

                    // My Stuffs
                    services.TryAddSingleton<YenniBot>();
                    services.TryAddSingleton<CommandHandler>();
                    services.TryAddSingleton<ReactionHandler>();

                    // DataStores
                    services.TryAddSingleton<IRoleDataStore, RoleDataStore>();

                    // Repositories
                    services.TryAddSingleton<IRoleMessageRepository, RoleMessageRepository>();

                    // Caches
                    services.TryAddSingleton<IRoleReactionCache, RoleReactionCache>();
                });
    }
}

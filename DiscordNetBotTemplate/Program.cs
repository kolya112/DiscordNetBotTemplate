using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordNetBotTemplate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        static internal DiscordSocketClient? _client;
        static internal CommandService? _commands;
        static internal IServiceProvider? _services;
        static internal InteractionService? _interaction;

        private async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 1000,
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = true,

                GatewayIntents =
                GatewayIntents.Guilds |
                GatewayIntents.GuildMembers |
                GatewayIntents.GuildBans |
                GatewayIntents.GuildEmojis |
                GatewayIntents.GuildMessages |
                GatewayIntents.GuildMessageTyping |
                GatewayIntents.GuildMessageReactions |
                GatewayIntents.GuildVoiceStates |
                GatewayIntents.MessageContent
            });

            _commands = new CommandService();

            // Sevices

            _services = new ServiceCollection()
                .AddSingleton(_commands)
                .AddSingleton(_client)
                .BuildServiceProvider();

            // Events

            _client.Ready += BotModules.Ready;
            _client.Log += BotEvents.Log;
            _client.MessageReceived += BotEvents.MessageHandler;
            _client.InteractionCreated += BotModules.OnInteractionAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, Constants._botToken);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
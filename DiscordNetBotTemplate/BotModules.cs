using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordNetBotTemplate
{
    internal class BotModules
    {
        /// <summary>
        /// Bot ready event handler
        /// </summary>
        /// <returns></returns>
        public static async Task Ready()
        {
            Program._interaction = new InteractionService(Program._client);
            await Program._interaction.AddModulesAsync(Assembly.GetEntryAssembly(), Program._services);
            //await Program._interaction.RegisterCommandsToGuildAsync(guildID); // Register slash bot commands to current discord guild
        }

        /// <summary>
        /// Bot InteractionCreated event handler
        /// </summary>
        /// <param name="interaction"></param>
        /// <returns></returns>
        public static async Task OnInteractionAsync(SocketInteraction interaction)
        {
            try
            {
                var scope = Program._services.CreateScope();
                var context = new SocketInteractionContext(Program._client, interaction);

                // Not handle interactrion from bot and webhook
                if (context.User.IsBot || context.User.IsWebhook)
                    return;

                var result = await Program._interaction.ExecuteCommandAsync(context, scope.ServiceProvider);
            }
            catch
            {
                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    await interaction.GetOriginalResponseAsync()
                        .ContinueWith(msg => msg.Result.DeleteAsync());
                }
            }
        }
    }
}
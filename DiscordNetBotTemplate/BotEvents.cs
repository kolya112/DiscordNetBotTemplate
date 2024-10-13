using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;

namespace DiscordNetBotTemplate
{
    internal class BotEvents
    {
        internal static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        internal static async Task MessageHandler(SocketMessage arg)
        {
            if (arg is not SocketUserMessage { Source: MessageSource.User } message)
                return;

            var msg = arg as SocketUserMessage;

            var guildUser = arg.Author as SocketGuildUser;

            var context = new SocketCommandContext(Program._client, msg);
        }
    }
}

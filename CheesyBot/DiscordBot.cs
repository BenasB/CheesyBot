using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace CheesyBot
{
    public class DiscordBot
    {
        DiscordSocketClient client;
        CommandService commands;
        IServiceProvider services;

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();

            string botToken = Auth.GetBotToken();

            if (botToken == null)
            {
                Console.ReadKey();
                return;
            }

            // Event subscriptions
            client.Log += Log;
            client.UserJoined += AnnounceUserJoined;
            client.UserLeft += UserLeft;
            client.UserBanned += UserBanned;
            client.ChannelCreated += ChannelCreated;

            await client.SetGameAsync("Eating cheese");

            await RegisterCommandsAsync();

            await client.LoginAsync(TokenType.Bot, botToken);       

            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task ChannelCreated(SocketChannel channel)
        {
            if (channel is SocketTextChannel)
                await (channel as SocketTextChannel).SendMessageAsync("A wild channel appears");
        }

        private async Task UserLeft(SocketGuildUser user)
        {
            SocketGuild guild = user.Guild;
            SocketTextChannel channel = guild.DefaultChannel;
            await channel.SendMessageAsync($"Farewell, {user.Mention}.");
        }

        private async Task UserBanned(SocketUser user, SocketGuild guild)
        {
            SocketTextChannel channel = guild.DefaultChannel;
            await channel.SendMessageAsync($"{user.Mention} was hit by a ban hammer");
        }

        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            SocketGuild guild = user.Guild;
            SocketTextChannel channel = guild.DefaultChannel;
            await channel.SendMessageAsync($"Welcome, {user.Mention} to the {guild.Name}.");
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            SocketUserMessage message = (SocketUserMessage)arg;

            if (message is null || message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                SocketCommandContext context = new SocketCommandContext(client, message);

                var result = await commands.ExecuteAsync(context, argPos, services);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}

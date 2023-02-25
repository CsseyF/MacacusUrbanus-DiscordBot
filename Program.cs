using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using MacacusUrbanusBot.Entities.Configuration;
using MacacusUrbanusBot.Service;

namespace MyFirstBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;
        public CommandService _commands;
        private IServiceProvider _services;


        private SocketGuild guild;
        //log
        private ulong LogChannelID;

        public async Task RunBotAsync()
        {
            var discConfig = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All
            };

            var json = File.ReadAllText("../../../appSettings.json");
            var configurations = JsonSerializer.Deserialize<EnvConfiguration>(json);

            _client = new DiscordSocketClient(discConfig);
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();



            string token = configurations.Token;

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        }

        public async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            int argPos = 0;

            if (message.HasStringPrefix("--", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }

        }
    }
}
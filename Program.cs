using System;
using System.IO;
using System.Threading;

using Microsoft.Extensions.Configuration;

using Telegram.Bot;

using CnKei.SekiRobot.Applications;

namespace CnKei.SekiRobot {
    class Program {
        static void Main(string[] args) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            var bot = new TelegramBotClient(configuration.GetSection("AccessToken").Value);
            var me = bot.GetMeAsync().Result;
            Console.WriteLine(
                $"I am user {me.Id} and my name is {me.FirstName}."
            );
            var lastMessage = new LastMessage(bot);

            bot.OnMessage += lastMessage.OnMessage;
            bot.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
    }
}

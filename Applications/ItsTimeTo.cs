
using Telegram.Bot;
using Telegram.Bot.Args;

namespace CnKei.SekiRobot.Applications {

    public class ItsTimeTo {

        TelegramBotClient bot;

        public ItsTimeTo(TelegramBotClient bot) {
            this.bot = bot;
        }

        public async void OnMessage(object sender, MessageEventArgs e) {
            var user = e.Message.From;
            if (!e.Message.Text.StartsWith("该")) {
                return;
            }
            var thingToDo = e.Message.Text[1];
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                replyToMessageId: e.Message.MessageId,
                text:   $"{thingToDo}啊当然{thingToDo}"
            );
        }
    }
}

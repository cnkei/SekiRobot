using System.Linq;
using System.Text;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;

namespace CnKei.SekiRobot.Applications {

    public class ItsTimeTo {

        TelegramBotClient bot;

        public ItsTimeTo(TelegramBotClient bot) {
            this.bot = bot;
        }

        public async void OnMessage(object sender, MessageEventArgs e) {
            var user = e.Message.From;
            if (e.Message.Text == null || !e.Message.Text.StartsWith("该")) {
                return;
            }
            var sb = new StringBuilder();
            foreach (var c in e.Message.Text.Skip(1)) {
                if (c > sbyte.MaxValue) {
                    if (sb.Length == 0) {
                        sb.Append(c);
                    }
                    break;
                }
                sb.Append(c);
            }
            var thingToDo = sb.ToString();
            try {
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    replyToMessageId: e.Message.MessageId,
                    text:   $"{thingToDo}啊当然{thingToDo}"
                );
            } catch (ApiRequestException) {
            }
        }
    }
}

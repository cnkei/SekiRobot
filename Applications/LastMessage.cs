using System;
using System.Linq;
using System.Text;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

using CnKei.SekiRobot.Data;

namespace CnKei.SekiRobot.Applications {
    class LastMessage {

        TelegramBotClient bot;
        ContactContext db;

        public LastMessage(TelegramBotClient bot) {
            this.bot = bot;
            db = new ContactContext();
        }

        public async void OnMessage(object sender, MessageEventArgs e) {
            var user = e.Message.From;
            var chat = e.Message.Chat;
            if (user.IsBot) {
                return;
            }
            // var contact = db.Contacts.SingleOrDefault(c => c.Id == user.Id);
            var contact = (from c in db.Contacts where c.Id == user.Id select c).SingleOrDefault<Contact>();
            if (contact == null) {
                db.Contacts.Add(new Contact{Id=user.Id, Username=user.Username, FirstName=user.FirstName, LastName=user.LastName, LastSeen=e.Message.Date});
            } else {
                contact.Username = user.Username;
                contact.FirstName = user.FirstName;
                contact.LastName = user.LastName;
                contact.LastSeen = e.Message.Date;
            }
            if (chat.Type == ChatType.Group || chat.Type == ChatType.Supergroup) {
                var chatMember = (from cm in db.ChatMembers where cm.ChatId == chat.Id && cm.UserId == user.Id select cm).SingleOrDefault<ChatMember>();
                if (chatMember == null) {
                    db.ChatMembers.Add(new ChatMember{ChatId=chat.Id, UserId=user.Id});
                }
            }
            await db.SaveChangesAsync();

            if ((chat.Type == ChatType.Group || chat.Type == ChatType.Supergroup) && e.Message.Text == "/last") {
                var members = (from cm in db.ChatMembers
                               join c in db.Contacts
                               on cm.UserId equals c.Id
                               where cm.ChatId == chat.Id && c.Id != user.Id
                               orderby c.LastSeen
                               select c).ToList();
                var sb = new StringBuilder();
                foreach (var c in members) {
                    sb.AppendLine($"{c.FirstName}{c.LastName}在{e.Message.Date.Subtract(c.LastSeen)}前出现");
                }
                if (sb.Length == 0) {
                    await bot.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text:   "这里只剩你了"
                    );
                    return;
                }
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:   sb.ToString()
                );
            }
        }
    }
}

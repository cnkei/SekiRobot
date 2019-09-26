using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace CnKei.SekiRobot.Data {

    public class ContactContext : DbContext {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=seki_robot.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ChatMember>().HasKey(cm => new { cm.ChatId, cm.UserId });
        }
    }

    public class Contact {
        [Key]
        public Int64 Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public class ChatMember {
        public Int64 ChatId { get; set; }
        public Int64 UserId { get; set; }
    }
}

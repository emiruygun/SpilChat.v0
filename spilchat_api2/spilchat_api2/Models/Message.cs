using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace spilchat_api.Models
{
    [Table("Messages")] // tablo adı dbo.Messages
    public class Message
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FromUser { get; set; } = null!;

        [Required, MaxLength(50)]
        public string ToUser { get; set; } = null!;

        // DB’de kolon adı "Message", ama class içinde adı "Text"
        [Required]
        [Column("Message")]
        [JsonPropertyName("message")] // JSON’da yine "message" anahtarıyla gelsin/gitsin
        public string Text { get; set; } = null!;

        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spilchat_api.Models;

namespace spilchat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly SpilchatDbContext _context;
        public ChatController(SpilchatDbContext context) => _context = context;

        // -------------------- DTO'lar --------------------
        public class ConversationDto
        {
            public string Peer { get; set; } = null!;
            public DateTime LastTime { get; set; }
            public string Text { get; set; } = null!;
        }

        public class ChatSummaryDto
        {
            public string OtherUser { get; set; } = null!;
            public string LastMessage { get; set; } = string.Empty;
            public DateTime LastTimestamp { get; set; }
            public int UnreadCount { get; set; }
        }

        public class MarkReadDto
        {
            public string CurrentUser { get; set; } = null!;
            public string OtherUser { get; set; } = null!;
        }

        // -------------------- Endpoints --------------------

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message, CancellationToken ct)
        {
            if (message == null ||
                string.IsNullOrWhiteSpace(message.FromUser) ||
                string.IsNullOrWhiteSpace(message.ToUser) ||
                string.IsNullOrWhiteSpace(message.Text))         // entity property: Text
            {
                return BadRequest("Geçersiz mesaj.");
            }

            _context.Messages.Add(message);                       // Timestamp DB default (GETDATE())
            await _context.SaveChangesAsync(ct);
            return Ok("Mesaj kaydedildi.");
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetChatHistory(
            [FromQuery] string from, [FromQuery] string to, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Kullanıcı adı eksik.");

            var messages = await _context.Messages
                .Where(m => (m.FromUser == from && m.ToUser == to) ||
                            (m.FromUser == to && m.ToUser == from))
                .OrderBy(m => m.Timestamp)
                .ToListAsync(ct);

            return Ok(messages);
        }

        // Son konuşmalar (WhatsApp tarzı liste) – tek sorgu
        [HttpGet("chats/summary/{username}")]
        public async Task<IActionResult> GetChatSummaries(string username, CancellationToken ct)
        {
            var query =
                from m in _context.Messages
                where m.FromUser == username || m.ToUser == username
                let other = (m.FromUser == username ? m.ToUser : m.FromUser)
                group m by other into g
                select new ChatSummaryDto
                {
                    OtherUser = g.Key,
                    LastTimestamp = g.Max(x => x.Timestamp),
                    LastMessage = g.OrderByDescending(x => x.Timestamp)
                                   .Select(x => x.Text)               // entity: Text
                                   .FirstOrDefault() ?? string.Empty,
                    UnreadCount = g.Count(x => x.FromUser == g.Key &&
                                              x.ToUser == username &&
                                              !x.IsRead)
                };

            var list = await query
                .OrderByDescending(x => x.LastTimestamp)
                .ToListAsync(ct);

            return Ok(list);
        }

        // "Kullanıcıya göre son konuşmalar" (basit özet)
        [HttpGet("recent/{user}")]
        public async Task<IActionResult> GetRecent(string user, CancellationToken ct)
        {
            var query =
                from m in _context.Messages
                where m.FromUser == user || m.ToUser == user
                let peer = (m.FromUser == user ? m.ToUser : m.FromUser)
                group m by peer into g
                select new ConversationDto
                {
                    Peer = g.Key,
                    LastTime = g.Max(x => x.Timestamp),
                    Text = g.OrderByDescending(x => x.Timestamp)
                            .Select(x => x.Text)                    // entity: Text
                            .FirstOrDefault() ?? string.Empty
                };

            var dto = await query
                .OrderByDescending(x => x.LastTime)
                .ToListAsync(ct);

            return Ok(dto);
        }

        // Okundu işaretleme
        [HttpPost("chats/mark-read")]
        public async Task<IActionResult> MarkRead([FromBody] MarkReadDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.CurrentUser) || string.IsNullOrWhiteSpace(dto.OtherUser))
                return BadRequest("Geçersiz istek.");

            var msgs = await _context.Messages
                .Where(m => m.FromUser == dto.OtherUser &&
                            m.ToUser == dto.CurrentUser &&
                            !m.IsRead)
                .ToListAsync(ct);

            if (msgs.Count == 0) return Ok(); // yapılacak iş yok

            foreach (var m in msgs) m.IsRead = true;
            await _context.SaveChangesAsync(ct);

            return Ok();
        }
    }
}

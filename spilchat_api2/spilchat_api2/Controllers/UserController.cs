using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spilchat_api.Models;

namespace spilchat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SpilchatDbContext _context;
        public UserController(SpilchatDbContext context) => _context = context;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser, CancellationToken ct)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginUser.Username && u.Password == loginUser.Password, ct);

            if (user == null) return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            return Ok(new { username = user.Username });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsernames(CancellationToken ct)
        {
            var usernames = await _context.Users.Select(u => u.Username).ToListAsync(ct);
            return Ok(usernames);
        }
    }
}

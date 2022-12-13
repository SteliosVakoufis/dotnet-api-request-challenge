using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api.Model;

namespace web_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class IPController : ControllerBase
    {
        private readonly DataContext _context;

        public IPController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{ip}")]
        public async Task<ActionResult<string>> GetIPInfo(string ip)
        {
            return Ok(ip);
        }
    }
}

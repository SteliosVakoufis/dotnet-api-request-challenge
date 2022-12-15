using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using web_api.Model;
using web_api.Services;

namespace web_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class IPController : ControllerBase
    {
        private readonly IIPService _service;

        public IPController(IIPService service)
        {
            _service = service;
        }

        [HttpGet("ip")]
        [EnableQuery]
        public async Task<ActionResult<List<IPInfoEntity>>> GetAllIpInfo()
        {
            return Ok(await _service.GetAllIpDetails());
        }

        [HttpGet("{ip}")]
        [EnableQuery]
        public async Task<ActionResult> GetIPInfo(string ip)
        {
            return Ok(await _service.GetIpDetails(ip));
        }
    }
}

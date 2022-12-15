using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{ip}")]
        public async Task<ActionResult> GetIPInfo(string ip)
        {
            try
            {
                return Ok(await _service.GetIpDetails(ip));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateIPInfo([FromBody] List<IPInfoEntity> entities)
        {
            try
            {
                return Ok(await _service.UpdateIpDetails(entities));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

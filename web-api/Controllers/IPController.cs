using Microsoft.AspNetCore.Mvc;
using web_api.Jobs;
using web_api.Model;
using web_api.Services;

namespace web_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class IPController : ControllerBase
    {
        private readonly IIPService _service;
        private readonly BackgroundJobs _jobs;

        public IPController(IIPService service, BackgroundJobs jobs)
        {
            _service = service;
            _jobs = jobs;
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

        [HttpGet("job/{id}")]
        public ActionResult GetJobInfo(Guid id)
        {
            return Ok(_service.GetJobInfo(id));
        }

        [HttpPatch("job")]
        public ActionResult CreateUpdateJob([FromBody] Queue<IPInfoEntity> entities)
        {
            try
            {
                return Ok(_service.CreateNewUpdateJob(entities));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Microsoft.AspNetCore.Mvc;
using web_api.BackgroundWorkers;
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

        [HttpPost("job")]
        public ActionResult StartJob([FromBody] Stack<IPInfoEntity> entities)
        {
            var id = BackgroundJob.Enqueue<IWorkerDb>(x => x.UpdateDb(entities));

            return Ok(id);
        }

        [HttpGet("/job/status/{id}")]
        public async Task<ActionResult> GetJobStatus(string id)
        {
            var conn = JobStorage.Current.GetMonitoringApi();
            var jobData = conn.JobDetails(id);
            
            if (jobData.History[0].Data.ContainsKey("Result"))
            {
                return Ok(jobData.History[0].Data["Result"]);
            }

            return Ok(jobData.History[0].StateName);
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
        public async Task<ActionResult> BatchUpdateIPInfo([FromBody] List<IPInfoEntity> entities)
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

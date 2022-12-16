using Hangfire.Storage;
using ipstack_lib.exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using web_api.Model;
using web_api.Services;

namespace web_api.BackgroundWorkers
{
    public class WorkerDbImpl : IWorkerDb
    {
        private readonly IIPService _service;

        private readonly int _buffersize = 10;

        public WorkerDbImpl(IIPService service)
        {
            _service = service;
        }

        public async Task<ActionResult> UpdateDb(Stack<IPInfoEntity> entities)
        {
            if (entities is null)
            {
                throw new IPServiceNotAvailableException("Invalid request, Please try again.");
            }

            var result = new List<IPInfoEntity>();

            while(entities.Count > 0)
            {
                var batch = new List<IPInfoEntity>();

                for(int i = 0; i < _buffersize; i++)
                {
                    if (entities.Count == 0) break;
                    batch.Add(entities.Pop());
                }

                try
                {
                    var updated = await _service.UpdateIpDetails(batch!);
                    result.AddRange(updated);
                }
                catch (Exception)
                {
                    throw;
                }

                Thread.Sleep(5000);
            }

            return new OkObjectResult(result);
        }
    }
}

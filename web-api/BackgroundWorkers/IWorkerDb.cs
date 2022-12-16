using Microsoft.AspNetCore.Mvc;
using web_api.Model;

namespace web_api.BackgroundWorkers
{
    public interface IWorkerDb
    {
        public Task<ActionResult> UpdateDb(Stack<IPInfoEntity> entities);
    }
}

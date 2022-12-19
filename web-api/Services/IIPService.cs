using Microsoft.AspNetCore.Mvc;
using web_api.Jobs;
using web_api.Model;

namespace web_api.Services
{
    public interface IIPService
    {
        public Task<IPInfoEntity> GetIpDetails(string ip);
        public Task<List<IPInfoEntity>> UpdateIpDetails(List<IPInfoEntity> entities);
        public Guid CreateNewUpdateJob(Queue<IPInfoEntity> entities);
        public JobDTO GetJobInfo(Guid id);
        public void SetCompletedJobInfo(JobDTO job);
    }
}

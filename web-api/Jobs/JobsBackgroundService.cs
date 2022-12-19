using web_api.Model;
using web_api.Services;

namespace web_api.Jobs
{
    public class JobsBackgroundService : BackgroundService
    {
        private readonly BackgroundJobs _jobs;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<JobsBackgroundService> _logger;

        private readonly int _buffersize = 10;

        public JobsBackgroundService(BackgroundJobs jobs, IServiceProvider serviceScopeFactory, ILogger<JobsBackgroundService> logger)
        {
            _jobs = jobs;
            _serviceProvider = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_jobs.QueuedTasks.Keys.Count == 0)
                {
                    //_logger.LogInformation("The job queue is empty {time}", DateTime.UtcNow.ToLongTimeString());
                    await Task.Delay(1000);
                    continue;
                }

                if (_jobs.QueuedTasks.TryGetValue(_jobs.QueuedTasks.Keys.First(), out var job))
                {
                    if (job is null || job.ProcessQueue is null || job.ProcessQueue.Count == 0)
                    {
                        _jobs.QueuedTasks.Remove(job.Id);
                        continue;
                    }

                    _logger.LogInformation("Job started!\nguid: {Id}\ntime: {time}\n", job.Id, DateTime.UtcNow.ToLongTimeString());

                    await ProcessJob(job, stoppingToken);

                    //_jobs.CompletedTasks.Add(job.Id, job);

                    _jobs.QueuedTasks.Remove(job.Id);

                    _logger.LogInformation("Job completed!\nguid: {Id}\ntime: {time}\n", job.Id, DateTime.UtcNow.ToLongTimeString());
                }
            }
        }

        private async Task ProcessJob(JobDTO job, CancellationToken stoppingToken)
        {
            job.Status = "Processing";
            var entities = job.ProcessQueue;

            if (entities is null || entities.Count == 0)
            {
                job.Status = "Partial Finish";
                job.Error = "No values to process.";
                job.Message = "Request provided had no values, Please try again.";
                job.ProcessQueue = null;
                job.Progress = null;
                return;
            }

            job.Progress!.Current = 0;
            job.Progress!.End = entities.Count;

            var result = new List<IPInfoEntity>();

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IIPService _service = scope.ServiceProvider.GetRequiredService<IIPService>();

                while (entities.Count > 0)
                {
                    var batch = new List<IPInfoEntity>();

                    for (int i = 0; i < _buffersize; i++)
                    {
                        if (entities.Count == 0) break;
                        batch.Add(entities.Dequeue());
                    }

                    try
                    {
                        var updated = await _service.UpdateIpDetails(batch!);
                        result.AddRange(updated);

                        job.Progress.Current += updated.Count;
                    }
                    catch (Exception e)
                    {
                        job.Status = "Partial Finish";
                        job.Error = e.Message;
                        job.Message = "Potentialy some data may not have been updated.";
                        job.ProcessQueue = null;
                        job.Result = null;
                        return;
                    }
                }

                job.Status = "Finished";
                job.Message = "Job finished with no issues.";
                job.ProcessQueue = null;
                job.Progress = null;
                job.Result = result;

                _service.SetCompletedJobInfo(job);
            }
        }
    }
}

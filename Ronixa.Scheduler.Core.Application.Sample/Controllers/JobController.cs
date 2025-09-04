namespace Ronixa.Scheduler.Core.Application.Sample.Controllers
{
    [ApiController]
    [Route("jobs")]
    public class JobController(IRonixaJobManagerService ronixaJobManager) : ControllerBase
    {
        [HttpGet("/play")]
        public IActionResult Play() => GetSampleJobStatus(() => ronixaJobManager.StartJob(JobIds.SampleJobId));

        [HttpGet("/pause")]
        public IActionResult Pause() => GetSampleJobStatus(() => ronixaJobManager.PauseJob(JobIds.SampleJobId));

        [HttpGet("/stop")]
        public IActionResult Stop() => GetSampleJobStatus(() => ronixaJobManager.StopJob(JobIds.SampleJobId));

        [HttpGet("/status")]
        public IActionResult Status() => GetSampleJobStatus();

        [HttpGet("/list")]
        public IActionResult List() => Ok(ronixaJobManager.GetAll());

        private OkObjectResult GetSampleJobStatus(Action? jobAction = null)
        {
            jobAction?.Invoke();
            return Ok(ronixaJobManager.GetJobStatus(JobIds.SampleJobId));
        }
    }
}

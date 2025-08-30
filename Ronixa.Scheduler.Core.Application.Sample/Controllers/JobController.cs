namespace Ronixa.Scheduler.Core.Application.Sample.Controllers
{
    [ApiController]
    [Route("jobs")]
    public class JobController : ControllerBase
    {
        private readonly IRonixaJobManagerService _ronixaJobManager;

        public JobController(IRonixaJobManagerService ronixaJobManager)
            => _ronixaJobManager = ronixaJobManager;

        [HttpGet("/play")]
        public IActionResult Play()
        {
            var job = _ronixaJobManager.GetJobStatus(Guid.Parse("EDF401E5-4FB7-4B98-B6B8-68528F617432"));
            _ronixaJobManager.StartJob(job!.Id);

            return Ok(_ronixaJobManager.GetJobStatus(Guid.Parse("EDF401E5-4FB7-4B98-B6B8-68528F617432")));
        }

        [HttpGet("/pause")]
        public IActionResult Pause()
        {
            var job = _ronixaJobManager.GetJobStatus(Guid.Parse("EDF401E5-4FB7-4B98-B6B8-68528F617432"));
            _ronixaJobManager.PauseJob(job!.Id);

            return Ok(_ronixaJobManager.GetJobStatus(Guid.Parse("EDF401E5-4FB7-4B98-B6B8-68528F617432")));
        }

        [HttpGet("/stop")]
        public IActionResult Stop()
        {
            var job = _ronixaJobManager.GetJobStatus(Guid.Parse("EDF401E5-4FB7-4B98-B6B8-68528F617432"));
            _ronixaJobManager.StopJob(job!.Id);

            return Ok(_ronixaJobManager.GetJobStatus(Guid.Parse("EDF401E5-4FB7-4B98-B6B8-68528F617432")));
        }
    }
}

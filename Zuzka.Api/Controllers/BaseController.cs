using Microsoft.AspNetCore.Mvc;

namespace CodeRama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        private readonly ILogger<T> _logger;

        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected virtual void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        protected virtual void LogError(string message)
        {
            _logger.LogError(message);
        }
        protected virtual void LogCritical(string message)
        {
            _logger.LogCritical(message);
        }
    }
}

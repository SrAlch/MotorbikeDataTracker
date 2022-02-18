using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MotorbikeDataTracker.Api.Repositories;

namespace MotorbikeDataTracker.Api.Controllers
{
    [ApiController]
    [Route("motorbikes")]
    public class MotorbikesController : ControllerBase
    {
        private readonly InMemItemsRepository repository;
        private readonly ILogger<MotorbikesController> logger;

        public MotorbikesController(InMemItemsRepository repository, ILogger<MotorbikesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
    }
}
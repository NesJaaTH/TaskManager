using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.WebApi.Middleware;

namespace TaskManager.WebApi.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemsService _taskItemService;
        private readonly ILogger<TaskItemsController> _logger;

        public TaskItemsController(ITaskItemsService taskItemService, ILogger<TaskItemsController> logger)
        {
            _taskItemService = taskItemService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("{projectId}/tasks")]
        public async Task<IActionResult> CreateTaskItem(Guid projectId, [FromBody] CreateTaskItemsDto request)
        {
            var correlationId = HttpContext.GetCorrelationId();
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                _logger.LogInformation(
                    "Create task item request started: {TaskName} {ProjectId} {UserId} {CorrelationId}",
                    request.Title,
                    projectId,
                    userId,
                    correlationId);
                var result = await _taskItemService.CreateTaskAsync(request, projectId);
                _logger.LogInformation(
                    "Create task item request succeeded: {TaskName} {ProjectId} {UserId} {CorrelationId}",
                    result.Title,
                    projectId,
                    userId,
                    correlationId);
                return Created($"api/project/{projectId}/tasks/{result.Id}", result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Create task item request failed (bad request): {TaskName} {ProjectId} {UserId} {CorrelationId} - {Message}",
                    request.Title,
                    projectId,
                    userId,
                    correlationId,
                    ex.Message);
                return BadRequest(new
                {
                    error = ex.Message,
                    correlationId
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    "Create task item request failed (not found): {TaskName} {ProjectId} {UserId} {CorrelationId} - {Message}",
                    request.Title,
                    projectId,
                    userId,
                    correlationId,
                    ex.Message);
                return NotFound(new
                {
                    error = ex.Message,
                    correlationId
                });
            }
        }
    }
}

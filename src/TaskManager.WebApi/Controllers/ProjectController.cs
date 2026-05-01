using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.WebApi.Middleware;

namespace TaskManager.WebApi.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        //Create a new project
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto request)
        {
            var correlationId = HttpContext.GetCorrelationId();
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                _logger.LogInformation(
                    "Create project request started: {ProjectName} {UserId} {CorrelationId}",
                    request.Name,
                    userId,
                    correlationId);
                var result = await _projectService.CreateProjectAsync(request, Guid.Parse(userId!));
                _logger.LogInformation(
                    "Create project request succeeded: {ProjectName} {UserId} {CorrelationId}",
                    result.Name,
                    userId,
                    correlationId);
                return Created($"api/projects/{result.Id}", result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Create project request failed (bad request): {ProjectName} {UserId} {CorrelationId} - {Message}",
                    request.Name,
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
                _logger.LogError(
                    "Create project request failed (server error): {ProjectName} {UserId} {CorrelationId} - {Message}",
                    request.Name,
                    userId,
                    correlationId,
                    ex.Message);
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred while creating the project.",
                    correlationId
                });
            }

        }
    }
}

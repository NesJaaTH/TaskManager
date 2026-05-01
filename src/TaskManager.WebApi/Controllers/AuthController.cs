using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.WebApi.Middleware;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// Register a new user.
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var correlationId = HttpContext.GetCorrelationId();
            
            try
            {
                _logger.LogInformation(
                    "Register request started: {Email} {CorrelationId}",
                    request.Email,
                    correlationId);

                var result = await _userService.RegisterAsync(request);

                _logger.LogInformation(
                    "Register request succeeded: {Email} {CorrelationId}",
                    request.Email,
                    correlationId);

                return Created($"api/auth/me", result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Register request failed (bad request): {Email} {CorrelationId} - {Message}",
                    request.Email,
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
                    "Register request failed (conflict): {Email} {CorrelationId} - {Message}",
                    request.Email,
                    correlationId,
                    ex.Message);

                return Conflict(new
                {
                    error = ex.Message,
                    correlationId
                });
            }
        }

        /// Login with email and password.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var correlationId = HttpContext.GetCorrelationId();

            try
            {
                _logger.LogInformation(
                    "Login request started: {Email} {CorrelationId}",
                    request.Email,
                    correlationId);

                var token = await _userService.LoginAsync(request);

                _logger.LogInformation(
                    "Login request succeeded: {Email} {CorrelationId}",
                    request.Email,
                    correlationId);

                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(
                    "Login request failed (unauthorized): {Email} {CorrelationId} - {Message}",
                    request.Email,
                    correlationId,
                    ex.Message);

                return Unauthorized(new
                {
                    error = ex.Message,
                    correlationId
                });
            }
        }

/// Get current logged-in user.
[Authorize]
[HttpGet("me")]
public async Task<IActionResult> GetCurrentUser()
{
    var correlationId = HttpContext.GetCorrelationId();
    
    // ดึง user ID จาก JWT token
    // FindFirstValue จะคืนค่า string? (nullable) เพราะว่าอาจไม่มี claim นี้ก็ได้
    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
    // ถ้าไม่มี claim หรือเป็น null → ส่งกลับ 401 Unauthorized
    if (userIdClaim == null)
    {
        _logger.LogWarning(
            "GetCurrentUser failed: no user ID claim in token {CorrelationId}",
            correlationId);

        return Unauthorized(new
        {
            error = "Invalid token: missing user ID",
            correlationId
        });
    }

    // แปลง string เป็น Guid
    var userId = Guid.Parse(userIdClaim);

    _logger.LogInformation(
        "GetCurrentUser request: {UserId} {CorrelationId}",
        userId,
        correlationId);

    var user = await _userService.GetByIdAsync(userId);
    
    return Ok(user);
}
    }
}
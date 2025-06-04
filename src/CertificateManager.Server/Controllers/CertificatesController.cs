using Microsoft.AspNetCore.Mvc;
using CertificateManager.Server.Data;
using Microsoft.EntityFrameworkCore;
using CertificateManager.Shared.Models; // Using shared models

namespace CertificateManager.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly ILogger<CertificatesController> _logger;
    private readonly ApplicationDbContext _context;

    public CertificatesController(ILogger<CertificatesController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCertificatesAsync()
    {
        _logger.LogInformation("Attempting to retrieve all certificates.");
        try
        {
            var certificates = await _context.Certificates
                                             .Include(c => c.SubjectAlternativeNames)
                                             .ToListAsync();
            _logger.LogInformation($"Successfully retrieved {certificates.Count} certificates.");
            return Ok(certificates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving certificates from database.");
            return StatusCode(500, "Internal server error while retrieving certificates.");
        }
    }

    [HttpGet("test")]
    public IActionResult TestConnection()
    {
        _logger.LogInformation("Test connection endpoint called");
        return Ok(new { 
            Status = "Success", 
            Message = "API connection is working!",
            Timestamp = DateTime.UtcNow,
            Version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0"
        });
    }
}
